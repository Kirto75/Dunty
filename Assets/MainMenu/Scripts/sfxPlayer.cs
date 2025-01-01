using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxPlayer : MonoBehaviour
{
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip walk, lightAttack, heavyAttack;
    [SerializeField] bool free;
    int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = 1;
        free = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator walking(){
        free = false;
        src.Stop();
        src.clip = walk;
        src.Play();
        yield return new WaitForSeconds(src.clip.length);
        free = true;
    }

    public IEnumerator attacking(bool doubleAttack){
        free = false;
        if (doubleAttack && count == 0){
            src.clip = heavyAttack;
            count = 1;
        }
        else{
            src.clip = lightAttack;
            count = 0;
        }
        src.Stop();
        src.Play();
        yield return new WaitForSeconds(60f / 100f);
        free = true;
    }

    public void killWalking(){
        if (src.clip == walk)
            src.Stop();
    }

    public bool isFree(){
        return free;
    }

}
