using UnityEngine;

public class HeadHitBox : MonoBehaviour
{
    private HumanoidEnnemy owner;
    private Collider selfCollider;

    private void Awake()
    {
        owner = GetComponentInParent<HumanoidEnnemy>();
        selfCollider = GetComponent<Collider>();
        if (selfCollider == null)
        {
            Debug.LogError("NoSelfColliderFound");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            owner.EnnemyDie();
            Destroy(gameObject, 1f);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
