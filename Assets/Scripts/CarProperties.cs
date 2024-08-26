using UnityEngine;
using System.Collections;

public class CarProperties : MonoBehaviour
{
	
	public int mSpeed,mHandle,mAcceleration,mBrake;
	public CarProperties (int speed,int handle,int accele,int brake){
		mSpeed=speed;
		mHandle=handle;
		mAcceleration=accele;
		mBrake=brake;
	}

}

