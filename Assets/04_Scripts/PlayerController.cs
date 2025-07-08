using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MotoController : MonoBehaviour
{
    private PlayerControls controls;
    private float throttleInput;
    private float brakeInput;
    private float turnInput;

    [Header("Movement")]
    public float forwardForce = 3000f;
    public float turnTorque = 1000f;
    public float brakeTorque = 100f;

    [Header("ContactPoints")]
    public Transform frontPoint;
    public Transform rearPoint;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Stab")]
    public float uprightTorque = 500f;
    public float uprightDamp = 5f;

    private Rigidbody rb;    
    private bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Vehicle.Throttle.performed += ctx => throttleInput = ctx.ReadValue<float>();
        controls.Vehicle.Throttle.canceled += _ => throttleInput = 0f;

        controls.Vehicle.Brake.performed += ctx => brakeInput = ctx.ReadValue<float>();
        controls.Vehicle.Brake.canceled += _ => brakeInput = 0f;

        controls.Vehicle.Turn.performed += ctx => turnInput = ctx.ReadValue<Vector2>().x;
        controls.Vehicle.Turn.canceled += _ => turnInput = 0f;
        
    }

     void OnEnable() => controls.Vehicle.Enable();
    private void OnDisable() => controls.Vehicle.Disable();   

    void FixedUpdate()
    {
        grounded = IsGrounded();

        Vector3 rotationTorque = Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime;

        Debug.Log("rotation " +  rotationTorque);

        if (grounded)
        {
            rb.AddForce(transform.forward * throttleInput * forwardForce * Time.fixedDeltaTime);
            //rb.AddTorque(Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime);            
        }
        else
        {
            //rb.AddTorque(Vector3.up * turnInput * Time.fixedDeltaTime);            
        }

        ApplyUprightTorque(rotationTorque);
    }

    bool IsGrounded()
    {
       
        bool frontGrounded = Physics.Raycast(frontPoint.position, Vector3.down, groundCheckDistance, groundLayer);
        bool rearGrounded = Physics.Raycast(rearPoint.position, Vector3.down, groundCheckDistance, groundLayer);
        return frontGrounded || rearGrounded;
    }

    void ApplyUprightTorque(Vector3 rotation)
    {
        Vector3 up = transform.up;
        Vector3 desiredUp = Vector3.up;
        Vector3 torqueVector = Vector3.Cross(up, desiredUp);
        rb.AddTorque(torqueVector * uprightTorque - rb.angularVelocity * uprightDamp + rotation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(frontPoint.position, frontPoint.position - frontPoint.transform.up * groundCheckDistance);
    }
}
