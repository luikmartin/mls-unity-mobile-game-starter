using UnityEngine;

public class Saves : Singleton<Saves>
{
    private static readonly string SAVE_KEY = "save";

    public SaveFile saveFile { get; set; }

    public override void Awake()
    {
        base.Awake();

        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            PlayerPrefs.SetString(SAVE_KEY, Utils.ToJson(Create()));
        }
        Load();
    }

    public SaveFile Create() => new SaveFile(highScore: 0);

    public void Save() => PlayerPrefs.SetString(SAVE_KEY, Utils.ToJson(saveFile));

    public void Load() => saveFile = Utils.FromJson<SaveFile>(PlayerPrefs.GetString(SAVE_KEY));
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