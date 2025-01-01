using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxHealing : MonoBehaviour
{
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip heal;
    // [SerializeField] bool free;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // free = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void healing(){
        // free = false;
        src.Stop();
        src.clip = heal;
        src.Play();
        // yield return new WaitForSeconds(src.clip.length);
        // free = true;
    }
}
