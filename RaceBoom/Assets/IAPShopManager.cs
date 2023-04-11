using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
public class IAPShopManager : MonoBehaviour
{
    public void Button_OnBuy(Product _product)
    {
        switch (_product.definition.id)
        {
            case "10k_coins":
                ResourceManager.AddCoins(10000);
                MessageManager.instance.SendMessage("+10,000!");
                SoundPool.instance.PlaySound("item_unlocked");
                break;
            default:
                Debug.Log($"no Button_OnBuy action set for {_product.definition.id}");
                break;
        }
    }
}
