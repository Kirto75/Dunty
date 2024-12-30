using UnityEngine;

public class Stats : MonoBehaviour
{
    public Animator animator;
    public int  maxHealth = 0;
    public int currentHealth = 0;
    public int speed = 0;
    public int damage = 0;
    public int turnSpeed = 0;
    public int armor = 0;
    public int jumpDistance = 0;

    public void Die()
    {
        if (currentHealth <= 0)
        {
        animator.SetBool("isDead",true);
        }
    }

}
