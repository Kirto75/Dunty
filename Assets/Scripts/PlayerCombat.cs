using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    private Stats stats;
    public Rigidbody rb;
    public GameObject sword;
    public PlayerStats playerStats;
    bool isAttacking = false;
    bool comboReady = false;
    float comboTimer = 0.9f; // Timer for combo
    bool attackInputProcessed = false; // to stop spamming leftclick
    bool invulnerableFrames = false;
    float invulnerableTime = 0.5f; //Player can't be attack within
    float attackForce = 300f; //The monemtum when player attacks

    void Awake()
    {
        stats = GetComponent<Stats>();
    }
    void Update()
    {
        HandleAttackAnimation();
        // HandleDefendAnimation();
        // HandleJumpAnimation();
    }
    private void HandleAttackAnimation()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && !attackInputProcessed)
        {

            rb.AddForce(transform.forward * attackForce); // add some force to give tha attack some moemntum

            animator.SetBool("isAttacking", true);
            isAttacking = true;

            attackInputProcessed = true;
            // Debug.Log("First Attack Triggered");
        }
        else if (isAttacking && comboReady && Input.GetMouseButtonDown(0))
        {

            rb.AddForce(transform.forward * attackForce); // add some force to give tha attack some moemntum

            animator.SetBool("Combo", true);
            comboReady = false;

            attackInputProcessed = true;
            //Debug.Log("Combo Attack Triggered");
        }
        if (Input.GetMouseButtonUp(0))
        {
            attackInputProcessed = false;
        }
    }
    private void HandleDefendAnimation()
    {
        if (Input.GetMouseButton(1))
        {
            animator.SetBool("isDefending", true);
        }
        else
        {
            animator.SetBool("isDefending", false);
        }
    }
    // private void HandleJumpAnimation() {
    //     if (Input.GetKey(KeyCode.Space)) {
    //         animator.SetTrigger("Space");
    //     }
    // }


    public void ResetAttackState()
    {
        //Debug.Log("ResetAttackState called");
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("Combo", false);
    }
    private IEnumerator ComboTimer()
    {
        comboReady = true;
        //Debug.Log("combo window on");
        yield return new WaitForSeconds(comboTimer);
        comboReady = false;
        //Debug.Log("combo window off");
        DisableSwordCollider();
    }
    private void EndCombo()
    {
        isAttacking = false;
        animator.SetBool("Combo", false);
        animator.SetBool("isAttacking", false);
        // Debug.Log("EndCombo called");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CreepHand") && !invulnerableFrames)
        {
            Debug.Log("got hit");
            playerStats.DamageEntity(25);
            StartCoroutine("InvulnerabilityFrames");
        }
        else if(other.CompareTag("DemonBall") && !invulnerableFrames)
        {
            Debug.Log("got hit");
            playerStats.DamageEntity(25);
            StartCoroutine("InvulnerabilityFrames");
        }
    }
    
    IEnumerator InvulnerabilityFrames()
    {
        invulnerableFrames = true;
        //mesh

        yield return new WaitForSeconds(invulnerableTime);
        //mesh
        invulnerableFrames = false;
    }

    private void EnableSwordCollider()
    {
        sword.GetComponent<Collider>().enabled = true;
    }
    private void DisableSwordCollider()
    {
        sword.GetComponent<Collider>().enabled = false;
    }
    public void EnablePlayerMovement()
    {
        //Debug.Log("EnablePlayerMovement called");
        isAttacking = false;

    }
    public void DisablePlayerMovement()
    {
        // Debug.Log("DisablePlayerMovement called");
        isAttacking = true;
    }
}



