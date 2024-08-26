using UnityEngine;
using System.Collections;

public class Achievement : MonoBehaviour {
	//0= lock, 1= unlock, 2= claimmed;
	int acvState;
	int acvNo;

	// Use this for initialization
	void Start () {
		acvNo= int.Parse(gameObject.name);
		checkState();

	}

	void checkState(){
		acvState= SavedDataManager.getAchievementState(acvNo);
		if (acvState==0){
	//		transform.FindChild("btReward").transform.FindChild("text").GetComponent<UILabel>().fontSize=70;
		//	transform.FindChild("btReward").transform.FindChild("text").GetComponent<UILabel>().height=107;
			transform.Find("btReward").GetComponent<UIButton>().disabledColor= Color.gray;
			transform.Find("btReward").transform.Find("text").GetComponent<UILabel>().text=MenuScript.achievementRewardArray[acvNo-1]+"$";
			transform.Find("btReward").GetComponent<UIButton>().isEnabled=false;
		} 
		if (acvState==1){
		//	transform.FindChild("btReward").transform.FindChild("text").GetComponent<UILabel>().fontSize=70;
		//	transform.FindChild("btReward").transform.FindChild("text").GetComponent<UILabel>().height=107;
			transform.Find("btReward").transform.Find("text").GetComponent<UILabel>().text="Claim";
			transform.Find("btReward").GetComponent<UIButton>().isEnabled=true;
		} 
		if (acvState==2){
		//	transform.FindChild("btReward").transform.FindChild("text").GetComponent<UILabel>().fontSize=70;
		//	transform.FindChild("btReward").transform.FindChild("text").GetComponent<UILabel>().height=107;
			transform.Find("btReward").GetComponent<UIButton>().disabledColor= Color.gray;
			transform.Find("btReward").transform.Find("text").GetComponent<UILabel>().text="Claimed";
			transform.Find("btReward").GetComponent<UIButton>().isEnabled=false;
		} 
	}
	public void claim(){
		SavedDataManager.claimAchievement(acvNo);
		checkState();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
