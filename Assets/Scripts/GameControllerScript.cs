using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
public class GameControllerScript : MonoBehaviour
{
    public enum GAME_STATE { PAUSE, GAME_OVER, RUNNING };
    public static GAME_STATE gameState;
	//public AudioClip[] carEngineSound;
	public AudioClip click_sound,desertBg,snowBg;
    public float GAS_TIME_CONSTANT;
    public float GAS_TIME_BONUS;
    public int SPEED_BONUS_SCORE;
    public int COMBO_REQUIRE_TIME;
    public int MAX_COMBO;
    public int MONEY_DEVIDE_BY;
	public int GAS_WARNING_TIME;

    public static GameControllerScript instance;

    float gasTime;
    float gasTimeStep;
    public GameObject gasPrefab;
    public GameObject mainCam;
    public GameObject[] carPrefabList;
    public GameObject playerStartPos;
	public UILabel lblScore, lblSpeed, lblDistance, lblcombo;
	public UISprite gasIcon,gasBackground;
    public UIProgressBar gasProgress;
    public GameObject highScoreLine, goDistance, goScore, goBestRecord,goPickMoney;
    public GameObject goRewardMoney;
    CarController playerCarController;
    GameObject playerCar;
    GameObject road;
    public GameObject dialogGameOver, dialogPause;
  //  List<GameObject> roadList;
    bool isBoost;
    int speedKmperHour;
    int currentScore;
    int recordDistance, currentDistance;
    int selectedCar;
    int speedCombo, comboScore;
    float comboTime;
    bool ispause = false;
    Vector3 tempVelocity;
	public AudioMixer masterMixer;
	public GameObject btSound,btMusic;
	bool startGasWarning=false;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	//	Application.targetFrameRate=60;
        gameState = GAME_STATE.RUNNING;
        isBoost = false;
        speedKmperHour = 0;
        speedCombo = 0;
        comboScore = 0;
        comboTime = 0;
        selectedCar = SavedDataManager.getSelectedCar();
    //    Application.targetFrameRate = 60;
        recordDistance = SavedDataManager.getRecordDistance();
        playerCar = Instantiate(carPrefabList[selectedCar], playerStartPos.transform.position, Quaternion.identity) as GameObject;
        mainCam.GetComponent<CameraScript>().assignPlayerToCamera(playerCar);
		checkSoundState();
		checkMusicState();
        playerCarController = playerCar.GetComponent<CarController>();
        road = GameObject.FindGameObjectWithTag("road");
    //    roadList = new List<GameObject>();
     //   foreach (Transform child in road.transform)
      //  {
        //    roadList.Add(child.gameObject);

