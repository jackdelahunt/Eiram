using Eiram;
using UnityEngine;
using Worlds;

namespace Players
{
    [RequireComponent(typeof(CharacterController))]
    //[RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
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

        void Update()
        {
            isPlayerIdle = true;
            CheckPlayerMovement();
            CheckForMouseInput();
            CheckPlayerJump();
            CheckPlayerIdle();
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
                World.Current.RemoveTileAt(tilePos);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                var mousePos = GetMousePosition();
                var tilePos = ConvertPositionToTile(mousePos);
                World.Current.PlaceTileAt(tilePos, TileId.BEDROCK);
            }
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
    }
}