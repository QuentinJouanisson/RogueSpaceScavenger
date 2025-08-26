using UnityEngine;
using System.Collections;

using Unity.VisualScripting;


[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(Rigidbody))]

public class HumanoidEnnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float rotationSpeed = 5f;
    public float controlPointZone = 0.5f;


    [Header("Detection Settings")]
    public string playerTag = "Player";
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float chaseSpeedMultiplier = 4f;



    [Header("Attack Settings")]
    public int contactDamage = 10;
    public float attackCooldown = 1f;
    

    [Header("Head collider")]
    public Collider headCollider;

    private Transform player;
    private int currentPatrolIndex = 0;    
    private float lastAttackTime = 0f;
    private Animator animator;
   

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if(playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player Not Found");
        }

        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogWarning("Animator Not Found");
        }
        if(patrolPoints.Length == 0)
        {
            Debug.LogWarning("no patrol points assigned");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        float basePatrolSpeed = patrolSpeed;

        if (player == null)
        {
            Debug.LogWarning("Player not found");            
            animator.SetBool("IsIdlying", true);
            animator.SetTrigger("Idlying");
        }      


        if (distanceToPlayer <= detectionRange)
        {
            
            float runSpeed = patrolSpeed * chaseSpeedMultiplier;
            MoveTowards(player.position, runSpeed);
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsIdlying", false);
            animator.SetBool("IsPatroling", false );            
            animator.SetTrigger("Attacking");
            
        }
        else
        {
            
            animator.SetBool("IsAttacking", false );
            animator.SetBool("IsPatroling", true);
            animator.SetTrigger("Patroling");
            Patrol(basePatrolSpeed);
        }

    }

    void Patrol(float patrolSpeed)
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPoint.position, patrolSpeed);

        float distance = Vector3.Distance(transform.position, targetPoint.position);
        if(distance < controlPointZone)
        {
            currentPatrolIndex++;
            if(currentPatrolIndex >= patrolPoints.Length)
                currentPatrolIndex = 0;
        }        
    }

    void MoveTowards(Vector3 target, float patrolSpeed)
    {
        float speed = patrolSpeed;
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0f;

        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position += direction * speed * Time.deltaTime;

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            float timeSincelastAttack = Time.time - lastAttackTime;
            if(timeSincelastAttack >= attackCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if(playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == headCollider && other.CompareTag(playerTag))
        {
            EnnemyDie();
        }
    }
    void EnnemyDie()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled =false;
        }

        if(animator != null)
        {
            animator.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
    
}
