using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CreepEnemy : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    Rigidbody rb;
    Vector3 Direction;
    public float speed = 5;
    public float detectionRange = 10f;  //Can detect player from
    public float attackRange = 2f;  //Can attack player from
    float rangeCheckInterval = 0.4f;  //time between every check 

    float rangeCheckTimer = 0f;
    bool isWalking = false;
    public bool isDead = false;
    bool isInRange = false;
    public int health = 100;
    bool isAttacking = false; //track if currently attacking 
    bool invulnerableFrames = false;
    float invulnerableTime = 0.5f; //Entity can't be attack within
    Color originalColor; //var to store the original color of the mesh
    float flashTime = 0.2f;
    public CreepMesh creepMesh;
    public GameObject creepHandR;
    public GameObject creepHandL;
    public Material material;
    private int deathlimit = 1;



    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if  isDead = false means the enemy health > 0 
        if (!isDead)
        {


            //Dead();

            Move();

            Rotate();

            //calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // check if the player is within attack range
            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                //Debug.Log("in Attack Range");
                StartCoroutine("Attack");
            }

            rangeCheckTimer -= Time.deltaTime;
            if (rangeCheckTimer <= 0)
            {
                //Debug.Log("Time to check range");
                playerRange(); //Check player's range
                rangeCheckTimer = rangeCheckInterval; // Reset timer
            }
        }
        else
        {
            
            if (deathlimit == 1) {SoundManager.PlaySound(SoundManager.getSoundType("HitSounds"), 0.3f, 10); deathlimit--;}
            StartCoroutine("DeathWait");

        }
    }

    private void Move()
    {
        if (!isAttacking && isInRange)
        {
            Direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + Direction * speed * Time.fixedDeltaTime);
            isWalking = true;
            animator.SetBool("isWalking", true);
        }
        else
        {
            isWalking = false;
            animator.SetBool("isWalking", false);
        }
    }
    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private IEnumerator Attack()
    {
        // Debug.Log("IEnumerator is called");
        //Trigger the attack animation
        animator.SetBool("isAttacking", true);
        isAttacking = true;

        //stop the movement
        isWalking = false;



        // wait for the animation to finish
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(stateInfo.length - 3.22f);
        //Debug.Log("wait ends");

        //check if the player is still in range before attacking again
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true; //keep the movement locked
            animator.SetBool("isAttacking", true);

            //wait until the monster finish the attack 
            yield return new WaitForSeconds(stateInfo.length - 3.22f);
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
        else
        {
            isAttacking = false; //resume movement
            animator.SetBool("isAttacking", false);
        }


    }
    private void playerRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //make sure the player is in detection range but not in attack range
        if (distanceToPlayer <= detectionRange && distanceToPlayer >= attackRange)
        {
            isInRange = true;
            //Debug.Log("inRange");
        }
        else
        {
            isInRange = false;
            //Debug.Log("not inRange");
        }
    }

    private void Dead()
    {
        StartCoroutine(DeathWait());
    }
    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    //called if the enemy is hit with Player sword 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !invulnerableFrames)
        {

            health -= 25;
            SoundManager.PlaySound(SoundManager.getSoundType("HitSounds"), 0.3f, 7);

            if (health <= 0)
            {
                //use play to start the animation immediately
                animator.Play("Death");
                isDead = true;
                animator.SetBool("isDead", true);
            }
            // gave the enemy i-frames
            StartCoroutine("InvulnerabilityFrames");
        }
    }
    IEnumerator InvulnerabilityFrames()
    {
        invulnerableFrames = true;
        //change the color of the mesh to red while he cant be damged
        creepMesh.meshRenderer.material.color = Color.red;
        
        yield return new WaitForSeconds(flashTime);

        invulnerableFrames = false;
        //return the original color of the mesh
        creepMesh.meshRenderer.material.color = material.color;
        //Debug.Log("i-frames ends");
    }
    private void EnableCreepHandCollider() {
        creepHandR.GetComponent<Collider>().enabled = true;
        creepHandL.GetComponent<Collider>().enabled = true;
    }
     private void DisableCreepHandCollider() {
        creepHandR.GetComponent<Collider>().enabled = false;
        creepHandL.GetComponent<Collider>().enabled = false;
    }
}
