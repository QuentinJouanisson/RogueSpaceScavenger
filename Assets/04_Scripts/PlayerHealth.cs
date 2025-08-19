using UnityEngine;



    public class PlayerHealth : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float currentHealth;
        private MotoController motoController;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            currentHealth = maxHealth;
            motoController = GetComponent<MotoController>();

            if(motoController == null)
            {
                Debug.LogWarning("PlayerHealth no motocontrollerFound");
            }
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log("Player took " + amount + "damages");

            if (currentHealth < 0)
            {
                Die();
            }
        }

        private void Die()
        {            
            //Debug.Log("Player died");
            
            if(motoController != null)
            {
                motoController.OnDeath();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

