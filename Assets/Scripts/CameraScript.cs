using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    GameObject playerObject;
    Vector3 offset;

    bool startFollow;
    // Use this for initialization
    void Awake()
    {
        startFollow = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (startFollow)
        {
            transform.position = new Vector3(0, playerObject.transform.position.y + offset.y, playerObject.transform.position.z + offset.z);
        }
    }
    public void assignPlayerToCamera(GameObject player)
    {
        startFollow = true;
        playerObject = player;
        offset = transform.position - playerObject.transform.position;
        offset.x = 0;

    }
}
