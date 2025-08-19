using UnityEngine;

namespace Ennemy
{

    public class EnnemyDamageSphere : MonoBehaviour
    {
        private string playerTag = "Player";
        private float entryDamage = 20f;
        private float periodicDamage = 5f;
        private float damageInterval = 0.25f;
        private float ennemyDamageMod = 1f;

        private float damageTimer = 0f;
        private bool playerInside = false;
        private PlayerHealth playerHealth;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInside = true;

                playerHealth = other.GetComponent<PlayerHealth>();
                if(playerHealth != null)
                {
                    playerHealth.TakeDamage(entryDamage * ennemyDamageMod);

                }

                damageTimer = 0f;

            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(playerInside && playerHealth != null)
            {
                damageTimer += Time.deltaTime;
                if(damageTimer >= damageInterval)
                {
                    playerHealth.TakeDamage(periodicDamage * ennemyDamageMod);
                    damageTimer = 0f;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInside = false; 
                playerHealth = null; 
            }
        }




        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
