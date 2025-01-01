using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleDamage();
        HandleDeath();
    }

    void HandleDamage(){
        if (Input.GetKeyDown(KeyCode.I)){
            animator.SetTrigger("i");
        }
    }

    void HandleDeath(){
        if (Input.GetKeyDown(KeyCode.K)){
            animator.SetTrigger("k");
        }
    }
}
