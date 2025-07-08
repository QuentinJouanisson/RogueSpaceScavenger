using UnityEngine;


namespace Controller
{
    


    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        private float forwardForce = 10f;
        private float TurnTorque = 1f;
        private float breakingForce = 1f;
        private float jumpingForce = 1f;

        [Header("ContactPoints")]
        public Transform frontPoint;
        public Transform rearPoint;
        public float groundCheckDistance = 0.5f;
        public LayerMask groundLayer;

        [Header("Stability")]
        public float uprightTorque = 5f;
        public float uprightDamping = 1f;

        private Rigidbody rb;

        private float inputVertical;
        private float inputHorizontal;
        private bool grounded;

        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            inputVertical = Input.GetAxis("Vertical");
            inputHorizontal = Input.GetAxis("Horizontal");

        }

        void FixedUpdate()
        {
            grounded = IsGrounded();
            
            if (grounded)
            {
                rb.AddForce(transform.forward * inputVertical * forwardForce * Time.fixedDeltaTime);
                rb.AddTorque(Vector3.up * inputHorizontal * TurnTorque * Time.fixedDeltaTime);
            }
            else
            {
                rb.AddTorque(Vector3.up * inputHorizontal * breakingForce * Time.fixedDeltaTime);
            }

            ApplyUprightTorque();

        }

        bool IsGrounded()
        {
            bool frontGrounded = Physics.Raycast(frontPoint.position, Vector3.down, groundCheckDistance, groundLayer);
            bool rearGrounded = Physics.Raycast(rearPoint.position, Vector3.down, groundCheckDistance, groundLayer);
            return frontGrounded || rearGrounded;
        }

        void ApplyUprightTorque()
        {
            Vector3 up = transform.up;
            Vector3 desiredUp = Vector3.up;
            Vector3 torqueVector = Vector3.Cross(up, desiredUp);
            rb.AddTorque(torqueVector * uprightTorque - rb.angularVelocity * uprightDamping);
        }
    }
}
