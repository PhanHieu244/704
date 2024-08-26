using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GarageController : MonoBehaviour {
	public int[] carPriceList;
	public int[] upgradeSpeedCost;
	public int[] upgradeHandleCost;
	public int[] upgradeAcceleCost;
	public int[] upgradeBrakeCost;

	public GameObject shop;
	public Transform showCarPos,leftPos,rightPos;
	public GameObject buttonNext,buttonPrevious,buttonBuy,buttonPlay,lblCarPrice,lblCurrentMoney,dialogConfirm,toastMessage;
	public GameObject[] carList;
	public GameObject[] statBars;
	public AudioClip clickSound,slideSound;
	public AudioClip pSound,upgradeSound;
	public GameObject[] purchaseButton;
	int currentMoney;
	int tempMoney;
	int selectedCar;

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	//	Application.targetFrameRate=60;
		selectedCar=PlayerPrefs.GetInt(GameConstant.CURRENT_SELECTED_CAR,0);
		updateCurrentMoney();
		setButtonVisible();
		setCarOrder();
		animStatBar();
		checkCarAvailability();
	

	}
	public void playClickSound(){
		GetComponents<AudioSource>()[1].PlayOneShot(clickSound);
	}
	public void playSlideSound(){
		GetComponents<AudioSource>()[1].PlayOneShot(slideSound);
	}
	public void playPurchaseSound()
	{
		GetComponents<AudioSource>()[1].PlayOneShot(pSound);
	}
	public void playUpgradeSound()
	{
		GetComponents<AudioSource>()[1].PlayOneShot(upgradeSound);
	}
	 public void updateCurrentMoney(){
		
		currentMoney=SavedDataManager.getCurrentMoney();
		lblCurrentMoney.GetComponent<UILabel>().text=""+ GameConstant.thounsandSeparator(currentMoney);
	}

	void checkCarAvailability(){
		int available= PlayerPrefs.GetInt("unlock"+(selectedCar+1),0);
		checkMaxUpgrade();
		if (available==0){
			buttonBuy.SetActive(true);
			buttonPlay.SetActive(false);
			lblCarPrice.SetActive(true);
			for (int i=0;i<purchaseButton.Length;i++)
				purchaseButton[i].SetActive(false);
			lblCarPrice.GetComponent<UILabel>().text= "$ "+ GameConstant.thounsandSeparator(carPriceList[selectedCar]);
		
			
		} else {
			buttonBuy.SetActive(false);
			buttonPlay.SetActive(true);
			lblCarPrice.SetActive(false);
			for (int i=0;i<purchaseButton.Length;i++)
				purchaseButton[i].SetActive(true);
		

			CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);

			if (currentProperties.mSpeed== carList[selectedCar].GetComponent<CarMaxUpgrade>().maxSpeed){
				purchaseButton[0].SetActive(false);
			}
			if (currentProperties.mHandle== carList[selectedCar].GetComponent<CarMaxUpgrade>().maxHandle){
				purchaseButton[1].SetActive(false);
			}
			if (currentProperties.mAcceleration== carList[selectedCar].GetComponent<CarMaxUpgrade>().maxAcceleration){
				purchaseButton[2].SetActive(false);
			}
			if (currentProperties.mBrake== carList[selectedCar].GetComponent<CarMaxUpgrade>().maxBrake){
				purchaseButton[3].SetActive(false);
			}

			PlayerPrefs.SetInt(GameConstant.CURRENT_SELECTED_CAR,selectedCar);
			PlayerPrefs.Save();
		}
	}
	void checkMaxUpgrade(){
		int countMaxUpgradeCar=0;
		for (int i =0 ;i< 5;i++){
			CarProperties currentProperties= SavedDataManager.getCarNo(i+1);
			if (currentProperties.mSpeed==carList[i].GetComponent<CarMaxUpgrade>().maxSpeed )
				if (currentProperties.mHandle==carList[i].GetComponent<CarMaxUpgrade>().maxHandle )
					if (currentProperties.mAcceleration==carList[i].GetComponent<CarMaxUpgrade>().maxAcceleration )
						if (currentProperties.mBrake==carList[i].GetComponent<CarMaxUpgrade>().maxBrake )
							countMaxUpgradeCar++;
				
			}
		if  (countMaxUpgradeCar==1){
			if (SavedDataManager.getAchievementState(19)==0)
				SavedDataManager.completeAchievement(19);
		} else if  (countMaxUpgradeCar==2){
			if (SavedDataManager.getAchievementState(20)==0)
				SavedDataManager.completeAchievement(20);
		} else if  (countMaxUpgradeCar==3){
			if (SavedDataManager.getAchievementState(21)==0)
				SavedDataManager.completeAchievement(21);
		} 

	}
	void animStatBar(){
		CarProperties currentCar= SavedDataManager.getCarNo(selectedCar+1);
		statBars[0].GetComponent<CarStatsBar>().setCarStats(currentCar.mSpeed);
		statBars[1].GetComponent<CarStatsBar>().setCarStats(currentCar.mHandle);
		statBars[2].GetComponent<CarStatsBar>().setCarStats(currentCar.mAcceleration);
		statBars[3].GetComponent<CarStatsBar>().setCarStats(currentCar.mBrake);
	}
	void setCarOrder(){
		for (int i=0;i<carList.Length;i++){
			if (i<selectedCar){
				carList[i].transform.position= leftPos.position;
			} else if (i>selectedCar){
				carList[i].transform.position= rightPos.position;
			}
		}
		carList[selectedCar].transform.position= showCarPos.position;
	}
	void setButtonVisible(){
		buttonNext.SetActive(true);
		buttonPrevious.SetActive(true);
		if (selectedCar == 0 ) {
			buttonNext.SetActive(false);
			buttonPrevious.SetActive(true);
		}
		if (selectedCar == carList.Length-1 ) {
	
			buttonNext.SetActive(true);
			buttonPrevious.SetActive(false);
		}

	}
	public void nextCar(){
		playClickSound();
		playSlideSound();
		//Debug.Log("nextCar");
		carList[selectedCar].GetComponent<CharacterRotating>().moveToPosition(leftPos.position);

		if (selectedCar<carList.Length-1){
			selectedCar++;

			animStatBar();
			checkCarAvailability();
		}

		carList[selectedCar].GetComponent<CharacterRotating>().resetRotateAngle();
		carList[selectedCar].GetComponent<CharacterRotating>().moveToPosition(showCarPos.position);
		setButtonVisible();
		
	}

	public void previousCar(){
		playClickSound();
		playSlideSound();
	//	Debug.Log("previousCar");
		carList[selectedCar].GetComponent<CharacterRotating>().moveToPosition(rightPos.position);
		if (selectedCar>0){
			selectedCar--;
		
			animStatBar();
			checkCarAvailability();
		}
		carList[selectedCar].GetComponent<CharacterRotating>().resetRotateAngle();
		carList[selectedCar].GetComponent<CharacterRotating>().moveToPosition(showCarPos.position);
		setButtonVisible();
	}

	// Update is called once per frame
	void Update () {

	}
	public void toMapScene(){
		playClickSound();
		SceneManager.LoadScene("MapScene");
	}
	public void purchaseSpeed(){
		playClickSound();
		CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
		if (currentProperties.mSpeed< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxSpeed){
			int cost = upgradeSpeedCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxSpeed-currentProperties.mSpeed)];
			Debug.Log(""+cost);
			if (currentMoney>=cost){
				currentMoney-=cost;
				checkAchievement();
				SavedDataManager.setCurrentMoney(currentMoney);
				updateCurrentMoney();
				playUpgradeSound();
				currentProperties.mSpeed+=1;
				SavedDataManager.setCarNo(selectedCar+1,currentProperties);
				animStatBar();
				checkCarAvailability();
				closeDialogConfirm();
			} else {
				closeDialogConfirm();
				toastMessage.GetComponent<ToastMessage>().show("Not enough money!");
			}
		} 
	}
	public void purchaseHandle(){
		playClickSound();
		CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
		if (currentProperties.mHandle< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxHandle){
			int cost = upgradeHandleCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxHandle-currentProperties.mHandle)];
			Debug.Log(""+cost);
			if (currentMoney>=cost){
				currentMoney-=cost;
				checkAchievement();
				SavedDataManager.setCurrentMoney(currentMoney);
				playUpgradeSound();
				updateCurrentMoney();
				currentProperties.mHandle+=1;
				SavedDataManager.setCarNo(selectedCar+1,currentProperties);
				animStatBar();
				checkCarAvailability();
				closeDialogConfirm();
			} else {
				closeDialogConfirm();
				toastMessage.GetComponent<ToastMessage>().show("Not enough money!");
			}
		} 
	}
	public void purchaseAcceleration(){
		playClickSound();
		CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
		if (currentProperties.mAcceleration< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxAcceleration){
			int cost = upgradeAcceleCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxAcceleration-currentProperties.mAcceleration)];
			Debug.Log(""+cost);
			if (currentMoney>=cost){
				currentMoney-=cost;
				checkAchievement();
				SavedDataManager.setCurrentMoney(currentMoney);
				updateCurrentMoney();
				playUpgradeSound();
				currentProperties.mAcceleration+=1;
				SavedDataManager.setCarNo(selectedCar+1,currentProperties);
				animStatBar();
				checkCarAvailability();
				closeDialogConfirm();
			} else {
				closeDialogConfirm();
				toastMessage.GetComponent<ToastMessage>().show("Not enough money!");
			}
		} 
	}
	public void purchaseBrake(){
		playClickSound();
		CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
		if (currentProperties.mBrake< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxBrake){
			int cost = upgradeBrakeCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxBrake-currentProperties.mBrake)];
			Debug.Log(""+cost);
			if (currentMoney>=cost){
				currentMoney-=cost;
				checkAchievement();
				SavedDataManager.setCurrentMoney(currentMoney);
				updateCurrentMoney();
				playUpgradeSound();
				currentProperties.mBrake+=1;
				SavedDataManager.setCarNo(selectedCar+1,currentProperties);
				animStatBar();
				checkCarAvailability();
				closeDialogConfirm();
			} else {
				closeDialogConfirm();
				toastMessage.GetComponent<ToastMessage>().show("Not enough money!");
			}
		} 
	}
	GameObject purchaseButtonGameObject;
	void checkAchievement(){
		if (SavedDataManager.getAchievementState(17)==0){
			SavedDataManager.completeAchievement(17);
		}
	}
	public void buyCar(){
		playClickSound();
		if (currentMoney>= carPriceList[selectedCar]){
			currentMoney-=carPriceList[selectedCar];
			SavedDataManager.setCurrentMoney(currentMoney);
			updateCurrentMoney();
			playPurchaseSound();
			SavedDataManager.unlockCarNo(selectedCar+1);
			checkCarAvailability();
		} else {
			Debug.Log("Not enough money");
			toastMessage.GetComponent<ToastMessage>().show("Not enough money!");
		}
	}


	public void showDialogConfirm(GameObject obj){
		playClickSound();
		dialogConfirm.GetComponent<UIScaleAnimation>().open();
		purchaseButtonGameObject=obj;
		string objectName= purchaseButtonGameObject.name;
		if (objectName.Equals("purchaseSpeed")){
			CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
			if (currentProperties.mSpeed< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxSpeed){
				int cost = upgradeSpeedCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxSpeed-currentProperties.mSpeed)];
				dialogConfirm.transform.Find("text").GetComponent<UILabel>().text="Upgrade with "+GameConstant.thounsandSeparator(cost)+"$?";
			}

		} else if (objectName.Equals("purchaseHandle")){
			CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
			if (currentProperties.mHandle< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxHandle){
				int cost = upgradeHandleCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxHandle-currentProperties.mHandle)];
				dialogConfirm.transform.Find("text").GetComponent<UILabel>().text="Upgrade with "+GameConstant.thounsandSeparator(cost)+"$?";
			}

		}else if (objectName.Equals("purchaseAcce")){
			CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
			if (currentProperties.mAcceleration< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxAcceleration){
				int cost = upgradeAcceleCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxAcceleration-currentProperties.mAcceleration)];
				dialogConfirm.transform.Find("text").GetComponent<UILabel>().text="Upgrade with "+GameConstant.thounsandSeparator(cost)+"$?";
					}

		}else if (objectName.Equals("purchaseBrake")){
			CarProperties currentProperties= SavedDataManager.getCarNo(selectedCar+1);
			if (currentProperties.mBrake< carList[selectedCar].GetComponent<CarMaxUpgrade>().maxBrake){
				int cost = upgradeBrakeCost[ 5- (carList[selectedCar].GetComponent<CarMaxUpgrade>().maxBrake-currentProperties.mBrake)];
				dialogConfirm.transform.Find("text").GetComponent<UILabel>().text="Upgrade with "+GameConstant.thounsandSeparator(cost)+"$?";
				}

		}
	
	}
	public void closeDialogConfirm(){
		playClickSound();
		dialogConfirm.GetComponent<UIScaleAnimation>().close();
	}
	public void purchase(){
		string objName= purchaseButtonGameObject.name;
		if (objName.Equals("purchaseSpeed")){
		//	Debug.Log("purchaseSpeed");
			purchaseSpeed();

		} else if (objName.Equals("purchaseHandle")){
		//	Debug.Log("purchaseHandle");
			purchaseHandle();

		}else if (objName.Equals("purchaseAcce")){
		//	Debug.Log("purchaseAcce");
			purchaseAcceleration();

		}else if (objName.Equals("purchaseBrake")){
		//	Debug.Log("purchaseBrake");
			purchaseBrake();

		}
	
	}
	public GameObject dialogShop;
	public void openShop(){
		shop.gameObject.SetActive(true);
	}

	public void closeDialogShop(){
		dialogShop.SetActive(false);
	}
	void onRemoveAdGarage(){
		toastMessage.GetComponent<ToastMessage>().show("Purchase Succeed!");
		if (SavedDataManager.hasPurchaseRemoveAd())
			dialogShop.transform.Find("btRemoveAd").gameObject.SetActive(false);
	}
	void onPurchasePackSuccessed(){
		toastMessage.GetComponent<ToastMessage>().show("Purchase Succeed!");
		updateCurrentMoney();
	}
}
