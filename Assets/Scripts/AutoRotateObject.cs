using UnityEngine;
using System.Collections;

public class AutoRotateObject : MonoBehaviour {
	public float rotateSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 30, 0) *rotateSpeed* Time.deltaTime);
	}
}
