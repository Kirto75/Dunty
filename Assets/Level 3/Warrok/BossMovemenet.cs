using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BossMovemenet : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    GameObject player;
     GameObject bossHand;
    // public String[] attackAnimations;
    public float speed;
    public float maxHealth = 100;
    public float detectionRange = 20;
    public float attackRange = 7;
    bool inAttackRange =false;
    // public float momentumDistance = 1f;
    // public float momentumDuration = 0.2f;
    float distanceToPlayer;
    bool isAttacking = false;
    bool isDead =false;
    bool invulnerableFrames = false;
    public BossMesh bossMesh;
    public Material material;
    private int deathlimit = 1;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // playerPosition =player.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        bossHand =GameObject.FindWithTag("BossHand");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {

        //Check if the player is in detection range
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToPlayer);
        if (distanceToPlayer <= detectionRange && !animator.GetBool("IsAttacking"))
        {
            Move();

            if (distanceToPlayer <= attackRange && !animator.GetBool("IsAttacking"))
            {
                inAttackRange = true;
                StartCoroutine("Attack");
            }
        }
        Rotate();
        // Rage();
        }
        else
        {
            if (deathlimit == 1) {SoundManager.PlaySound(SoundManager.getSoundType("HitSounds"), 0.7f, 10); deathlimit--;};
            StartCoroutine("Die");
            if (deathlimit == 0){SoundManager.PlaySound(SoundManager.getSoundType("Victory"), 1f, 0); deathlimit--;};
        }


    }




    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }




    public void Move()
    {
        if (!isAttacking&&!inAttackRange)
        {
            animator.SetBool("IsFollownig", true);

            // Calculate the direction towards the player
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize(); // Normlize the direction vector.

            //Move towards the player
            transform.position += direction * speed * Time.deltaTime;
        }
    }



    public IEnumerator Die()
    {
        animator.Play("Dying", 0);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        
    }


    // public void Rage()
    // {
    //     if (maxHealth <= 50 && !animator.GetBool("IsRaging"))
    //     {
    //         animator.Play("Rage");
    //         animator.SetBool("IsRaging", true);
    //         speed = speed * 1.5f;
    //         Debug.Log("speed = " + speed);
    //     }
    // }

    public IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsFollownig", false);
        animator.SetBool("IsAttacking", true);
        animator.SetBool("Finshed",false);

        // animator.Play("Swiping");

        //wait for the end of the current frame to let the animator update
        yield return new WaitForSeconds(0.9f);

        //get the updated state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(attackAnimations[randomAttack]+" duration is" + stateInfo.length);

        yield return new WaitForSeconds(stateInfo.length + 0.3f);
        Debug.Log (stateInfo.length);

        
        if(distanceToPlayer >= attackRange  )
        {
            inAttackRange = false;
        }
       
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
        animator.SetBool("Finshed",true);
        Debug.Log("Attacks ends");
        
    }
    public void DisableBossHandCollider()
    {
        bossHand.GetComponent<Collider>().enabled = false;

    }
    public void EnableBossHandCollider()
    {
        bossHand.GetComponent<Collider>().enabled = true;

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !invulnerableFrames)
        {

            maxHealth -= 25;
            SoundManager.PlaySound(SoundManager.getSoundType("HitSounds"), 0.3f, 8 + UnityEngine.Random.Range(0, 2));

            if (maxHealth <= 0)
            {
                //use play to start the animation immediately
                animator.Play("Dying");
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
        bossMesh.meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        bossMesh.meshRenderer.material.color = material.color;
        invulnerableFrames = false;
        //return the original color of the mesh
        //Debug.Log("i-frames ends");
    }

}
