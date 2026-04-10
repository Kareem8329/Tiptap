using UnityEngine;

public class Shrink : MonoBehaviour
{
    public float shrinkRate = 0.5f;
    private UIManager manager;
    public Vector3 size;
    private Animator animator;
    private float animationDuration = 0.15f;  // Adjust to your animation length
    private float timeSinceSpawn = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        manager = FindFirstObjectByType<UIManager>();
    }
    
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        
        // Disable animator after animation finishes to allow shrinking
        if (timeSinceSpawn > animationDuration && animator != null && animator.enabled)
        {
            animator.enabled = false;
        }
        
        size = transform.localScale;
        if (timeSinceSpawn > animationDuration && transform.localScale.x > 0.1f)
        {
            Debug.Log("Shrinking: " + transform.localScale);
            transform.localScale -= new Vector3(shrinkRate, shrinkRate, 0) * Time.deltaTime;
        }
        else if (timeSinceSpawn > animationDuration && transform.localScale.x <= 0.1f)
        {
            if (manager != null)
            {
                manager.AddScore(-10);
            }
            Destroy(gameObject);
        }
    }
}