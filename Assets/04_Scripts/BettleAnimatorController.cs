using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ennemy
{

    public class BettleAnimatorController : MonoBehaviour
    {
        [Header("Ranges")]
        public float detectionRange = 10f;
        public float attackRange = 2f;

        [Header("Movement")]
        public float speed = 2f;
        public float rotationSpeed = 1f;

        [Header("Settings")]
        public string playerTag = "Player";


        private Animator animator;
        private Transform player;
        private bool hasEnteredAttack = false;
        private PlayerHealth playerHealth;

       

        void Start()
        {
            GameObject playerHealthInstance = GameObject.FindGameObjectWithTag("Player");
            if(playerHealthInstance != null)
            {
                playerHealth = playerHealthInstance.GetComponent<PlayerHealth>();
            }
            if(playerHealthInstance == null)
            {
                Debug.LogError("playerHealthNotFound");
            }
            
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

            bool IsPlayerAlive = playerHealth.currentHealth > 0f;
            animator.SetBool("IsPlayerAlive", true);

            float distance = Vector3.Distance(transform.position, player.position);

            if ( distance <= detectionRange && IsPlayerAlive)
            {
                if (!hasEnteredAttack)
                {
                    animator.SetTrigger("EnterAttack");
                    animator.SetBool("IsAttacking", true);
                    hasEnteredAttack = true;
                }
                Attack();
                
                if(distance < attackRange)
                {
                    if (!hasEnteredAttack)
                    {
                        animator.SetTrigger("EnterAttack");
                        hasEnteredAttack= true;
                        
                    }
                    animator.SetBool("IsAttacking", true );
                    animator.SetBool("PlayerInRange", true ) ;
                    

                }
                else
                {
                    animator.SetBool("IsAttacking", false );
                    animator.SetBool("PlayerInRange", false ) ;
                    hasEnteredAttack = false;
                }
            }
            else
            {
                    animator.SetBool("IsAttacking",false);
                    animator.SetBool("PlayerInRange", false);
                    hasEnteredAttack = false;
                    
             
            }
           
        }
    }
}

