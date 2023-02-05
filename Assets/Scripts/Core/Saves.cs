using UnityEngine;

public class Saves : Singleton<Saves>
{
	private static readonly string SAVE_FILE_KEY = "save";

	public delegate void SaveFileSavedDelegate();
	public static event SaveFileSavedDelegate SaveFileSavedEvent;
	public static void NotifySaveFileSavedEvent() => SaveFileSavedEvent?.Invoke();

	public delegate void SaveFileLoadedDelegate();
	public static event SaveFileLoadedDelegate SaveFileLoadedEvent;
	public static void NotifySaveFileLoadedEvent() => SaveFileLoadedEvent?.Invoke();

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
		saveFile = new SaveFile(highScore: 0);

		Save(false);

		Load();
	}

	public void Save(bool notify = true)
	{
		PlayerPrefs.SetString(SAVE_FILE_KEY, Utils.ToJson(saveFile));

		if (notify)
		{
			NotifySaveFileSavedEvent();
		}
	}

	public void Load(bool notify = true)
	{
		saveFile = Utils.FromJson<SaveFile>(PlayerPrefs.GetString(SAVE_FILE_KEY));

		if (notify)
		{
			NotifySaveFileLoadedEvent();
		}
	}
}

[System.Serializable]
public class SaveFile
{
	public int highScore;

	public SaveFile(int highScore)
	{
		this.highScore = highScore;
	}
}
