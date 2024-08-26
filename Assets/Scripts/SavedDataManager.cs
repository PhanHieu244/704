using UnityEngine;
using System.Collections;

public class SavedDataManager : MonoBehaviour
{

	public static void firstCreateCarData(){
		unlockMap(0);
		PlayerPrefs.SetInt(GameConstant.UNLOCK+"1",1);
		PlayerPrefs.SetInt(GameConstant.UNLOCK+"2",0);
		PlayerPrefs.SetInt(GameConstant.UNLOCK+"3",0);
		PlayerPrefs.SetInt(GameConstant.UNLOCK+"4",0);
		PlayerPrefs.SetInt(GameConstant.UNLOCK+"5",0);

		PlayerPrefs.SetInt(GameConstant.SPEED+"1",1);
		PlayerPrefs.SetInt(GameConstant.HANDLE+"1",1);
		PlayerPrefs.SetInt(GameConstant.ACCELERATION+"1",1);
		PlayerPrefs.SetInt(GameConstant.BRAKE+"1",1);

		PlayerPrefs.SetInt(GameConstant.SPEED+"2",2);
		PlayerPrefs.SetInt(GameConstant.HANDLE+"2",2);
		PlayerPrefs.SetInt(GameConstant.ACCELERATION+"2",1);
		PlayerPrefs.SetInt(GameConstant.BRAKE+"2",2);

		PlayerPrefs.SetInt(GameConstant.SPEED+"3",3);
		PlayerPrefs.SetInt(GameConstant.HANDLE+"3",4);
		PlayerPrefs.SetInt(GameConstant.ACCELERATION+"3",2);
		PlayerPrefs.SetInt(GameConstant.BRAKE+"3",2);

		PlayerPrefs.SetInt(GameConstant.SPEED+"4",4);
		PlayerPrefs.SetInt(GameConstant.HANDLE+"4",5);
		PlayerPrefs.SetInt(GameConstant.ACCELERATION+"4",3);
		PlayerPrefs.SetInt(GameConstant.BRAKE+"4",2);

		PlayerPrefs.SetInt(GameConstant.SPEED+"5",5);
		PlayerPrefs.SetInt(GameConstant.HANDLE+"5",5);
		PlayerPrefs.SetInt(GameConstant.ACCELERATION+"5",5);
		PlayerPrefs.SetInt(GameConstant.BRAKE+"5",5);

		PlayerPrefs.Save();


	}
	public static CarProperties getCarNo(int no){
		int speed = PlayerPrefs.GetInt(GameConstant.SPEED+no,1);
		int handle = PlayerPrefs.GetInt(GameConstant.HANDLE+no,1);
		int skill = PlayerPrefs.GetInt(GameConstant.ACCELERATION+no,1);
		int durability = PlayerPrefs.GetInt(GameConstant.BRAKE+no,1);

		CarProperties properties  = new CarProperties(speed,handle,skill,durability);
		return properties;
	}
	public static int getCurrentSeledtedMap(){
		int map = PlayerPrefs.GetInt(GameConstant.SELECTED_MAP,0);
		return map;
	}
	public static void setSelectedMap(int map){
		PlayerPrefs.SetInt(GameConstant.SELECTED_MAP,map);
		PlayerPrefs.Save();
	
	}
	public static void setCarNo(int no,CarProperties pro){
		PlayerPrefs.SetInt(GameConstant.SPEED+no,pro.mSpeed);
		PlayerPrefs.SetInt(GameConstant.HANDLE+no,pro.mHandle);
		PlayerPrefs.SetInt(GameConstant.ACCELERATION+no,pro.mAcceleration);
		PlayerPrefs.SetInt(GameConstant.BRAKE+no,pro.mBrake);
		PlayerPrefs.Save();
	
	
	}
	public static void checkFullUpgrade(int no){

		int speed = PlayerPrefs.GetInt(GameConstant.SPEED+no,1);
		int handle = PlayerPrefs.GetInt(GameConstant.HANDLE+no,1);
		int skill = PlayerPrefs.GetInt(GameConstant.ACCELERATION+no,1);
		int durability = PlayerPrefs.GetInt(GameConstant.BRAKE+no,1);

		switch (no){
		case 1:
			break;
		}

	}
	public static void unlockCarNo(int no){
		PlayerPrefs.SetInt(GameConstant.UNLOCK+no,1);
		PlayerPrefs.Save();
	}
	public static int getNumberOfUnlockedCar(){
		int number =0;
		if (PlayerPrefs.GetInt(GameConstant.UNLOCK+"1",1)==1) number++;
		if (PlayerPrefs.GetInt(GameConstant.UNLOCK+"2",0)==1) number++;
		if (PlayerPrefs.GetInt(GameConstant.UNLOCK+"3",0)==1) number++;
		if (PlayerPrefs.GetInt(GameConstant.UNLOCK+"4",0)==1) number++;
		if (PlayerPrefs.GetInt(GameConstant.UNLOCK+"5",0)==1) number++;

		return number;
	}
	public static void setCurrentMoney(int amount){
		PlayerPrefs.SetInt(GameConstant.CURRENT_MONEY,amount);
		PlayerPrefs.Save();
	}
	public static bool hasPurchaseRemoveAd(){
		if (PlayerPrefs.GetInt(GameConstant.PURCHASE_REMOVE_AD,0)==1){
			return true;
		} else return false;
	}
	public static void addMoney(int amount){
		int currentMoney= getCurrentMoney();
		currentMoney+=amount;
		PlayerPrefs.SetInt(GameConstant.CURRENT_MONEY,currentMoney);
		PlayerPrefs.Save();
	}
	public static int getCurrentMoney(){
		return PlayerPrefs.GetInt(GameConstant.CURRENT_MONEY,0);
	}

