using UnityEngine;

public class AchievementsManager : Singleton<AchievementsManager>
{
	[SerializeField]
	private GameObject _achievementListItemsContainer;


	private void OnEnable()
	{
		SaveManager.OnLoadSuccess += UpdateAchievements;
		LocalizationManager.OnLocalizationChange += UpdateAchievements;
	}

	private void OnDisable()
	{
		SaveManager.OnLoadSuccess -= UpdateAchievements;
		LocalizationManager.OnLocalizationChange -= UpdateAchievements;
	}

	private void UpdateAchievements()
	{
		var achievementsData = SaveManager.Instance.saveFile.achievements;

		foreach (var achievement in _achievementListItemsContainer.GetComponentsInChildren<Achievement>())
		{
			var achievementData = achievementsData.Find(a => a.id.Equals(achievement.GetId()));
			achievement.Setup(achievementData != null && achievementData.isUnlocked);
		}
	}
}
