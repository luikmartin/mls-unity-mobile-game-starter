using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class LanguageSelectionDropdownBehaviour : MonoBehaviour
{
    private void OnEnable() => GetComponent<TMP_Dropdown>().value = (int)Localization.Instance.SelectedLanguage;
}
