using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class LanguageSelectionDropdown : MonoBehaviour
{
	private void OnEnable() => GetComponent<TMP_Dropdown>().value = LocalizationManager.GetLocalizationKey(LocalizationManager.Instance.currentLanguage);
}
