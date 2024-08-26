using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform target;
    [SerializeField]
    private Vector3 distance;
    [SerializeField]
    private float speed = 100;

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.z = target.position.z + distance.z;
        pos.y = target.position.y + distance.y;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 100);
    }
}
