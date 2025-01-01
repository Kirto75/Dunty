using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] public SoundType sound;
    [SerializeField, Range(0, 1)] public float volume;
    [SerializeField] public bool random;
    [SerializeField] public int index;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        SoundManager.PlaySound(sound, volume, index);
    }
}

[CustomEditor(typeof(PlaySoundEnter))]
public class PlaySoundEnter_Editor : Editor
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
