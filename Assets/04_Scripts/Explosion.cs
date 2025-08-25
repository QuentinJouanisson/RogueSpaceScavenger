using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Tooltip("if defined only defined object will explode, otherwise this gameobject will")]
    public GameObject targetRoot;

    public float explosionForce = 200f;

    public float explosionRadius = 5f;

    public float destroyDelay = 0f;

    private Animator animator;


    public void explosion()
    {
        if(targetRoot == null)
        {
            targetRoot = gameObject;
        }
        if (animator != null)
        {
            animator.enabled = false;
        }       

        var parts = targetRoot.GetComponentsInChildren<MeshRenderer>();

        foreach(var part in parts)
        {
            if (part.GetComponent<Rigidbody>() != null) continue;
            var rb = part.gameObject.AddComponent<Rigidbody>();            
            var col = part.gameObject.AddComponent<SphereCollider>();
            col.isTrigger = false;

            rb.AddExplosionForce(explosionForce, targetRoot.transform.position, explosionRadius);

            if(destroyDelay > 0f)
                Destroy(part.gameObject, destroyDelay);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
