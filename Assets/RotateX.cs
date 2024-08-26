using UnityEngine;
using System.Collections;

public class RotateX : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.right*Time.deltaTime*speed);
	}
}
