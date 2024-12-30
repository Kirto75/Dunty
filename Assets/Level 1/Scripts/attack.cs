using UnityEngine;

public class attack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
    }

    void HandleAttack(){
        if(Input.GetMouseButton(0)){
            animator.SetTrigger("attack");
        }
    }
}