	public static int getSelectedCar(){
		return PlayerPrefs.GetInt(GameConstant.CURRENT_SELECTED_CAR,0);
	}

	public static int getRecordDistance(){
		return PlayerPrefs.GetInt(GameConstant.NEW_RECORD_DISTANCE,0);
			
	}
	public static void setRecordDistance(int distance){
		PlayerPrefs.SetInt(GameConstant.NEW_RECORD_DISTANCE,distance);
		PlayerPrefs.Save();
	}
	public static int getTotalDistance(){
		return	PlayerPrefs.GetInt(GameConstant.TOTAL_DISTANCE,0);
	}
	public static void increaseTotalDistance(int dis){
		int total= getTotalDistance()+dis;
		PlayerPrefs.SetInt(GameConstant.TOTAL_DISTANCE,total);
		PlayerPrefs.Save();
	}
	public static int isMapUnlock(int map){
		int isunlock=PlayerPrefs.GetInt(GameConstant.UNLOCK_MAP+map,0);
		return isunlock;
	}
	public static void unlockMap(int map){
		PlayerPrefs.SetInt(GameConstant.UNLOCK_MAP+map,1);
		PlayerPrefs.Save();
	}
	
	public static int getAchievementState(int acvNo){
		switch (acvNo){
		case 1:
			return PlayerPrefs.GetInt(GameConstant.RACE_1000M,0);
		case 2:
			return PlayerPrefs.GetInt(GameConstant.RACE_10000M,0);
		case 3:
			return PlayerPrefs.GetInt(GameConstant.RACE_100000M,0);
		case 4:
			return PlayerPrefs.GetInt(GameConstant.RECORD_500M,0);
		case 5:
			return PlayerPrefs.GetInt(GameConstant.RECORD_2000M,0);
		case 6:
			return PlayerPrefs.GetInt(GameConstant.RECORD_5000M,0);
		case 7:
			return PlayerPrefs.GetInt(GameConstant.HAVE_2CAR,0);
		case 8:
			return PlayerPrefs.GetInt(GameConstant.HAVE_3CAR,0);
		case 9:
			return PlayerPrefs.GetInt(GameConstant.HAVE_5CAR,0);
		case 10:
			return PlayerPrefs.GetInt(GameConstant.HAVE_10000,0);
		case 11:
			return PlayerPrefs.GetInt(GameConstant.HAVE_100000,0);
		case 12:
			return PlayerPrefs.GetInt(GameConstant.HAVE_1000000,0);
		case 13:
			return PlayerPrefs.GetInt(GameConstant.COLLECT_SHIELD,0);
		case 14:
			return PlayerPrefs.GetInt(GameConstant.CRASH,0);
		case 15:
			return PlayerPrefs.GetInt(GameConstant.SHARE,0);
		case 16:
			return PlayerPrefs.GetInt(GameConstant.RATE,0);
		case 17:
			return PlayerPrefs.GetInt(GameConstant.UPGRADE,0);
		case 18:
			return PlayerPrefs.GetInt(GameConstant.COLLECT_CASH,0);
		case 19:
			return PlayerPrefs.GetInt(GameConstant.MAX_UPGRADE1,0);
		case 20:
			return PlayerPrefs.GetInt(GameConstant.MAX_UPGRADE2,0);
		case 21:
			return PlayerPrefs.GetInt(GameConstant.MAX_UPGRADE3,0);
		default:
			return 0;
		}

	}
	public static void completeAchievement(int acvNo){
		switch (acvNo){
		case 1:
			 PlayerPrefs.SetInt(GameConstant.RACE_1000M,1);
			break;
		case 2:
			 PlayerPrefs.SetInt(GameConstant.RACE_10000M,1);
			break;
		case 3:
			 PlayerPrefs.SetInt(GameConstant.RACE_100000M,1);
			break;
		case 4:
			 PlayerPrefs.SetInt(GameConstant.RECORD_500M,1);
			break;
		case 5:
			 PlayerPrefs.SetInt(GameConstant.RECORD_2000M,1);
			break;
		case 6:
			 PlayerPrefs.SetInt(GameConstant.RECORD_5000M,1);
			break;
		case 7:
			 PlayerPrefs.SetInt(GameConstant.HAVE_2CAR,1);
			break;
		case 8:
			 PlayerPrefs.SetInt(GameConstant.HAVE_3CAR,1);
			break;
		case 9:
			 PlayerPrefs.SetInt(GameConstant.HAVE_5CAR,1);
			break;
		case 10:
			 PlayerPrefs.SetInt(GameConstant.HAVE_10000,1);
			break;
		case 11:
			 PlayerPrefs.SetInt(GameConstant.HAVE_100000,1);
			break;
		case 12:
			 PlayerPrefs.SetInt(GameConstant.HAVE_1000000,1);
			break;
		case 13:
			 PlayerPrefs.SetInt(GameConstant.COLLECT_SHIELD,1);
			break;
		case 14:
			 PlayerPrefs.SetInt(GameConstant.CRASH,1);
			break;
		case 15:
			 PlayerPrefs.SetInt(GameConstant.SHARE,1);
			break;
		case 16:
			 PlayerPrefs.SetInt(GameConstant.RATE,1);
			break;
		case 17:
			 PlayerPrefs.SetInt(GameConstant.UPGRADE,1);
			break;
		case 18:
			 PlayerPrefs.SetInt(GameConstant.COLLECT_CASH,1);
			break;
		case 19:
			 PlayerPrefs.SetInt(GameConstant.MAX_UPGRADE1,1);
			break;
		case 20:
			 PlayerPrefs.SetInt(GameConstant.MAX_UPGRADE2,1);
			break;
		case 21:
			 PlayerPrefs.SetInt(GameConstant.MAX_UPGRADE3,1);
			break;
	
		}
		PlayerPrefs.Save();

	}
	public static void claimAchievement(int acvNo){
		switch (acvNo){
		case 1:
			PlayerPrefs.SetInt(GameConstant.RACE_1000M,2);
			break;
		case 2:
			PlayerPrefs.SetInt(GameConstant.RACE_10000M,2);
			break;
		case 3:
			PlayerPrefs.SetInt(GameConstant.RACE_100000M,2);
			break;
		case 4:
			PlayerPrefs.SetInt(GameConstant.RECORD_500M,2);
			break;
		case 5:
			PlayerPrefs.SetInt(GameConstant.RECORD_2000M,2);
			break;
		case 6:
			PlayerPrefs.SetInt(GameConstant.RECORD_5000M,2);
			break;
		case 7:
			PlayerPrefs.SetInt(GameConstant.HAVE_2CAR,2);
			break;
		case 8:
			PlayerPrefs.SetInt(GameConstant.HAVE_3CAR,2);
			break;
		case 9:
			PlayerPrefs.SetInt(GameConstant.HAVE_5CAR,2);
			break;
		case 10:
			PlayerPrefs.SetInt(GameConstant.HAVE_10000,2);
			break;
		case 11:
			PlayerPrefs.SetInt(GameConstant.HAVE_100000,2);
			break;
		case 12:
			PlayerPrefs.SetInt(GameConstant.HAVE_1000000,2);
			break;
		case 13:
			PlayerPrefs.SetInt(GameConstant.COLLECT_SHIELD,2);
			break;
		case 14:
			PlayerPrefs.SetInt(GameConstant.CRASH,2);
			break;
		case 15:
			PlayerPrefs.SetInt(GameConstant.SHARE,2);
			break;
		case 16:
			PlayerPrefs.SetInt(GameConstant.RATE,2);
			break;
		case 17:
			PlayerPrefs.SetInt(GameConstant.UPGRADE,2);
			break;
		case 18:
			PlayerPrefs.SetInt(GameConstant.COLLECT_CASH,2);
			break;
		case 19:
			PlayerPrefs.SetInt(GameConstant.MAX_UPGRADE1,2);
			break;
		case 20:
			PlayerPrefs.SetInt(GameConstant.MAX_UPGRADE2,2);
			break;
		case 21:
			PlayerPrefs.SetInt(GameConstant.MAX_UPGRADE3,2);
			break;
		
		}
		PlayerPrefs.Save();

	}
}

