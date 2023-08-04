using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
	private static readonly string SAVE_FILE_KEY = "save";

	public static event System.Action OnLoadSuccess;
	public static event System.Action OnSaveSuccess;

	public SaveFile saveFile { get; set; }


	private void OnEnable() => Modal.OnConfirmEvent += Create;

	private void OnDisable() => Modal.OnConfirmEvent -= Create;

	private void Start()
	{
		if (PlayerPrefs.HasKey(SAVE_FILE_KEY)) Load();
		else Create();
	}

	private void Create() => Create(true);

	public void Create(bool notify = false)
	{
		saveFile = new SaveFile(highScore: 0, achievements: new List<AchievementData>());
		Save(false);
		Load(notify);
	}

	public void Save(bool notify = true)
	{
		PlayerPrefs.SetString(SAVE_FILE_KEY, Helpers.ToJson(saveFile));

		if (notify) OnSaveSuccess();
	}

	public void Load(bool notify = true)
	{
		saveFile = Helpers.FromJson<SaveFile>(PlayerPrefs.GetString(SAVE_FILE_KEY));

		if (notify) OnLoadSuccess();
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
