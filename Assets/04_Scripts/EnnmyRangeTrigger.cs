using UnityEngine;

[RequireComponent (typeof(Collider))]
public class EnnmyRangeTrigger : MonoBehaviour
{
    private Animator animator;
    private string inRangeParam = "IsAttacking";
    private string playerTag = "Player";
    private int overlapCount = 0;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        if (!animator) animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            overlapCount++;
            if(animator) animator.SetBool(inRangeParam, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            overlapCount = Mathf.Max(0, overlapCount - 1);
            if (overlapCount == 0 && animator) animator.SetBool(inRangeParam, false);
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
