using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class MenuScript : MonoBehaviour {
	public GameObject dialogAchievement,dialogOption,btSound,btMusic;
	// Use this for initialization
	public AudioClip logoMoveInSound,clickSound;
	public AudioMixer masterMixer;
	public GameObject toastMessage,mapDesert,mapSnow;
	public static int[] achievementRewardArray;
	bool isFirstPlay;
	void Start () {
        //PlayerPrefs.DeleteAll();

        //	QualitySettings.SetQualityLevel(5);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
    //    Application.targetFrameRate=60;
		checkSoundState();
		checkMusicState();
		achievementRewardArray=new int[]{100,250,500,100,250,500,100,250,500,100,250,500,200,200,200,200,200,200,1000,2000,3000};
		isFirstPlay= (PlayerPrefs.GetInt(GameConstant.FIRST_PLAY,0)==0);
	
		if (isFirstPlay){
			isFirstPlay=false;
			SavedDataManager.firstCreateCarData();
			PlayerPrefs.SetInt(GameConstant.FIRST_PLAY,1);
			PlayerPrefs.Save();
		//	Debug.Log("firstCreateCarData");
		}
		if (SavedDataManager.getNumberOfUnlockedCar()>=2){
			if (SavedDataManager.getAchievementState(7)==0)
				SavedDataManager.completeAchievement(7);
		}
		if (SavedDataManager.getNumberOfUnlockedCar()>=3){
			if (SavedDataManager.getAchievementState(8)==0)
				SavedDataManager.completeAchievement(8);
		}
		if (SavedDataManager.getNumberOfUnlockedCar()>=5){
			if (SavedDataManager.getAchievementState(9)==0)
				SavedDataManager.completeAchievement(9);
		}

		if (SavedDataManager.getCurrentMoney()>=10000){
			if (SavedDataManager.getAchievementState(10)==0)
				SavedDataManager.completeAchievement(10);
		}
		if (SavedDataManager.getCurrentMoney()>=100000){
			if (SavedDataManager.getAchievementState(11)==0)
				SavedDataManager.completeAchievement(11);
		}
		if (SavedDataManager.getCurrentMoney()>=1000000){
			if (SavedDataManager.getAchievementState(12)==0)
				SavedDataManager.completeAchievement(12);
		}
		switch (SavedDataManager.getCurrentSeledtedMap()){
		case 0:
			mapDesert.SetActive(true);
			mapSnow.SetActive(false);
			break;
		case 1:
			mapDesert.SetActive(false);
			mapSnow.SetActive(true);
			break;

		}
		Invoke("playLogoSound",0.3f);

	}
	public void openAchievement(){
		playClickSound();
		dialogOption.SetActive(false);
		dialogAchievement.GetComponent<TweenPosition>().ResetToBeginning();
		dialogAchievement.GetComponent<TweenPosition>().PlayForward();
		dialogAchievement.SetActive(true);
	}

	public void closeAchievement(){
		playClickSound();
		dialogAchievement.SetActive(false);
	}

	public void openOption(){
		playClickSound();
		dialogAchievement.SetActive(false);
		dialogOption.GetComponent<TweenPosition>().ResetToBeginning();
		dialogOption.GetComponent<TweenPosition>().PlayForward();
		dialogOption.SetActive(true);

	}
	public void closeOption(){
		playClickSound();
		dialogOption.SetActive(false);
	
	}
	public void toUpgradeScene(){
		playClickSound();
		SceneManager.LoadScene("UpgradeScene");
	}


	public void claimAchievement(GameObject no){
		int acvNo= int.Parse(no.name);
		no.GetComponent<Achievement>().claim();
		SavedDataManager.addMoney(MenuScript.achievementRewardArray[acvNo-1]) ;
		// dua ra thong bao nhan dc tien
		toastMessage.GetComponent<ToastMessage>().show("You've got "+MenuScript.achievementRewardArray[acvNo-1]+"$");
	}
	int soundOn;
	int musicOn;
	public void checkSoundState(){
		soundOn = PlayerPrefs.GetInt ("soundOn",1);
	
		if (soundOn == 0) {
			masterMixer.SetFloat("sfxMix", -80f);
			btSound.GetComponent<UIButton>().normalSprite= "bt-sound-2";
			btSound.GetComponent<UIButton>().pressedSprite= "bt-sound-2";

		} else {
			masterMixer.SetFloat("sfxMix", 0f);
			btSound.GetComponent<UIButton>().normalSprite= "bt-sound-1";
			btSound.GetComponent<UIButton>().pressedSprite= "bt-sound-1";
		}

	

	}
	public void changeSoundState(){
		playClickSound();
		soundOn = PlayerPrefs.GetInt ("soundOn",1);
		if (soundOn == 1) {
			masterMixer.SetFloat("sfxMix", -80f);
			PlayerPrefs.SetInt("soundOn",0);

			btSound.GetComponent<UIButton>().normalSprite= "bt-sound-2";
			btSound.GetComponent<UIButton>().pressedSprite= "bt-sound-2";
	
		} else {
			masterMixer.SetFloat("sfxMix", 0f);
			PlayerPrefs.SetInt("soundOn",1);

			btSound.GetComponent<UIButton>().normalSprite= "bt-sound-1";
			btSound.GetComponent<UIButton>().pressedSprite= "bt-sound-1";
		}

	}
	public void checkMusicState(){
		musicOn = PlayerPrefs.GetInt ("musicOn",1);

		if (musicOn == 0) {
			masterMixer.SetFloat("musicMix", -80f);
			btMusic.GetComponent<UIButton>().normalSprite= "bt-music-2";
			btMusic.GetComponent<UIButton>().pressedSprite= "bt-music-2";

		} else {
			masterMixer.SetFloat("musicMix", 0f);
			btMusic.GetComponent<UIButton>().normalSprite= "bt-music-1";
			btMusic.GetComponent<UIButton>().pressedSprite= "bt-music-1";
		}



	}
	public void changeMusicState(){
		playClickSound();
		musicOn = PlayerPrefs.GetInt ("musicOn",1);
		if (musicOn == 1) {
			masterMixer.SetFloat("musicMix", -80f);
			PlayerPrefs.SetInt("musicOn",0);

			btMusic.GetComponent<UIButton>().normalSprite= "bt-music-2";
			btMusic.GetComponent<UIButton>().pressedSprite= "bt-music-2";

		} else {
			masterMixer.SetFloat("musicMix", 0f);
			PlayerPrefs.SetInt("musicOn",1);

			btMusic.GetComponent<UIButton>().normalSprite= "bt-music-1";
			btMusic.GetComponent<UIButton>().pressedSprite= "bt-music-1";
		}

	}
	public void playLogoSound(){
		GetComponents<AudioSource>()[1].PlayOneShot(logoMoveInSound);

	}
	public void playClickSound(){
		GetComponents<AudioSource>()[1].PlayOneShot(clickSound);
	}
	public void rateGame(){

		playClickSound();
		#if UNITY_ANDROID
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.arrowhitech.furiousroad");
		#elif UNITY_IOS
		// Thay cai nay bang link ios moi'
		Application.OpenURL("https://itunes.apple.com/us/app/lets-get-45/id1046445808?ls=1&mt=8");
		#endif

		if (SavedDataManager.getAchievementState(16)==0){
			SavedDataManager.completeAchievement(16);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
