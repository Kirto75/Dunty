using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    Stats stats;
    public HealthBar healthBar;
    public Animator animator;
    void Awake()
    {
        stats = GetComponent<Stats>();
    }
    void Start()
    {
        healthBar.RaiseHealthBar(stats.maxHealth);
        stats.currentHealth = stats.maxHealth;
    }
    void Update()
    {
        
        
    }
    public void DamageEntity(int amount)
    {
         
            stats.currentHealth -= amount;
            healthBar.LowerHealthBar(amount);
        
    }
    public void HealEntity(int amount)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            stats.currentHealth += 20;
            healthBar.RaiseHealthBar(20);
            if (stats.currentHealth > stats.maxHealth)
                stats.currentHealth = stats.maxHealth;  //make sure health doesnt go above max  
        }
    }
}
