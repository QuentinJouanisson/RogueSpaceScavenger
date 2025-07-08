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

        }

        public async void Progress()
        {
            while (true)
            {
                await Awaitable.NextFrameAsync();
            }
        }

    }
}
