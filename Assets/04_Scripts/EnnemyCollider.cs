using UnityEngine;

namespace Ennemy
{

    public class EnnemyCollider : MonoBehaviour
    {
        private BettleAnimatorController parentController;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            parentController = GetComponentInParent<BettleAnimatorController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("CollisionFromPlayer");
                parentController.OnPlayerCollision(other);
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
