using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class zeroBar : MonoBehaviour
{
    public Slider slider;
    public RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnZero();
    }

    void OnZero(){
        if (slider.value == 0){
            Image g = rectTransform.GetComponent<Image>();
            g.enabled = false;
        }
    }
}
