using UnityEngine;
using System.Xml.Linq;
using System.Collections.Generic;

public class Localization : Singleton<Localization>
{
	public delegate void TranslationsChangedDelegate();
	public static event TranslationsChangedDelegate TranslationsChangedEvent;
	public static void NotifyTranslationsChangeEvent() => TranslationsChangedEvent?.Invoke();

	private static readonly string LANGUAGE_KEY = "lang";
	private static readonly Language DEFAULT_LANGUAGE = Language.EN;
	private static readonly string TRANSLATIONS_PATH = "Lang/";

	public Language SelectedLanguage { get; private set; }
	public List<Language> GetLanguages => new() { Language.EN, Language.ET };

	private List<LocalizationItem> _translations;


	public override void Awake()
	{
		base.Awake();

		if (!PlayerPrefs.HasKey(LANGUAGE_KEY))
		{
			SelectedLanguage = DEFAULT_LANGUAGE;
			PlayerPrefs.SetString(LANGUAGE_KEY, SelectedLanguage.ToString());
		}
		else
		{
			SelectedLanguage = Utils.ParseEnum<Language>(PlayerPrefs.GetString(LANGUAGE_KEY));
		}
		LoadTranslations();
	}

	private void LoadTranslations()
	{
		var textAsset = (TextAsset)Resources.Load(TRANSLATIONS_PATH + SelectedLanguage);
		var languageXmlData = XDocument.Parse(textAsset.text);
		var xElements = languageXmlData.Element("Language").Elements();

		_translations = new List<LocalizationItem>();

		foreach (var xElement in xElements)
		{
			_translations.Add(new LocalizationItem(xElement.Attribute("key").Value, xElement.Value));
		}
		NotifyTranslationsChangeEvent();
	}

	public string GetText(string key)
	{
		var result = _translations.Find(t => t.key.Equals(key));
		return result == null ? "Undefined" : result.value;
	}

	public string GetText(string key, object args) => string.Format(GetText(key), args);

	public void SetLanguage(Language value)
	{
		if (SelectedLanguage.Equals(value))
		{
			return;
		}
		SelectedLanguage = value;
		PlayerPrefs.SetString(LANGUAGE_KEY, SelectedLanguage.ToString());
		LoadTranslations();
		NotifyTranslationsChangeEvent();
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

public enum Language
{
	EN,
	ET
}
