using System;
using Eiram;
using Events;
using Inventories;
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
        [SerializeField] private float movementSpeed = 10f;

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
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            //animator = GetComponent<Animator>();
        }

        public void Start()
        {
            playerInventory.TryAddItem(ItemId.WOOD_SHOVEL, 1);
            playerInventory.TryAddItem(ItemId.WOOD_AXE, 1);
            playerInventory.TryAddItem(ItemId.WOOD_PICKAXE, 1);
            playerInventory.TryAddItem(ItemId.CHEST, 5);
        }

        public void OnDestroy()
        {
            EiramEvents.PlayerInventoryRequestEvent -= OnPlayerInventoryRequest;
            EiramEvents.ToolBreakEvent -= OnToolBreak;
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
            }

            CheckPlayerUIInteraction();
        }

        public void ApplyPlayerData(PlayerData playerData)
        {
            transform.position = new Vector3(playerData.X, playerData.Y, playerData.Z);
            this.playerInventory = playerData.PlayerInventory;
            this.hunger = playerData.hunger;
            this.playerInventory.IsDirty = true;
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
            if (Input.GetButtonDown("Fire1"))
            {
                var mousePos = GetMousePosition();
                var tilePos = ConvertPositionToTile(mousePos);
                World.Current.RemoveTileAtAsPlayer(tilePos, playerInventory.PeekSelectedItem(), this);
            }

            
            if (Input.GetButtonDown("Fire2"))
            {
                var inHandStack = playerInventory.PeekSelectedItem();
                var mousePos = GetMousePosition();
                var tilePos = ConvertPositionToTile(mousePos);

                var tileData = World.Current.GetTileData(tilePos).Unwrap();
                if (inHandStack.IsEmpty() || tileData.TileId != TileId.AIR)
                {
                    World.Current.UseTileAt(tilePos, this);
                    return;
                }
                
                var item = Register.GetItemByItemId(inHandStack.ItemId);
                if (item.TileId() == TileId.UNKNOWN)
                {
                    World.Current.UseTileAt(tilePos, this);
                }
                else
                {
                    // placing the item, pop item from inventory
                    var poppedItem = playerInventory.PopSelectedItem();
                    World.Current.PlaceTileAt(tilePos, Register.GetItemByItemId(poppedItem.ItemId).TileId());
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
                controller.Jump(jumpForce);
                ChangeHunger(-10);
            }
        }

        /*
         * checks if the the player has moved or jumped
         * this frame
         */
        private void CheckPlayerIdle()
        {
            if (isPlayerIdle)
            {
                // animator.SetBool(IsWalking, false);
                // animator.SetBool(IsJumping, false);
            }
        }

        private void ChangeHunger(int delta)
        {
            hunger += delta;
            EiramEvents.OnPlayerChangedHungerEvent(hunger);
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
                EiramEvents.OnPlayerToggleNotebook();
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
