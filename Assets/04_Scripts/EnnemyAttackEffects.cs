using UnityEngine;

public class EnnemyAttackEffects : MonoBehaviour
{
    public Animator animator;
    public string attackBoolName = "PlayerInRange";
    public GameObject attackSphereVFX;

    private int attackBoolHash;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackBoolHash = Animator.StringToHash(attackBoolName);

        if(attackSphereVFX != null)
        {
            attackSphereVFX.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null || attackSphereVFX == null)
            return;
        bool isAttacking = animator.GetBool(attackBoolHash);
        attackSphereVFX.SetActive (isAttacking);
       
    }
}
