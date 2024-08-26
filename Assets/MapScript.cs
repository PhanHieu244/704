using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MapScript : MonoBehaviour {
	public GameObject lblMapName,btNext,btPrevious,gridView,toastMessage;
	public Transform[] mapObject;
	public AudioClip clickSound;
	public Color enableColor,disableColor;
	int currentSelectedMap;
	// Use this for initialization
	void Start () {
		currentSelectedMap=SavedDataManager.getCurrentSeledtedMap();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	//	Application.targetFrameRate=60;
		updateLayout();

		Invoke("defaultCenterOn",0.3f);
	}
	public void playClickSound(){
		GetComponents<AudioSource>()[1].PlayOneShot(clickSound);
	}
	void updateLayout(){
		btPrevious.SetActive(true);
		btNext.SetActive(true);
		for(int i=0;i<mapObject.Length;i++){
			if (SavedDataManager.isMapUnlock(i)==1){
				mapObject[i].gameObject.GetComponent<UISprite>().color=  enableColor;

			}
			else{ 
				mapObject[i].gameObject.GetComponent<UISprite>().color=  disableColor;
			}
		}
		switch (currentSelectedMap){
		case 0:
			btPrevious.SetActive(false);
			btNext.SetActive(true);
			lblMapName.GetComponent<UILabel>().text="Black Desert";
			break;
		case 1:
			lblMapName.GetComponent<UILabel>().text="Snow Mountain";
			break;
		case 2:
			btPrevious.SetActive(true);
			btNext.SetActive(false);
			lblMapName.GetComponent<UILabel>().text="Coming Soon...";
			break;

		}

	}
	void OnEnable(){
		if (gridView!=null)
		gridView.GetComponent<UICenterOnChild>().onCenter+=startCenterOnChild;
	}
	void OnDisable(){
		if (gridView!=null)
		gridView.GetComponent<UICenterOnChild>().onCenter-=startCenterOnChild;
	}
	void startCenterOnChild(GameObject obj){
		
		currentSelectedMap= int.Parse(obj.name);
		updateLayout();
	}
	void defaultCenterOn(){
		
		gridView.GetComponent<UICenterOnChild>().CenterOn(mapObject[SavedDataManager.getCurrentSeledtedMap()]);
	}
	void centerOnCurrentMap(){
		
		gridView.GetComponent<UICenterOnChild>().CenterOn(mapObject[currentSelectedMap]);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void next(){
		playClickSound();
		if (currentSelectedMap<2){
			currentSelectedMap++;
			centerOnCurrentMap();
			updateLayout();
		}

	}
	public void previous(){
		playClickSound();
		if (currentSelectedMap>0){
			currentSelectedMap--;
			centerOnCurrentMap();
			updateLayout();
		}
	}
	public void back(){
		playClickSound();
		SceneManager.LoadScene("UpgradeScene");
	}
	public void toPlayScreen(GameObject obj){
		playClickSound();
		currentSelectedMap= int.Parse(obj.name);
		if (currentSelectedMap<2){
			if (SavedDataManager.isMapUnlock(currentSelectedMap)==1){
				SavedDataManager.setSelectedMap(currentSelectedMap);
				SceneManager.LoadScene("GamePlay");
			} else {
				toastMessage.GetComponent<ToastMessage>().show("Reach 10.000m in previous map to Unlock");
			}

		} else {
			toastMessage.GetComponent<ToastMessage>().show("Coming soon...");
		}
	}
}
