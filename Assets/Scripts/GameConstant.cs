public class GameConstant
{
	public static string CURRENT_SELECTED_CAR ="current_selected_car";
	public static string FIRST_PLAY ="first_play";
	public static string CURRENT_MONEY ="current_money";
	public static string NEW_RECORD_DISTANCE ="record_distance";
	public static string UNLOCK ="unlock";
	public static string SELECTED_MAP ="selectedMap";
	public static string UNLOCK_MAP ="unlockmap";

	public static string SPEED ="speed";
	public static string HANDLE ="handle";
	public static string ACCELERATION ="acceleration";
	public static string BRAKE ="brake";

	public static string RACE_1000M ="race_1000";
	public static string RACE_10000M ="race_10000";
	public static string RACE_100000M ="race_100000";

	public static string RECORD_500M ="record_500";
	public static string RECORD_2000M ="record_2000";
	public static string RECORD_5000M ="record_5000";

	public static string HAVE_2CAR ="have_2car";
	public static string HAVE_3CAR ="have_3car";
	public static string HAVE_5CAR ="have_5car";

	public static string HAVE_10000 ="have_10000";
	public static string HAVE_100000 ="have_100000";
	public static string HAVE_1000000 ="have_1000000";

	public static string COLLECT_SHIELD ="collect_shield";
	public static string CRASH ="crash";
	public static string SHARE ="share";
	public static string RATE ="rate";
	public static string UPGRADE ="upgrade";
	public static string COLLECT_CASH ="collect_cash";
	public static string MAX_UPGRADE1 ="max_upgrade1";
	public static string MAX_UPGRADE2 ="max_upgrade2";
	public static string MAX_UPGRADE3 ="max_upgrade3";

	public static string TOTAL_DISTANCE ="total_distance";
	public static string PURCHASE_REMOVE_AD="remove_ads";
	public static int PACK_1_DOLLAR_AMOUNT= 100000;
	public static int PACK_2_DOLLAR_AMOUNT= 220000;
	public static int PACK_3_DOLLAR_AMOUNT= 360000;



	public static string thounsandSeparator(int number){
		if (number!=0){
		string numberString= string.Format("{0:#,###}",number);
		numberString= numberString.Replace(",",".");
			return numberString;}
		else return "0";
	}

}

