using UnityEngine;
using Unity.Services.Mediation;

public class BannerAd : AdBase<IBannerAd, IBannerAd>
{
	[Header("Banner options")]
	public BannerAdAnchor bannerAdAnchor = BannerAdAnchor.TopCenter;
	public BannerAdPredefinedSize bannerSize = BannerAdPredefinedSize.Banner;

	public override void InitializationComplete()
	{
		// Impression Event
		MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;

		var bannerAdSize = bannerSize.ToBannerAdSize();

		switch (Application.platform)
		{
			case RuntimePlatform.Android:
				ad = MediationService.Instance.CreateBannerAd(androidAdUnitId, bannerAdSize, bannerAdAnchor);
				break;
			case RuntimePlatform.IPhonePlayer:
				ad = MediationService.Instance.CreateBannerAd(iosAdUnitId, bannerAdSize, bannerAdAnchor);
				break;
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.LinuxEditor:
				ad = MediationService.Instance.CreateBannerAd(!string.IsNullOrEmpty(androidAdUnitId) ? androidAdUnitId : iosAdUnitId, bannerAdSize, bannerAdAnchor);
				break;
			default:
				Debug.LogWarning("Mediation service is not available for this platform:" + Application.platform);
				return;
		}
		Debug.Log("Initialized On Start! Loading banner Ad...");
		LoadAd();
	}

	public async override void LoadAd()
	{
		try
		{
			await ad.LoadAsync();
			AdLoaded();
		}
		catch (LoadFailedException e)
		{
			AdFailedLoad(e);
		}
	}
}
