using System.Collections.Generic;
using UnityEngine;

public class Saves : Singleton<Saves>
{
	private static readonly string SAVE_FILE_KEY = "save";

	public delegate void SaveFileSavedDelegate();
	public static event SaveFileSavedDelegate SaveFileSavedEvent;
	public static void OnSaveSuccess() => SaveFileSavedEvent?.Invoke();

	public delegate void SaveFileLoadedDelegate();
	public static event SaveFileLoadedDelegate SaveFileLoadedEvent;
	public static void OnLoadSuccess() => SaveFileLoadedEvent?.Invoke();

	public SaveFile saveFile { get; set; }


	public override void Awake()
	{
		base.Awake();

		if (PlayerPrefs.HasKey(SAVE_FILE_KEY))
		{
			Load();
		}
		else
		{
			Create();
		}
	}

	public void Create()
	{
		saveFile = new SaveFile(highScore: 0, achievements: new List<AchievementData>());

		Save(false);

		Load();
	}

	public void Save(bool notify = true)
	{
		PlayerPrefs.SetString(SAVE_FILE_KEY, Utils.ToJson(saveFile));

		if (notify)
		{
			OnSaveSuccess();
		}
	}

	public void Load(bool notify = true)
	{
		saveFile = Utils.FromJson<SaveFile>(PlayerPrefs.GetString(SAVE_FILE_KEY));

		if (notify)
		{
			OnLoadSuccess();
		}
	}
}

[System.Serializable]
public class SaveFile
{
	public int highScore;
	public List<AchievementData> achievements;

	public SaveFile(int highScore, List<AchievementData> achievements)
	{
		this.highScore = highScore;
		this.achievements = achievements;
	}
}

[System.Serializable]
public class AchievementData
{
	public int id;
	public bool isUnlocked;

	public AchievementData(int id, bool isUnlocked)
	{
		this.id = id;
		this.isUnlocked = isUnlocked;
	}
}
