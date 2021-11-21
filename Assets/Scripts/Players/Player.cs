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
        
        [SerializeField] private float jumpForce = 400f;
        [SerializeField] private float movementSpeed = 10f;
        
        private Camera mainCamera = null;
        private CharacterController controller = null;
        // private Animator animator = null;

        private bool isPlayerIdle = true;

        // private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        // private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            //animator = GetComponent<Animator>();
        }

        public void Start()
        {
            playerInventory.TryAddItem(ItemId.THORNS, 5);
        }

        void Update()
        {
            isPlayerIdle = true;
            CheckPlayerMovement();
            CheckForMouseInput();
            CheckPlayerJump();
            CheckPlayerIdle();
        }

        public void ApplyPlayerData(PlayerData playerData)
        {
            transform.position = new Vector3(playerData.X, playerData.Y, playerData.Z);
            this.playerInventory = playerData.PlayerInventory;
            this.playerInventory.IsDirty = true;
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
            
            if (Input.GetButtonDown("ToggleInventory"))
            {
                EiramEvents.OnPlayerToggleInventory(playerInventory);
            }
        }
        
        private void CheckForMouseInput()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var mousePos = GetMousePosition();
                var tilePos = ConvertPositionToTile(mousePos);
                World.Current.RemoveTileAt(tilePos);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                // check current item
                var inHandStack = playerInventory.PeekSelectedItem();
                var mousePos = GetMousePosition();
                var tilePos = ConvertPositionToTile(mousePos);
                
                if (!inHandStack.IsEmpty())
                {
                    // if placed remove item in hand
                    if (World.Current.PlaceTileAt(tilePos, Register.GetItemById(inHandStack.ItemId).tileId))
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
                controller.Jump(jumpForce);
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
        public PlayerInventory PlayerInventory;
    }
}
