using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlaySoundExit : StateMachineBehaviour
{
    [HideInInspector] public SoundType sound;
    [HideInInspector, Range(0, 1)] public float volume;
    [HideInInspector] public bool random;
    [HideInInspector] public int index;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        SoundManager.PlaySound(sound, volume, index);
    }
}

[CustomEditor(typeof(PlaySoundExit))]
public class PlaySoundExit_Editor : Editor
{
    public override void OnInspectorGUI(){
        // Check if the game is playing
        GUI.enabled = !EditorApplication.isPlaying;

        // // Draw the default inspector
        // base.OnInspectorGUI();

        var script = (PlaySoundEnter)target;

        script.sound = (SoundType) EditorGUILayout.EnumPopup("Sound", script.sound);
        
        script.volume = EditorGUILayout.Slider("Volume", script.volume, 0f, 1f);

        script.random = EditorGUILayout.Toggle("is Random", script.random);

        if (script.random){
            script.index = -1;
            return;
        }

        script.index = EditorGUILayout.IntField("Index: ", script.index);

        // Re-enable GUI for other editors if needed
        GUI.enabled = true;
    }
}
