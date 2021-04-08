using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Movement
{
    public class PlayerMovement : Entity
    {
        #region PUBLIC FIELDS
        [Header(header: "Aim Settings")]
        public float lookSpeed = 2.0f;
        public Transform cam;
        public float turnSmoothTime = 0.1f;

        [Header(header: "Walk / Run Settings")]
        public float walkSpeed;
        public float runSpeed;

        [Header(header: "Jump Settings")]
        public float playerJumpForce;
        public ForceMode appliedForceMode;

        [Header(header: "Jumping State")]
        public bool playerIsJumping;

        [Header(header: "Current Player Speed")]
        public float currentSpeed;

        [Header(header: "Ground LayerMask name")]
        public string groundLayerMask;

        [Header(header: "Raycast Distance")]
        public float raycastDistance;

        [Header(header: "Panel")]
        public Image Panel;

        #endregion

        #region PRIVATE FIELDS
        private PlayerControls controls;
        private CharacterController controller;
        private Vector3 direction;
        //private GameManager gm;
        RaycastHit[] hitInfo;
        float turnSmoothVelocity;
        private float m_xAxis;
        private float m_zAxis;
        private Rigidbody m_rb;
        private RaycastHit m_hit;
        private Vector3 m_groundLocation;
        private bool m_leftShiftPressed;
        private int m_groundLayerMask;
        public float m_distanceFromPlayerToGround;
        public bool m_playerIsGrounded;
        public bool m_playerJumpStarted;
        //private Camera cam;
        private float rotationX;
        private float lookXLimit = 90.0f;
        private float healRate = 10.0f;
        private float elapsedHitTime;
        private Animator animator;
        
        #endregion

        #region jump presets

        public const int MaxAllowJump = 2; //maximum allowed jumps
        public int m_currentNumberOfJumpsMade; //current number of jumps processed
        #endregion

        #region MONODEVELOP ROUTINES
        private void Awake()
        {
            //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            controls = new PlayerControls();
            //controller = GetComponent<CharacterController>();
        }
        private void OnEnable()
        {
            controls.Enable();
            controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        }
        private void OnDisable()
        {
            controls.Disable();
            controls.Player.Move.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        }
        private void Move(Vector2 input)
        {
            direction = new Vector3(input.x, 0, input.y).normalized;
        }
        protected override void Start()
        {
            #region initializing components

            base.Start();
            m_rb = GetComponent<Rigidbody>();
            //cam = transform.GetChild(0).GetComponent<Camera>();
            animator = transform.GetChild(1).GetComponent<Animator>();

            #endregion
            #region get layer mask [env ground layer]

            m_groundLayerMask = LayerMask.GetMask(groundLayerMask);

            #endregion
            #region jump press initiated

            m_playerJumpStarted = true;

            #endregion
        }

        private void Update()
        {
            #region controller Input [horizontal | vertical ] movement

            m_xAxis = Input.GetAxis("Horizontal");
            animator.SetFloat("Xpos", m_xAxis);
            m_zAxis = Input.GetAxis("Vertical");
            animator.SetFloat("Ypos", m_zAxis);

            #endregion
            #region adjust player move speed [walk | run]

            currentSpeed = m_leftShiftPressed ? runSpeed : walkSpeed;

            #endregion

            #region Camera Movement

            //rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            //rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            //cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            //if (direction.magnitude >= 0.1f)
            //{
            //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //    transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //    controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            //}

            #endregion

            #region Health Regeneration
            if (hp < maxHealth)
            {
                elapsedHitTime += Time.deltaTime;
                hp += healRate * elapsedHitTime * Time.deltaTime;
                //Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, 1 - (hp / maxHealth));
            }
            if (hp > maxHealth)
            {
                hp = maxHealth;
                //Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, 1 - (hp / maxHealth));
            }
            #endregion

            #region play jump input

            playerIsJumping = Input.GetButton("Jump");

            #endregion
            #region Shift To Change Speed

            m_leftShiftPressed = Input.GetKey(KeyCode.LeftShift);

            #endregion

            #region compute player distance from ground
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastDistance, Color.blue); 
            //added layermask for those dealing with complex ground objects.
            if (Physics.SphereCast(transform.position, 0.9f, transform.TransformDirection(Vector3.down), out m_hit, Mathf.Infinity))
            {
                m_groundLocation = m_hit.point;
                m_distanceFromPlayerToGround = transform.position.y - m_groundLocation.y;
                if (m_distanceFromPlayerToGround >= 1.1f)
                {
                    playerIsJumping = false;
                }
            }
            #endregion
        }
        private void FixedUpdate()
        {
            #region move player

            m_rb.MovePosition(transform.position + Time.deltaTime * currentSpeed * transform.TransformDirection(m_xAxis, 0f, m_zAxis));

            #endregion

            #region apply single / double jump

            m_playerIsGrounded = m_distanceFromPlayerToGround <= 1.1f;

            if (playerIsJumping && m_playerJumpStarted && (m_playerIsGrounded || MaxAllowJump > m_currentNumberOfJumpsMade))
                StartCoroutine(ApplyJump());

            if (m_playerIsGrounded)
                m_currentNumberOfJumpsMade = 0;

            #endregion
        }
        #region Base Entity Overrides
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            elapsedHitTime = 0;
        }
        protected override void Die()
        {
            base.Die();
            gm.GameOver();
        }
        public override void RagdollState(bool toggle)
        {

        }
        #endregion

        #endregion

        #region HELPER ROUTINES

        /// <summary>
        /// applies force in the upward direction
        /// using jump force and supplied force mode
        /// </summary>
        /// <param name="jumpForce"></param>
        /// <param name="forceMode"></param>

        private void PlayerJumps(float jumpForce, ForceMode forceMode)
        {
            m_rb.AddForce(jumpForce * m_rb.mass * Time.deltaTime * Vector3.up, forceMode);
        }

        /// <summary>
        /// handles single and double jump
        /// waits until space bar pressed is terminated before
        /// next jump is initiated
        /// </summary>
        private IEnumerator ApplyJump()
        {
            PlayerJumps(playerJumpForce, appliedForceMode);
            m_playerIsGrounded = false;
            m_playerJumpStarted = false;
            yield return new WaitUntil(() => !playerIsJumping);
            ++m_currentNumberOfJumpsMade;
            m_playerJumpStarted = true;
        }
        #endregion
    }
}