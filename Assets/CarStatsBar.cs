using UnityEngine;
using System.Collections;

public class CarStatsBar : MonoBehaviour {
	public enum STAT {
		SPEED,HANDLE,ACCELERATION,BRAKE
	}
	public GameObject orangeLeftDot,orangeBar;
	public GameObject blueBar;
	public STAT statType;
	float targetScale;
	int currentStats;
	void Start () {
		targetScale=0.1f;

	
	}

	// Update is called once per frame
	void Update () {

		orangeBar.transform.localScale= Vector3.Lerp(orangeBar.transform.localScale,new Vector3(targetScale,1f,1f),0.2f);
		if (targetScale<1)
			blueBar.transform.localScale= Vector3.Lerp(blueBar.transform.localScale,new Vector3(targetScale+0.1f,1f,1f),0.1f);


	}
	public void setCarStats(int statCount){
		currentStats=statCount;
		targetScale= statCount*0.1f;
	}

	public void increaseStat(){
		currentStats++;
		targetScale= 2*0.1f;
	}

}