      //  }
        gasProgress.value = 1;
        //	gasTimeStep=1f/(gasTime/Time.deltaTime);
        gasTime = GAS_TIME_CONSTANT;
        if (recordDistance > 0)
        {
            GameObject highScoreLineObj = Instantiate(highScoreLine, new Vector3(0, 2f, recordDistance), Quaternion.identity) as GameObject;
        }
		switch (SavedDataManager.getCurrentSeledtedMap()){
		case 0:
			GetComponents<AudioSource>()[0].clip=desertBg;
			GetComponents<AudioSource>()[0].Play();
	
			break;
		case 1:
			GetComponents<AudioSource>()[0].clip=snowBg;
			GetComponents<AudioSource>()[0].Play();
			break;
		}
    }

    public void increaseGasTime()
    {
        gasTime += GAS_TIME_BONUS;
        if (gasTime >= GAS_TIME_CONSTANT) gasTime = GAS_TIME_CONSTANT;
    }
    void Update()
    {
        if (gameState == GAME_STATE.RUNNING)
        {


            gasTime -= Time.deltaTime;
            gasProgress.value = gasTime / GAS_TIME_CONSTANT;


            if (gameState == GAME_STATE.RUNNING && gasTime <= 0)
            {
                GameOver();
                playerCar.GetComponent<CarController>().TimeOut();
            }
            else
            {
                currentDistance = (int)playerCar.transform.position.z;
				speedKmperHour = 	(int)playerCar.GetComponent<CarController>().speedForward/36;
                if (speedKmperHour >= 70)
                {
                    comboTime += Time.deltaTime;
                    if (comboTime >= COMBO_REQUIRE_TIME)
                    {
                        comboTime = 0;
                        speedCombo++;
                        if (speedCombo >= MAX_COMBO) speedCombo = MAX_COMBO;
                        else
                        {
                            lblcombo.gameObject.GetComponent<TweenScale>().ResetToBeginning();
                            lblcombo.gameObject.GetComponent<TweenScale>().PlayForward();

                        }
                        comboScore += speedCombo * SPEED_BONUS_SCORE;
                        lblcombo.gameObject.SetActive(true);
                        lblcombo.text = "COMBO X " + speedCombo;


                        //		Debug.Log("COMBOx" +speedCombo);
                        //		Debug.Log("COMBOx Socre " +comboScore );
                    }

                    lblSpeed.color = Color.green;
                }
                else
                {
                    speedCombo = 0;
                    comboTime = 0;
                    lblcombo.gameObject.SetActive(false);
                    lblSpeed.color = Color.white;
                }

                currentScore = currentDistance + comboScore;

                lblSpeed.text = "" + speedKmperHour + " km/h";
				lblDistance.text = GameConstant.thounsandSeparator((int)playerCar.transform.position.z) + "m";
				lblScore.text = "" +  GameConstant.thounsandSeparator((int)currentScore);
				if (gasTime<=GAS_WARNING_TIME){
					gasIcon.color=Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.3f));
					gasBackground.color=Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.3f));
					if(!startGasWarning){
						startGasWarning=true;
						gasIcon.gameObject.GetComponent<TweenScale>().ResetToBeginning();
						gasIcon.gameObject.GetComponent<TweenScale>().PlayForward();
						gasIcon.gameObject.GetComponent<TweenScale>().enabled=true;
						GetComponents<AudioSource>()[2].Play();
					}
				}else{
					gasIcon.color=Color.white;
					gasBackground.color=Color.white;
					if(startGasWarning){
						startGasWarning=false;
						gasIcon.gameObject.GetComponent<TweenScale>().ResetToBeginning();
						gasIcon.gameObject.GetComponent<TweenScale>().enabled=false;
						GetComponents<AudioSource>()[2].Stop();
					}
				}

            }


            if (Input.GetMouseButtonDown(0))
            {
				if(gameState==GAME_STATE.RUNNING){
					if (!isBoost){
						isBoost = true;
						playBoostSound();
						Invoke("stopBoostSound",2.866f);
					}

					
				}
                

            }
            if (Input.GetMouseButtonUp(0))
            {
                //	Debug.Log("GetMouseButtonUp");
     
				if(gameState==GAME_STATE.RUNNING){
					if (isBoost){
						isBoost = false;
					}

				}
            }
            if (isBoost)
            {
                playerCar.GetComponent<CarController>().boostSpeed();
            }
            else
            {
                playerCar.GetComponent<CarController>().stopBoostSpeed();
            }

        }
    }
	public void playClickSound(){
		GetComponents<AudioSource>()[1].PlayOneShot(click_sound);
	}
	void playBoostSound(){
		playerCar.GetComponents<AudioSource>()[1].Stop();
		playerCar.GetComponents<AudioSource>()[1].PlayOneShot(playerCarController.engine_boost);
		playerCar.GetComponents<AudioSource>()[0].Pause();
	}

	void stopBoostSound(){
		playerCar.GetComponents<AudioSource>()[0].UnPause();
	}
	void checkAchievement(){
		if (SavedDataManager.getTotalDistance()>1000 ){
			if ( SavedDataManager.getAchievementState(1)==0)
				SavedDataManager.completeAchievement(1);
		} 
		if (SavedDataManager.getTotalDistance()>10000 ){
			if ( SavedDataManager.getAchievementState(2)==0)
				SavedDataManager.completeAchievement(2);
		} 
		if (SavedDataManager.getTotalDistance()>100000 ){
			if ( SavedDataManager.getAchievementState(3)==0)
				SavedDataManager.completeAchievement(3);
		} 

		if (currentDistance>=500 ){
			if ( SavedDataManager.getAchievementState(4)==0)
				SavedDataManager.completeAchievement(4);
		} 
		if (currentDistance>=2000  ){
			if ( SavedDataManager.getAchievementState(5)==0)
				SavedDataManager.completeAchievement(5);
		} 
		if (currentDistance>=5000 ){
			if ( SavedDataManager.getAchievementState(6)==0)
				SavedDataManager.completeAchievement(6);
		} 
		if (currentDistance>=1000 ){
			int selectedMap= SavedDataManager.getCurrentSeledtedMap();
			if (selectedMap<1) selectedMap++;
			SavedDataManager.unlockMap(selectedMap);
		} 

	}
    public void GameOver()
    {
	    gameState = GAME_STATE.GAME_OVER;
		pauseSound();
		SavedDataManager.increaseTotalDistance(currentDistance);
		checkAchievement();

        if (currentDistance > recordDistance)
        {
            SavedDataManager.setRecordDistance(currentDistance);
            recordDistance = SavedDataManager.getRecordDistance();
        }
        Time.timeScale = 0;
        int moneyReward = currentScore / 10;
        dialogGameOver.SetActive(true);
        StartCoroutine(Test());
		goDistance.GetComponent<UILabel>().text =  GameConstant.thounsandSeparator(currentDistance) + " m";
		goBestRecord.GetComponent<UILabel>().text =  GameConstant.thounsandSeparator(recordDistance) + " m";
		goScore.GetComponent<UILabel>().text = "" +  GameConstant.thounsandSeparator(currentScore);
		goRewardMoney.GetComponent<UILabel>().text = "" +  GameConstant.thounsandSeparator(moneyReward) + " $";
		goPickMoney.GetComponent<UILabel>().text ="" +  GameConstant.thounsandSeparator(playerCarController.collectedMoney) + " $";
		int totalRewardMoney= moneyReward+playerCarController.collectedMoney;
		SavedDataManager.addMoney(totalRewardMoney);
    }

    IEnumerator Test()
    {
	    yield return new WaitForSecondsRealtime(2f);
	    toMainMenu();
    }

    void pauseGame()
    {
	    dialogPause.SetActive(true);
		gameState = GAME_STATE.PAUSE;
        ispause = true;
        Time.timeScale = 0;
        //dialogPause.GetComponent<UIScaleAnimation>().open();
		pauseSound();
    }
    void resumeGame()
    {
		gameState = GAME_STATE.RUNNING;
        ispause = false;
        Time.timeScale = 1;
        dialogPause.GetComponent<UIScaleAnimation>().close();
		resumeSound();
    }

    public void pressPauseButton()
    {
		playClickSound();
        if (!ispause)
            pauseGame();
        else resumeGame();
    }

    public void toMainMenu()
    {
		playClickSound();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void replay()
    {
		playClickSound();
        SceneManager.LoadScene("GamePlay");
        Time.timeScale = 1;
    }

	void pauseSound(){
		GetComponents<AudioSource>()[0].Pause();
		GetComponents<AudioSource>()[1].Pause();
		GetComponents<AudioSource>()[2].Pause();
		playerCar.GetComponents<AudioSource>()[0].Pause();
		playerCar.GetComponents<AudioSource>()[1].Pause();
		playerCar.GetComponents<AudioSource>()[2].Pause();

	}
	void resumeSound(){
		GetComponents<AudioSource>()[0].UnPause();
		GetComponents<AudioSource>()[1].UnPause();
		GetComponents<AudioSource>()[2].UnPause();
		playerCar.GetComponents<AudioSource>()[0].UnPause();
		playerCar.GetComponents<AudioSource>()[1].UnPause();
		playerCar.GetComponents<AudioSource>()[2].UnPause();
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
	public void shareFacebook(){
		playClickSound();
		shareScreenShot();
	}
	public void shareScreenShot(){
		playClickSound();
		//	yield return new WaitForEndOfFrame();
		ScreenCapture.CaptureScreenshot("share.png");
		string pathToImage = Application.persistentDataPath + "/" + "share.png";
		print("image path "+ pathToImage);
		#if UNITY_ANDROID

		//   byte[] bytes = new byte[];

		//    File.WriteAllBytes(pathToImage, bytes);

		//instantiate the class Intent
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

		//instantiate the object Intent
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		//call setAction setting ACTION_SEND as parameter
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

		//instantiate the class Uri
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");


		//instantiate the object Uri with the parse of the url's file
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://"+pathToImage);

		//call putExtra with the uri object of the file
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

		//set the type of file
		intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");

		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Furious Road");

		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Just played this great game! https://play.google.com/store/apps/details?id=com.arrowhitech.furiousroad  #furiousroad");
		//instantiate the class UnityPlayer
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

		//instantiate the object currentActivity
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		//call the activity with our Intent
		currentActivity.Call("startActivity", intentObject);
		#elif UNITY_IOS
		GeneralSharingiOSBridge.ShareTextWithImage(pathToImage, "Do you dare to get through me? https://itunes.apple.com/us/app/lets-get-45/id1046445808?ls=1&mt=8 #letsget45");

		#endif
		if (SavedDataManager.getAchievementState(15)==0)
			SavedDataManager.completeAchievement(15);
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
}
