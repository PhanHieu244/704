using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
    [SerializeField]
    private float speed = 100;

    public bool available
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
        //if (transform.position.z < Camera.main.transform.position.z - 10) Deactive();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER)
        {
            Deactive();
            other.GetComponent<CarController>().ShieldOn(true);
        }
    }

    public void Put(float x, float z)
    {
        Vector3 pos = transform.position;
        pos.x = x;
        pos.z = z;
        transform.position = pos;
        gameObject.SetActive(true);
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }
}
