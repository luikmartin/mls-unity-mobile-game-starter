using UnityEngine;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

public class LocalizationManager : Singleton<LocalizationManager>
{
	public static event System.Action OnLocalizationChange;

	public static string playerPrefsLocalizationKey = "localization";
	public static string localizaionsPath = "Localizations";
	public static string xmlLocalizationRootTag = "Localization";
	public static int defaultLanguageKey = 1;
	public static string defaultLanguageValue = "en";

	public string currentLanguage { get; private set; }

	[SerializeField] private static Dictionary<int, string> _supportedLocalization = new Dictionary<int, string>()
	{
		{ defaultLanguageKey, defaultLanguageValue }
	};

	[SerializeField] private Dictionary<string, string> _localizedText = new Dictionary<string, string>()
	{
		{ "en_key", "en_value" }
	};


	public override void Awake()
	{
		base.Awake();

		currentLanguage = defaultLanguageValue;

		if (!PlayerPrefs.HasKey(playerPrefsLocalizationKey))
			PlayerPrefs.SetString(playerPrefsLocalizationKey, currentLanguage);
		else
			currentLanguage = PlayerPrefs.GetString(playerPrefsLocalizationKey);
	}

	private void Start()
	{
		LoadLocalizedText();
	}

	private void LoadLocalizedText()
	{
		var textAsset = (TextAsset)Resources.Load(localizaionsPath + "/" + currentLanguage);
		var languageXmlData = XDocument.Parse(textAsset.text);
		var xElements = languageXmlData.Element(xmlLocalizationRootTag).Elements();

		_localizedText = new Dictionary<string, string>();

		foreach (var xElement in xElements)
			_localizedText.Add(xElement.Attribute("key").Value, xElement.Value);

		OnLocalizationChange?.Invoke();
	}

	public string GetLocalizedValue(string key)
	{
		_localizedText.TryGetValue(key, out var value);
		return value;
	}

	public string GetLocalizedValue(string key, object args) => string.Format(GetLocalizedValue(key), args);

	public void SetLanguage(string value)
	{
		if (currentLanguage.Equals(value)) return;
		currentLanguage = value;
		PlayerPrefs.SetString(playerPrefsLocalizationKey, currentLanguage);
		LoadLocalizedText();
		OnLocalizationChange?.Invoke();
	}

	public static int GetLocalizationKey(string value)
	{
		var key = _supportedLocalization.FirstOrDefault(x => x.Value == value).Key;
		return key != 0 ? key : defaultLanguageKey;
	}

	public static string GetLocalizationValue(int key)
	{
		var value = defaultLanguageValue;
		_supportedLocalization.TryGetValue(key, out value);
		return value;
	}

	[System.Serializable]
	private class LocalizationItem
	{
		public string key;
		public string value;

		public LocalizationItem(string key, string value)
		{
			this.key = key;
			this.value = value;
		}
	}
}
