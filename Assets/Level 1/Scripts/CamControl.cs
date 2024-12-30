using UnityEngine;

public class CamControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public float lerpSpeed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position;
        // InvokeRepeating("moveCam", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = player.transform.position + offset;
        Vector3 tragetPosition = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, tragetPosition,Time.deltaTime *lerpSpeed);
    }

    // void moveCam(){
    //     transform.position = player.transform.position + offset;
    // }

}
