using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
	[SerializeField]
	private string _key;

	private void OnEnable()
	{
		LocalizationManager.OnLocalizationChange += LoadTranslation;

		if (LocalizationManager.Instance != null) LoadTranslation();
	}

	private void OnDisable() => LocalizationManager.OnLocalizationChange -= LoadTranslation;

	private void Start() => LoadTranslation();

	private void LoadTranslation() => GetComponent<TextMeshProUGUI>().SetText(LocalizationManager.Instance.GetLocalizedValue(_key).Replace("\\n", "\n"));
}
