using System;
using Eiram;
using Events;
using Effects;
using Inventories;
using IO;
using Items;
using Registers;
using UnityEngine;
using Worlds;

namespace Players
{
    [RequireComponent(typeof(CharacterController))]
    //[RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public int hunger = 100;
        
        [SerializeField] private float jumpForce = 400f;
        [SerializeField] private float maxJumpForce = 400f;
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float maxMovementSpeed = 10f;

        private bool inInventory = false;
        private bool inNotebook = false;
        
        private Camera mainCamera = null;
        private CharacterController controller = null;
        // private Animator animator = null;

        private bool isPlayerIdle = true;

        // private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        // private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Awake()
        {
            EiramEvents.PlayerInventoryRequestEvent += OnPlayerInventoryRequest;
            EiramEvents.ToolBreakEvent += OnToolBreak;
            EiramEvents.SaveToDiskRequestEvent += SaveData;
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            //animator = GetComponent<Animator>();
        }

        public void Start()
        {
            EiramEvents.OnPlayerChangedHungerEvent(hunger);
            
            playerInventory.TryAddItem(ItemId.WOOD_SHOVEL, 1);
            playerInventory.TryAddItem(ItemId.CRANBERRIES, 5);
            playerInventory.TryAddItem(ItemId.STICK, 5);
            
            TryApplySave();
        }

        public void OnDestroy()
        {
            EiramEvents.PlayerInventoryRequestEvent -= OnPlayerInventoryRequest;
            EiramEvents.ToolBreakEvent -= OnToolBreak;
            EiramEvents.SaveToDiskRequestEvent -= SaveData;
        }

        void Update()
        {
            isPlayerIdle = true;
            if (!(inInventory || inNotebook))
            {
                CheckPlayerMovement();
                CheckForMouseInput();
                CheckPlayerJump();
                CheckPlayerIdle();
                CheckPlayerHunger();
            }

            CheckPlayerUIInteraction();
        }
        
        public bool ChangeHunger(int delta)
        {
            int startHunger = hunger;
            
            hunger += delta;
            
            if (hunger <= 0)
                hunger = 0;

            if (hunger >= 100)
                hunger = 100;
            
            if(startHunger != hunger)
            {
                EiramEvents.OnPlayerChangedHungerEvent(hunger);
                return true;
            }

            return false;
        }

        private void TryApplySave()
        {
            var loadResult = Filesystem.LoadFrom<PlayerData>("player.data", World.Current.Save.Data);
            if (loadResult.IsSome(out var playerData))
            {
                transform.position = new Vector3(playerData.X, playerData.Y, playerData.Z);
                this.playerInventory = playerData.PlayerInventory;
                this.hunger = playerData.hunger;
                this.playerInventory.IsDirty = true;
            }
        }

        private void OnPlayerInventoryRequest()
        {
            EiramEvents.OnPlayerTogglePlayerInventory(playerInventory);
            inInventory = !inInventory;
        }

        private void OnToolBreak(ItemStack itemStack)
        {
            var index = playerInventory.SlotOfStack(itemStack);
            if (index.IsSome(out var i))
            {
                playerInventory.ClearSlot(i);
            }
        }

        /*
         * checks for player movement an invokes the
         * player movement event or
         */
        private void CheckPlayerMovement()
        {
            if (Input.GetButton("Horizontal"))
            {
                isPlayerIdle = false;
                controller.Move(Input.GetAxisRaw("Horizontal") * movementSpeed);
            }
        }
        
