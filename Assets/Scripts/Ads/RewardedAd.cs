using UnityEngine;
using Unity.Services.Mediation;

public class RewardedAd : AdBase<IRewardedAd, IRewardedAd>
{
	public async void ShowRewarded()
	{
		if (ad?.AdState == AdState.Loaded)
		{
			try
			{
				var showOptions = new RewardedAdShowOptions { AutoReload = true };
				await ad.ShowAsync(showOptions);
				Debug.Log("Rewarded Shown!");
			}
			catch (ShowFailedException e)
			{
				Debug.LogWarning($"Rewarded failed to show: {e.Message}");
			}
		}
	}

	public async void ShowRewardedWithOptions()
	{
		if (ad?.AdState == AdState.Loaded)
		{
			try
			{
				//Here we provide a user id and custom data for server to server validation.
				RewardedAdShowOptions showOptions = new RewardedAdShowOptions();
				showOptions.AutoReload = true;
				S2SRedeemData s2SData;
				s2SData.UserId = "my cool user id";
				s2SData.CustomData = "{\"reward\":\"Gems\",\"amount\":20}";
				showOptions.S2SData = s2SData;

				await ad.ShowAsync(showOptions);
				Debug.Log("Rewarded Shown!");
			}
			catch (ShowFailedException e)
			{
				Debug.LogWarning($"Rewarded failed to show: {e.Message}");
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
				ad = MediationService.Instance.CreateRewardedAd(androidAdUnitId);
				break;

			case RuntimePlatform.IPhonePlayer:
				ad = MediationService.Instance.CreateRewardedAd(iosAdUnitId);
				break;
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.LinuxEditor:
				ad = MediationService.Instance.CreateRewardedAd(!string.IsNullOrEmpty(androidAdUnitId) ? androidAdUnitId : iosAdUnitId);
				break;
			default:
				Debug.LogWarning("Mediation service is not available for this platform:" + Application.platform);
				return;
		}

		// Load Events
		ad.OnLoaded += AdLoaded;
		ad.OnFailedLoad += AdFailedLoad;

		// Show Events
		ad.OnUserRewarded += UserRewarded;
		ad.OnClosed += AdClosed;

		Debug.Log($"Initialized On Start. Loading Ad...");
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

	void UserRewarded(object sender, RewardEventArgs e)
	{
		Debug.Log($"User Rewarded! Type: {e.Type} Amount: {e.Amount}");
	}
}
