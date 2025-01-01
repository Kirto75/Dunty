using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DemonEnemy : MonoBehaviour
{
    public GameObject player;
    public Rigidbody Ball;

    Animator animator;
    Rigidbody rb;
    Vector3 Direction;
    public Transform handPalm;
    public float detectionRange = 10f;
    bool isAttacking = false;
    bool isInRange = false;
    public int health = 100;
    bool isDead = false;
    bool invulnerableFrames = false;
    float invulnerableTime = 0.5f; //Entity can't be attack within
    public DemonMesh demonMesh;
    Color originalColor;
    public Material material;
    private int deathlimit = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("isDead"))
        {
            Rotate();
            //calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange && !isAttacking)
            {

                Attack();
            }

            // Stop everything
            // canAttack = false;
            // isInRange = false;
            // Call the Death() function
        }
        else{
             if (deathlimit == 1) {SoundManager.PlaySound(SoundManager.getSoundType("HitSounds"), 0.3f, 10); deathlimit--;}
            Dead();
        }

    }
    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            animator.Play("Shoot");

            animator.SetBool("isAttacking", true);
            isAttacking = true;
            //Debug.Log("isattacking is true");

            StartCoroutine("AttackWait");
        }
    }

    IEnumerator AttackWait()
    {
        //to fix the proplem that it reads the default
        yield return new WaitForSeconds(0.1f);

        //wait for the animation to end
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log("the length of the animation " + stateInfo.length);
        yield return new WaitForSeconds(stateInfo.length);
        // Debug.Log("the length of the animation " + stateInfo.length);


        //make the enemy able to attack aggain
        animator.SetBool("isAttacking", false);
        isAttacking = false;
        // Debug.Log("isattacking is false");
    }

    // private void playerRange()
    // {
    //     float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    //     if (distanceToPlayer <= detectionRange)
    //     {

    //         isInRange = true;
    //         Attack();
    //     }
    //     else
    //     {
    //         isInRange = false;
    //     }
    // }

    private void Dead()
    {
        StartCoroutine(DeathWait());
    }
    IEnumerator DeathWait()
    {
        animator.SetFloat("isDead", 1);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    private void ShootBall()
    {
        Rigidbody ball = Instantiate(Ball, handPalm.position, handPalm.rotation) as Rigidbody;
        ball.AddForce(ball.transform.up * 1000);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !invulnerableFrames)
        {

            health -= 25;
            SoundManager.PlaySound(SoundManager.getSoundType("HitSounds"), 0.3f, 5 + UnityEngine.Random.Range(0, 1));


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
        demonMesh.meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(invulnerableTime);

        invulnerableFrames = false;
        //return the original color of the mesh
        demonMesh.meshRenderer.material.color = material.color;

        //Debug.Log("i-frames ends");
    }
}
