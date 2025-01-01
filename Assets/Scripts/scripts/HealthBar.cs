using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Animator animator;
    private Stats playerstats;
    public GameObject player;
    public Slider slider;

    void Awake()
    {
        playerstats = player.GetComponent<Stats>(); //take the player stats

    }

    public void SetMaxHealth(int amount)
    {
        slider.maxValue = amount;
        slider.value = amount;


    }
    public void LowerHealthBar(int amount)
    {
        slider.value -= amount;

    }
    public void RaiseHealthBar(int amount)
    {
        slider.value += amount;
        if (playerstats.currentHealth > playerstats.maxHealth)
        {
            playerstats.currentHealth = playerstats.maxHealth;
        }



    }
}
