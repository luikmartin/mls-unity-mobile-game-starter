using Unity.Services.Mediation;
using UnityEngine;

public class Ad : AdBase<IInterstitialAd, IInterstitialAd>
{
    public async void ShowInterstitial()
    {
        if (ad?.AdState == AdState.Loaded)
        {
            try
            {
                var showOptions = new InterstitialAdShowOptions { AutoReload = true };
                await ad.ShowAsync(showOptions);
                Debug.Log("Interstitial Shown!");
            }
            catch (ShowFailedException e)
            {
                Debug.Log($"Interstitial failed to show : {e.Message}");
            }
        }
    }

    public override void InitializationComplete()
    {
        // Impression Event
        MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                ad = MediationService.Instance.CreateInterstitialAd(androidAdUnitId);
                break;
            case RuntimePlatform.IPhonePlayer:
                ad = MediationService.Instance.CreateInterstitialAd(iosAdUnitId);
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.LinuxEditor:
                ad = MediationService.Instance.CreateInterstitialAd(!string.IsNullOrEmpty(androidAdUnitId) ? androidAdUnitId : iosAdUnitId);
                break;
            default:
                Debug.LogWarning("Mediation service is not available for this platform:" + Application.platform);
                return;
        }

        // Load Events
        ad.OnLoaded += AdLoaded;
        ad.OnFailedLoad += AdFailedLoad;

        // Show Events
        ad.OnClosed += AdClosed;

        Debug.Log("Initialized On Start! Loading Ad...");
        LoadAd();
    }

    public override async void LoadAd()
    {
        try
        {
            await ad.LoadAsync();
        }
        catch (LoadFailedException e)
        {
            AdFailedLoad(e);
        }
    }
}
