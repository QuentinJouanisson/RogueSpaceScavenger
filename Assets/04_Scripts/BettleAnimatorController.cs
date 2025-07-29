using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ennemy
{

    public class BettleAnimatorController : MonoBehaviour
    {
        public float detectionRange = 10f;
        public float speed = 2f;
        public string playerTag = "Player";
        public float rotationSpeed = 1f;

        private Animator animator;
        private Transform player;
        private bool hasEnteredAttack = false;

       

        void Start()
        {
            animator = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag(playerTag)?.transform;            

            if (animator != null)
            {
                animator.Play("Idle");
            }
            
        }
        private void Attack()
        {
            
            Vector3 direction = (player.position - transform.position).normalized;            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            transform.Translate(direction * speed * Time.deltaTime,Space.World);
        }

        private void Update()
        {
            
            if (player == null) return;
            float distance = Vector3.Distance(transform.position, player.position);

            if ( distance <= detectionRange)
            {
                if (!hasEnteredAttack)
                {
                    animator.SetTrigger("EnterAttack");
                    animator.SetBool("IsAttacking", true);
                    hasEnteredAttack = true;
                }
                Attack();                
            }
            else
            {
                    animator.SetBool("IsAttacking", false);
                    hasEnteredAttack = false;
                    
             
            }
           
        }
    }
}

