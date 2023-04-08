using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private AdsBanner bannerAd;
    [SerializeField] private AdsSkippable interstitialAd;
    [SerializeField] private AdsRewarded rewardedAd;

    public static AdsManager instance { get; private set; } = null;
    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }


    public bool ShowBannerAd()
    {
        // Call the ShowAd function on the bannerAd object
        return bannerAd.ShowBannerAd();
    }
    public bool ShowInterstitialAd()
    {
        // Call the ShowAd function on the interstitialAd object
        return interstitialAd.ShowInterstitialAd();
    }
    public bool ShowRewardedAd()
    {
        // Call the ShowAd function on the rewardedAd object
        return rewardedAd.ShowRewardedVideo();
    }
}
