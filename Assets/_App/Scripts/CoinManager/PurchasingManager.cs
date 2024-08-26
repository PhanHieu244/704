using UnityEngine;

namespace _App.Scripts.CoinManager
{
   public class PurchasingManager : MonoBehaviour
   {
      [SerializeField] private GarageController _garageController;

      public void OnPressDown(int i)
      {
         switch (i)
         {
            case 1:
               SavedDataManager.addMoney(1000);
               IAPManager.Instance.BuyProductID(IAPKey.PACK1);
               break;
            case 2:
               SavedDataManager.addMoney(3000);
               IAPManager.Instance.BuyProductID(IAPKey.PACK2);
               break;
            case 3:
               SavedDataManager.addMoney(5000);
               IAPManager.Instance.BuyProductID(IAPKey.PACK3);
               break;
            case 4:
               SavedDataManager.addMoney(10000);
               IAPManager.Instance.BuyProductID(IAPKey.PACK4);
               break;
         }
         _garageController.updateCurrentMoney();
      }

      public void Sub(int i)
      {
         GameDataManager.Instance.playerData.SubDiamond(i);
      }
   }
}
