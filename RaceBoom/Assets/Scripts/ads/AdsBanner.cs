using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements; //Assuming you imported the Advertisements from the "Package Manager"

public class AdsBanner : MonoBehaviour
{
#if UNITY_IOS
    public string gameId = "Your-Apple-ID";
    public string mySurfacingId = "Banner_iOS";
#elif UNITY_ANDROID
    public string gameId = "Your-Google-ID";
    public string mySurfacingId = "Banner_Android";
#endif
    public bool testMode = true; //Leave this as True UNTIL you release your game!!!
    
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        //StartCoroutine(ShowBannerWhenInitialized());
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER); //Positions Banner where you want -
    }

    public bool ShowBannerAd()
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.Banner.Show(mySurfacingId);
            return true;
        } else
        {
            Debug.Log("!Advertisement.isInitialized");
            return false;
        }
    }
    /*
    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(mySurfacingId);
    }
    */
}