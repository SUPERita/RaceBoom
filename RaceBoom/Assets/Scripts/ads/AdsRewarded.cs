using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements; //Assuming you imported the Advertisements from the "Package Manager"
public class AdsRewarded : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    public string gameId = "Your-Apple-ID";
    public string mySurfacingId = "Rewarded_iOS";
#elif UNITY_ANDROID
    public string gameId = "Your-Google-ID";
    public string mySurfacingId = "Rewarded_Android";
#endif
    public bool testMode = true; //Leave this as True UNTIL you release your game!!!

    void Start()
    {
        Advertisement.AddListener(this);    //Used to check if Player COMPLETED the ad
        Advertisement.Initialize(gameId, testMode);     // Initialize the Ads listener and service:
    }

    public void ShowRewardedVideo() //Shows The add when this method is called - 
    {   // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId)) Advertisement.Show(mySurfacingId);
        else Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
    }
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult) // Implement IUnityAdsListener interface methods:
    {
        if (showResult == ShowResult.Finished)
        {
            print("The Ad finished!!!");
        }
        else if (showResult == ShowResult.Skipped)
        {
            print("The Ad was SKIPPED you Cheater...");
        }
        else if (showResult == ShowResult.Failed) print("The Ad was interrupted or Failed.");
    }
    public void OnUnityAdsReady(string surfacingId) //Activates when ADD is ready
    {// If the ready Ad Unit or legacy Placement is rewarded, show the ad:        
        if (surfacingId == mySurfacingId)
        {// Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
            print("The Ad is ready - Lord Vader");
        }
    }
    public void OnUnityAdsDidError(string message) // Log the error.
    {
        print("Something's wrong, it's... the Ad's not working!!!");
    }
    public void OnUnityAdsDidStart(string surfacingId) // Optional actions to take when the end-users triggers an ad.
    {
        print("this is extra");
    }
    /*
    public void OnDestro() 
    {
        print("The object your Ad's were attached to has BEEN DESTROYED");
        Advertisement.RemoveListener(this);
    }
    */
}