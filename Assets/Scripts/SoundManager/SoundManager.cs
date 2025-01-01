using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum SoundType{
    Attacks,
    Footsteps,
    Jumping,
    Landing,
    HitSounds,
    Healing,
    LevelingUp,
    Exploding,
    Defending,
    Victory,
    GameOver
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    private void Awake(){
        instance = this;
    }

    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume, int index = -1){
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (index == -1){
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            instance.audioSource.PlayOneShot(randomClip, volume);
        }
        else{
            AudioClip specificClip = clips[index];
            instance.audioSource.PlayOneShot(specificClip, volume);
        }

        // instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }

#if UNITY_EDITOR
    private void OnEnable(){
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++){
            soundList[i].name = names[i];
        }
    }
#endif

    public static SoundType getSoundType(string name){
        return (SoundType)Enum.Parse(typeof(SoundType), name);
        // string[] names = Enum.GetNames(typeof(SoundType));
        // for (int i = 0; i < names.Length; i++){
        //     if (names[i].Equals(name))
        //         return SoundType[i];
        // }
        // return null;
    }

}

[Serializable]
public struct SoundList{
    public AudioClip[] Sounds { get => sounds;}
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
