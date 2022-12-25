using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    private string _key;

    private void OnEnable()
    {
        Localization.TranslationsChangedEvent += LoadTranslation;

        if (Localization.Instance != null)
        {
            LoadTranslation();
        }
    }

    private void OnDisable() => Localization.TranslationsChangedEvent -= LoadTranslation;

    private void Start() => LoadTranslation();

    private void LoadTranslation() => GetComponent<TextMeshProUGUI>().SetText(Localization.Instance.GetText(_key).Replace("\\n", "\n"));
}