        private void CheckForMouseInput()
        {
            var mousePos = GetMousePosition();
            var tileWorldPos = ConvertPositionToTile(mousePos);

            if (World.Current.GetTileData(tileWorldPos).IsSome(out var tileDataAtMouse))
            {
                if (tileDataAtMouse.TileId != TileId.AIR && !inInventory && !inNotebook)
                    EiramEvents.OnTileInfoRequestEvent(tileDataAtMouse);
            }
            
            if (Input.GetButtonDown("Fire1"))
            {
                World.Current.RemoveTileAtAsPlayer(tileWorldPos, playerInventory.PeekSelectedItem(), this);
            }

            
            if (Input.GetButtonDown("Fire2"))
            {
                var inHandStack = playerInventory.PeekSelectedItem();

                // use tile if empty
                if (inHandStack.IsEmpty())
                {
                    World.Current.UseTileAt(tileWorldPos, this);
                    return;
                }
                
                // if can place item then place
                var item = Register.GetItemByItemId(inHandStack.ItemId);
                var tileData = World.Current.GetTileData(tileWorldPos).Unwrap();
                if (item.TileId() != TileId.UNKNOWN && tileData.TileId == TileId.AIR)
                {
                    World.Current.PlaceTileAt(tileWorldPos, Register.GetItemByItemId(playerInventory.PopSelectedItem().ItemId).TileId());
                    return;
                }
                
                // if tile is usable then do not use item, else use item
                if(!World.Current.UseTileAt(tileWorldPos, this))
                {
                    if (item.OnUse(tileWorldPos, inHandStack, this))
                        playerInventory.PopSelectedItem();
                }
            }

            float scrollAmount = Input.GetAxisRaw("Scroll"); 
            if (scrollAmount > 0)
                playerInventory.SelectPrevious();
            else if (scrollAmount < 0)
                playerInventory.SelectNext();
        }

        /*
         * checks for player movement an invokes the
         * player movement event or
         */
        private void CheckPlayerJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                isPlayerIdle = false;
                if (controller.Jump(jumpForce))
                    ChangeHunger(-10);
            }
        }

        private void CheckPlayerIdle()
        {
            if (isPlayerIdle)
            {
                // animator.SetBool(IsWalking, false);
                // animator.SetBool(IsJumping, false);
            }
        }

        private void CheckPlayerHunger()
        {
            if (hunger == 0)
            {
                movementSpeed = maxMovementSpeed * 0.7f;
                jumpForce = maxJumpForce * 0.85f;
                PostProcessing.instance.Vignette(0.5f);
            }
            else
            {
                movementSpeed = maxMovementSpeed;
                jumpForce = maxJumpForce;
                PostProcessing.instance.ResetVignette();
            }
        }

        private void SaveData()
        {
            Filesystem.SaveTo(SerializableData(), "player.data", World.Current.Save.Data);
        }

        /*
         * returns a position of a the tile
         * where the players mouse is
         */
        private Vector3Int ConvertPositionToTile(Vector3 position)
        {
            return new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0);
        }

        /*
         * returns the coords of the players mouse
         */
        private Vector3 GetMousePosition()
        {
            return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -mainCamera.transform.position.z));
        }

        private void CheckPlayerUIInteraction()
        {
            if (Input.GetButtonDown("ToggleInventory"))
            {
                EiramEvents.OnPlayerTogglePlayerInventory(playerInventory);
                EiramEvents.OnPlayerInteractEvent();
                inInventory = !inInventory;
            }
            
            if (Input.GetButtonDown("ToggleNotebook"))
            {
                EiramEvents.OnPlayerToggleNotebookEvent();
                inNotebook = !inNotebook;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("ItemEntity"))
            {
                var go = other.gameObject;
                var entity = go.GetComponent<ItemEntity>();
                int remainingSize = playerInventory.TryAddItem(entity.ItemId, entity.Size);
                if (remainingSize > 0)
                    entity.Size = remainingSize;
                else
                    Destroy(go);
            }
        }
        
        public PlayerData SerializableData()
        {
            var position = transform.position;
            return new PlayerData
            {
                X = position.x,
                Y = position.y,
                Z = position.z,
                hunger = hunger,
                PlayerInventory = playerInventory
            };
        }
    }

    [Serializable]
    public class PlayerData
    {
        public float X;
        public float Y;
        public float Z;
        public int hunger;
        public PlayerInventory PlayerInventory;
    }
}
