using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Stats stats;

    // [SerializeField] private float jumpHeight = 2f;
    // [SerializeField] private float jumpuDuration = 0.5f; //Jump speed
    // private bool isJumping = false;
    // private float jumpStartTime;
    // private Vector3 startPos;
    public Rigidbody rb;

    private Vector3 input;
    public Animator animator;
    private bool wasRunning = false;
    public GameObject OnGround;
    public bool onGround;
    // help in hell
    public bool invertedControls = false;
    public bool invertx, inverty;

    void Awake()
    {
        stats = GetComponent<Stats>();
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            animator.Play("DieRecover");
    }
    void Update()
    {
        if (!animator.GetBool("isDead"))
        {
            GatherInput();
            Look();
            Move();
            HandleAnimations();
            //StartJump();  
            jump();
            CheckGroundStatus();
            // checkAttack();
            stats.Die();
        }
    }



    void GatherInput()
    {
        float horizontal, vertical;
        if (invertx){
            horizontal = -Input.GetAxisRaw("Horizontal"); // Invert the horizontal input 
        }
        else{
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        if (inverty){
            vertical = -Input.GetAxisRaw("Vertical"); // Invert the vertical input
        }
        else{
            vertical = Input.GetAxisRaw("Vertical");
        }

        if (invertedControls){
            input = new Vector3(vertical, 0, horizontal);
        }
        else{
            input = new Vector3(horizontal, 0, vertical);
        }
        
    }
    void Move()
    {
        if (!animator.GetBool("isAttacking"))
        {
            transform.position = transform.position + (input * stats.speed * Time.deltaTime);
        }
    }
    void Look()
    {

        if (input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(input);
            var relative = (transform.position + input) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, stats.turnSpeed * Time.deltaTime);

        }

    }
    void HandleAnimations()
    {
        bool isMoving = input.x != 0 || input.z != 0;
        if (isMoving && onGround)
        {
            if (!wasRunning)
            {
                animator.SetBool("isRunning", true);
                wasRunning = true;
                // Debug.Log("animation is working");
            }
        }
        else
        {
            if (wasRunning)
            {
                animator.SetBool("isRunning", false);
                wasRunning = false;
                // Debug.Log("animation is not working");
            }
        }
    }
    public void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround){
            animator.SetTrigger("Space");
            rb.AddForce(0, stats.jumpDistance, 0, ForceMode.Impulse);
            SoundManager.PlaySound(SoundManager.getSoundType("Jumping"), 0.3f, 0);
        }
    }

    // public void StartJump() {
    //      if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
    //     isJumping = true;
    //     jumpStartTime = Time.time;
    //     startPos = transform.position;
    //      }
    //      if(isJumping){
    //         PerformJump();
    //      }
    // }
    // public void PerformJump() {
    //     float elapsedTime = Time.time - jumpStartTime;
    //     if (elapsedTime < jumpuDuration) {
    //         //calculate the fraction of the jump duration that has passed
    //         float jumpFraction = elapsedTime / jumpuDuration;

    //         //calculate the height of the jump based on a parabole
    //         float jumpHeightAtThisPoint = Mathf.Sin(jumpFraction * Mathf.PI) * jumpHeight;

    //         //update the player's  position
    //         transform.position = new Vector3(startPos.x, startPos.y + jumpHeightAtThisPoint, startPos.z);
    //     }
    //     else {
    //         //end the jump when the jump duration has elapsed
    //         isJumping = false;
    //         transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
    //     }
    // }

    // public void checkAttack(){
    //     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //     if (stateInfo != "Attack")
    // }
    void CheckGroundStatus()
    {
        //Assume the player in not grounded intially
        onGround = false;

        //use raycast to check if the player is grounded on the terrain
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                onGround = true;
            }
            else
            {
                onGround = false;
            }
        }
    }
}

