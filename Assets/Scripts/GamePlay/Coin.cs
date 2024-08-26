using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER)
        {
            Deactive();
            other.GetComponent<CarController>().PlayCoinEffect();
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
