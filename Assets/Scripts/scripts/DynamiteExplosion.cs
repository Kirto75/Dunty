using UnityEngine;

public class DynamiteExplosion : MonoBehaviour
{
    public Transform playerTransform;
    public bool isHeld = false;
    void Update()
    {
        if (isHeld)
        {
            transform.localPosition = new Vector3(0,0.5f,1);
        }


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CaveEntrance"))
        {
            SoundManager.PlaySound(SoundManager.getSoundType("Exploding"), 0.7f, 0);
            DestroyAllRocks(); //Destroy the rocks
            Destroy(gameObject); //destroy the dynamite

        }
        if(collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            PickUp();
        }
    }

    void DestroyAllRocks()
    {
        GameObject[] allrocks = GameObject.FindGameObjectsWithTag("CaveEntrance");
        foreach(GameObject Rock in allrocks)
        {
            Destroy(Rock);
        }
    }
    void PickUp()
    {
        isHeld = true;
        transform.SetParent(playerTransform);  //Attach dynamite to the player
        transform.localPosition = new Vector3(0,1,0); //Adjust positon reletive to the player
    }
}
