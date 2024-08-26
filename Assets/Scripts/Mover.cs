using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	Vector3 defaultPosision;
	float totalTravel;
	public float speed;
	// Use this for initialization
	void Start () {
		defaultPosision=transform.position;
		GetComponent<Rigidbody>().velocity= transform.forward*speed;
	}

	// Update is called once per frame
	void Update () {
		totalTravel=Mathf.Abs(transform.position.z- defaultPosision.z);
	//	Debug.Log(gameObject.name+ " "+ totalTravel);
		if (totalTravel>=250) {
			transform.position=defaultPosision;

		}
	}
}
