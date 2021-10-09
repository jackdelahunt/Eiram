using UnityEngine;
using UnityEngine.Events;

namespace Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController : MonoBehaviour
    {
        [Range(0, 0.3f)] [SerializeField] private float movementSmoothing = .05f;
        [Range(0, 1f)] [SerializeField] private float airSpeedMultiplier = .5f;
        [Range(0, 0.1f)][SerializeField]private float groundedRadius = 0.01f;
        [SerializeField] private bool isAirControlling = false;
        [SerializeField] private LayerMask whatIsGround = new LayerMask();
        [SerializeField] private Transform groundCheck = null;
        
        private bool isGrounded;
        private Rigidbody2D thisRigidbody;
        private bool isFacingRight = true;
        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {
            thisRigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            isGrounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                    break;
                }
            }
        }


        /*
         * Moves player along the x axis with the given force move
         * Negative move moves left
         * Positive move moves right
         */
        public void Move(float move)
        {
            if (isGrounded || isAirControlling)
            {
                if (!isGrounded)
                    move *= airSpeedMultiplier;

                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * 10f, thisRigidbody.velocity.y);
                // And then smoothing it out and applying it to the character
                thisRigidbody.velocity = Vector3.SmoothDamp(thisRigidbody.velocity, targetVelocity, ref velocity, movementSmoothing);


                if (move > 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (move < 0 && isFacingRight)
                {
                    Flip();
                }
            }
        }

        public void Jump(float jumpForce)
        {
            jumpForce *= 50;
            if (isGrounded)
            {
                isGrounded = false;
                thisRigidbody.AddForce(new Vector2(0f, jumpForce));
            }
        }


        private void Flip()
        {
            isFacingRight = !isFacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
