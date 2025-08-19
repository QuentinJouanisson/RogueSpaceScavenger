using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class MotoController : MonoBehaviour
{
    private PlayerControls controls;

    private ParticleSystem impactParticles;
    private ParticleSystem prolongedImpactParticles;
    private ParticleSystem jumpParticles;
    public float throttleInput;
    public float brakeInput;
    public float turnInput;
    public float jumpInput;

    [Header("UI")]
    public Slider HoverSlider;

    [Header("Movement")]
    public float forwardForce = 3000f;
    public float turnTorque = 1000f;
    public float brakeTorque = 1f;

    [Header("HoverThruster")]
    public float hoverThrustPower = 50f;
    public float maxHoverThrustPower = 2f;
    public float hoverThrustRechargeRate = 1f;
    public float hoverThrustConsumptionRate = 1f;

    private float currentHoverThrusterPower;
    private bool isHoverThrusting = false;


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
    
    
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();        
        controls = new PlayerControls();
        currentHoverThrusterPower = maxHoverThrustPower;

        controls.Vehicle.Throttle.performed += ctx => throttleInput = ctx.ReadValue<float>();
        controls.Vehicle.Throttle.canceled += _ => throttleInput = 0f;

        controls.Vehicle.Jump.performed += ctx => jumpInput = ctx.ReadValue<float>();
        controls.Vehicle.Jump.canceled += _ =>
        {
            jumpInput = 0f;            
        };

        controls.Vehicle.Brake.performed += ctx => brakeInput = ctx.ReadValue<float>();
        controls.Vehicle.Brake.canceled += _ => brakeInput = 0f;

        controls.Vehicle.Turn.performed += ctx => turnInput = ctx.ReadValue<Vector2>().x;
        controls.Vehicle.Turn.canceled += _ => turnInput = 0f;
        
    }

     void OnEnable() => controls.Vehicle.Enable();
    private void OnDisable() => controls.Vehicle.Disable();
    private void Update()
    {
        if (HoverSlider != null)
        {
            HoverSlider.value = (currentHoverThrusterPower/ maxHoverThrustPower) * HoverSlider.maxValue;
        }
    }

    private void FixedUpdate()
    {
        grounded = IsGrounded();

        Vector3 rotationTorque = Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime;
                
        ApplyLevitationForce(frontPoint);
        ApplyLevitationForce(rearPoint);

        if (brakeInput <= 0f)
        {
            rb.AddForce(transform.forward * throttleInput * forwardForce * Time.fixedDeltaTime);
        }
        else
        {
            //ApplyBrakeForce();
            SecondBrake();
        }

        ApplyLeanTorque();
        ApplyUprightTorque(rotationTorque);

        if (jumpInput > 0f && currentHoverThrusterPower > 0f)
        {
            isHoverThrusting = true;

            if (jumpParticles != null)
            {
                jumpParticles.Play();
            }
        }
        else
        {
            isHoverThrusting = false;
            if(jumpParticles != null && jumpParticles.isPlaying)
            {
                jumpParticles.Stop();
            }
        }

        if (isHoverThrusting)
        {
            rb.AddForce(Vector3.up * hoverThrustPower, ForceMode.Acceleration);
            currentHoverThrusterPower -= hoverThrustConsumptionRate * Time.fixedDeltaTime;
            currentHoverThrusterPower = Mathf.Max(0f, currentHoverThrusterPower);
        }

        if (grounded && !isHoverThrusting && currentHoverThrusterPower < maxHoverThrustPower)
        {
            currentHoverThrusterPower += hoverThrustRechargeRate * Time.fixedDeltaTime;
            currentHoverThrusterPower = Mathf.Min(maxHoverThrustPower, currentHoverThrusterPower);
        }
    }

    private bool IsGrounded()
    {       
        bool frontGrounded = Physics.Raycast(frontPoint.position, Vector3.down, groundCheckDistance, groundLayer);
        bool rearGrounded = Physics.Raycast(rearPoint.position, Vector3.down, groundCheckDistance, groundLayer);
        return frontGrounded || rearGrounded;
    }    
    private void ApplyBrakeForce()
    {
       float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        if (forwardSpeed > 0.5f )
        {
            Vector3 forwardVelocity = transform.forward * forwardSpeed;
            Vector3 brakeForce = -forwardVelocity * brakeInput * brakeTorque;
            rb.AddForce(brakeForce, ForceMode.Force);
        }           
    }
    private void SecondBrake()
    {
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 10);
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
    private void OnCollisionEnter(Collision collision)
    {
        EmitContactParticles(collision);
        EmitProlongedContactParticles(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        impactParticles.Stop();
        prolongedImpactParticles.Stop();
    }
    private void EmitContactParticles(Collision collision)
    {

        if (impactParticles == null) return;
        ContactPoint contact = collision.contacts[0];
        impactParticles.transform.position = contact.point;
        impactParticles.transform.rotation = Quaternion.LookRotation(contact.normal);

        impactParticles.Play();
    }
    private void EmitProlongedContactParticles(Collision collision)
    {
        if (prolongedImpactParticles == null) return;
        ContactPoint contact = collision.contacts[0];
        prolongedImpactParticles.transform.position = contact.point;
        prolongedImpactParticles.transform.rotation = Quaternion.LookRotation(contact.normal);

        prolongedImpactParticles.Play();
    }
}
