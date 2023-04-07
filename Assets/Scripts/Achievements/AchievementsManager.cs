using UnityEngine;

public class AchievementsManager : Singleton<AchievementsManager>
{
	[SerializeField]
	private GameObject _achievementListItemsContainer;


	private void OnEnable()
	{
		Saves.SaveFileLoadedEvent += UpdateAchievements;
		Localization.TranslationsChangedEvent += UpdateAchievements;
	}

	private void OnDisable()
	{
		Saves.SaveFileLoadedEvent -= UpdateAchievements;
		Localization.TranslationsChangedEvent -= UpdateAchievements;
	}

	private void UpdateAchievements()
	{
		var achievementsData = Saves.Instance.saveFile.achievements;

		foreach (var achievement in _achievementListItemsContainer.GetComponentsInChildren<Achievement>())
		{
			var achievementData = achievementsData.Find(a => a.id.Equals(achievement.GetId()));

			achievement.Setup(achievementData != null && achievementData.isUnlocked);
		}
	}
}
