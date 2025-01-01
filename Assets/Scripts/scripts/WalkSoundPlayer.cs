using UnityEngine;

public class WalkSoundPlayer : MonoBehaviour
{
    public void stepSound(){
        // play sounds 0, 1, 2 randomly
        SoundManager.PlaySound(SoundManager.getSoundType("Footsteps"), 0.2f, (int)Random.Range(0,2));
    }

}
