using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MotoController : MonoBehaviour
{
    private PlayerControls controls;
    private float throttleInput;
    private float brakeInput;
    private float turnInput;
    private float jumpInput;

    [Header("Movement")]
    public float forwardForce = 3000f;
    public float turnTorque = 1000f;
    public float brakeTorque = 100f;

    [Header("jump")]
    public float jumpForce = 50f;
    public float maxJumpDuration = 2f;
    private float jumpTimer = 0f;


    [Header("TurnLeaning")]
    public float LeanStrenght = 500f;
    public float MaxLeanAngle = 45f;
    public float LeanDamping = 5f;

    [Header("ContactPoints")]
    public Transform frontPoint;
    public Transform rearPoint;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    [Header("Levitation")]
    public float LevitationForce = 100f;
    public float DesiredHeight = 1.5f;
    public float LevitationDamping = 10f;
    public float LevitationRayLength = 3f;
    public float gavitationPower = 9.8f;

    [Header("Stab")]
    public float uprightTorque = 500f;
    public float uprightDamp = 5f;

    private Rigidbody rb;    
    private bool grounded;
    private bool canJump;
    private bool isJumping;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();        
        controls = new PlayerControls();

        controls.Vehicle.Throttle.performed += ctx => throttleInput = ctx.ReadValue<float>();
        controls.Vehicle.Throttle.canceled += _ => throttleInput = 0f;

        controls.Vehicle.Jump.performed += ctx => jumpInput = ctx.ReadValue<float>();
        controls.Vehicle.Jump.canceled += _ =>
        {
            jumpInput = 0f;
            isJumping = false; ;
        };

        controls.Vehicle.Brake.performed += ctx => brakeInput = ctx.ReadValue<float>();
        controls.Vehicle.Brake.canceled += _ => brakeInput = 0f;

        controls.Vehicle.Turn.performed += ctx => turnInput = ctx.ReadValue<Vector2>().x;
        controls.Vehicle.Turn.canceled += _ => turnInput = 0f;
        
    }

     void OnEnable() => controls.Vehicle.Enable();
    private void OnDisable() => controls.Vehicle.Disable();   

    private void FixedUpdate()
    {
        grounded = IsGrounded();

        Vector3 rotationTorque = Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime;
                
        ApplyLevitationForce(frontPoint);
        ApplyLevitationForce(rearPoint);

        rb.AddForce(transform.forward * throttleInput * forwardForce * Time.fixedDeltaTime);
        
              
        ApplyLeanTorque();
        ApplyUprightTorque(rotationTorque);

        if (jumpInput > 0f && !isJumping && canJump)
        {
            isJumping = true;
            jumpTimer = 0f;
        }
        if (isJumping)
        {
            if (jumpTimer < maxJumpDuration)
            {
                rb.AddForce(Vector3.up);
            }
        }
    }

    private bool IsGrounded()
    {
       
        bool frontGrounded = Physics.Raycast(frontPoint.position, Vector3.down, groundCheckDistance, groundLayer);
        bool rearGrounded = Physics.Raycast(rearPoint.position, Vector3.down, groundCheckDistance, groundLayer);
        return frontGrounded || rearGrounded;
    }    

    private void ApplyUprightTorque(Vector3 rotation)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out RaycastHit depthHit, LevitationRayLength, groundLayer);       
              
        Vector3 up = transform.up; 
        Vector3 desiredUp = depthHit.normal == Vector3.zero ? Vector3.up : depthHit.normal;       
        Vector3 torqueVector = Vector3.Cross(up, desiredUp);
        rb.AddTorque(torqueVector * uprightTorque - rb.angularVelocity * uprightDamp + rotation);
    }

    private void ApplyLevitationForce(Transform LevitationPoint)
    {
        Ray ray = new Ray(LevitationPoint.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, LevitationRayLength, groundLayer))
        {
            float currentHeight = hit.distance;
            float heightError = DesiredHeight - currentHeight;
            float springforce = Mathf.Max(0f, heightError * LevitationForce);

            Vector3 pointVelocity = rb.GetPointVelocity(LevitationPoint.position);
            float verticalVelocity = Vector3.Dot(pointVelocity, Vector3.down);
            float dampingForce = verticalVelocity > 0 ? verticalVelocity * LevitationDamping : 0f;

            Vector3 totalForce = Vector3.up * (springforce - dampingForce);
            rb.AddForceAtPosition(totalForce, LevitationPoint.position);

            Debug.DrawRay(LevitationPoint.position, Vector3.down * currentHeight, Color.green);


        }
        else
        {
            Debug.DrawRay(LevitationPoint.position, Vector3.down, Color.red);
        }        
    }
    
        private void ApplyLeanTorque()
    {
        float targetLeanAngle = -turnInput * MaxLeanAngle;
        float currentLeanAngle = Vector3.SignedAngle(transform.up, Vector3.up, transform.forward);
        float leanError = targetLeanAngle - currentLeanAngle;
        float angularLeanVelocity = Vector3.Dot(rb.angularVelocity, transform.forward);
        float leanTorque = leanError * LeanStrenght - angularLeanVelocity * LeanDamping;
              

            rb.AddTorque(transform.forward * leanTorque);        
    }
   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(frontPoint.position, frontPoint.position - frontPoint.transform.up * groundCheckDistance);
    }
}
