using UnityEngine;

public class PauseMinu : MonoBehaviour
{
    public CanvasGroup OptionPanel;

    void Start(){

    }

    void Update(){
        if (Input.GetKey(KeyCode.Escape)){
            Option();
        }
    }
    public void Back(){
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }

    public void Option(){
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

    public void QuitGame(){
        Application.Quit();
    }
}
