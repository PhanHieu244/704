using UnityEngine;
using System.Collections;

public class RotateY : MonoBehaviour {
    [SerializeField]
    private float speed = 100;

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
