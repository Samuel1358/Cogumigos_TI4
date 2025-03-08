using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;


    public class TEMP_PlayerMovement : MonoBehaviour
    {
        [Header("External Access")]
        [SerializeField] private Transform orientation;

        [Header("Movement")]
        [SerializeField] private float moveSpd;
        [SerializeField] private float groundDrag;
        [SerializeField] private float airDrag;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float airMultiplier;
        [SerializeField] private float gravityForce;
        [SerializeField] private int jumpCount;
        private int _jumpCount;
        private bool readyToJump = true;
        private bool onJump;

        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask ground;

        public UnityEvent onPlayerMove;

        private bool grounded;

        private float horizontalInput;
        private float verticalInput;

        Vector3 moveDirection;

        private Rigidbody rb;

        private void Awake()
        {
            _jumpCount = jumpCount;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        // Metodos Publicos
        public void MovementUpdate(float horizontal, float vertical)
        {
            PlayerInput(horizontal, vertical);
            GroundCheck();
            SpeedControl();
        }

        public void PlayerMove()
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            float speed = 0;
            if (grounded)
                speed = moveSpd * 10f;
            else
            {
                speed = moveSpd * airMultiplier * 10f;
                rb.AddForce(gravityForce * 10f * Vector3.down, ForceMode.Force);
            }               

            rb.AddForce(speed * moveDirection.normalized, ForceMode.Force);

            onPlayerMove.Invoke();
        }

        public void JumpUpdate(bool jumpInput)
        {
            if (jumpInput && readyToJump && _jumpCount > 0)
            {
                readyToJump = false;
                onJump = true;
                _jumpCount--;

                Jump(jumpForce);

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        public void JumpImpulse(float impulse)
        {
            Jump(impulse);
        }

        // Metodos Privados
        private void PlayerInput(float horizontal, float vertical)
        {
            horizontalInput = horizontal;
            verticalInput = vertical;
        }

        private void GroundCheck()
        {
            grounded = Physics.Raycast(transform.position + Vector3.up * (playerHeight * 0.5f), Vector3.down, playerHeight * 0.5f + 0.2f, ground);

            if (grounded && !onJump)
            {
                rb.linearDamping = groundDrag;
                _jumpCount = jumpCount;
            }
            else
                rb.linearDamping = airDrag;
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpd)
            {
                Vector3 limitVel = moveSpd * flatVel.normalized;
                rb.linearVelocity = new Vector3(limitVel.x, rb.linearVelocity.y, limitVel.z);
            }
        }

        private void Jump(float impulse)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(impulse * transform.up, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyToJump = true;
            onJump = false;
        }
    }