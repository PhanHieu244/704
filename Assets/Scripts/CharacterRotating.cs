using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CharacterRotating : MonoBehaviour
{
	Camera cam;
	Vector3 defaultCharacterAngle;
	public float slowSpeedRotation;
	public float speedRotation;
	private const string AVATAR_TAG = "Car_Model_Tag";
	private bool isRotating = false;
	private RaycastHit hit;
	bool isMoving;
	Vector3 targetPos;

	void Start()
	{
		targetPos=transform.position;
		isMoving=false;

		defaultCharacterAngle=transform.localEulerAngles;
		cam=Camera.main;
	}
	public void resetRotateAngle(){
		transform.eulerAngles=defaultCharacterAngle;
	}
	public void moveToPosition(Vector3 pos ){
		isMoving=true;
		targetPos=new Vector3(pos.x,transform.position.y,pos.z);

	}
	// Update is called once per frame
	void Update()
	{
		
		MouseButtonDown();
		MouseButotnUp();
		if (isMoving){
			transform.position= Vector3.Lerp(transform.position,targetPos,Time.deltaTime*10);
			if (transform.position==targetPos){
				isMoving=false;
			
			}
		} else 
		if (Input.GetMouseButton(0) && isRotating)
		{
			float x = -Input.GetAxis("Mouse X");
			#if UNITY_EDITOR
			//float x = -Input.GetAxis("Mouse X");
			#elif UNITY_ANDROID
			float x = -Input.touches[0].deltaPosition.x;
			#endif
			transform.rotation *= Quaternion.AngleAxis(x * speedRotation, Vector3.up);

		}
		else
		{
		//	if(transform.transform.localEulerAngles.y != defaultCharacterAngle.y)
		//	{
			//	Debug.Log("transform.rotation.y:  "+transform.localEulerAngles.y);
				SlowRotation();
		//	}
		}
	}

	private void MouseButtonDown()
	{
		if(Input.GetMouseButtonDown(0))
		{
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			#if UNITY_EDITOR
		//	Debug.Log(""+Physics.Raycast(ray, out hit));
		//	Debug.Log(""+hit.collider);
			#elif UNITY_ANDROID
			Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
			#endif
			if(Physics.Raycast(ray, out hit))
			{
			//	Debug.Log(""+hit.collider.name);
				if(hit.collider.tag == AVATAR_TAG && transform == hit.collider.transform)
				{
					isRotating = true;
				}
			}
		}
	}

	private void MouseButotnUp()
	{
		if(Input.GetMouseButtonUp(0))
		{
			isRotating = false;
			hit = new RaycastHit();
		}
	}

	private void SlowRotation()
	{
		
	//	transform.localEulerAngles= transform.localEulerAngles,defaultCharacterAngle,slowSpeedRotation* Time.deltaTime);
	
			transform.Rotate(Vector3.up *Time.deltaTime*slowSpeedRotation);
	}
}