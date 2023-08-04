using TMPro;
using UnityEngine;

public class Achievement : MonoBehaviour
{
	[SerializeField]
	private int _id;
	[SerializeField]
	private string _nameLocalizationKey;
	[SerializeField]
	private string _descriptionLocalizationKey;
	[Space(10)]
	[SerializeField]
	private TextMeshProUGUI _name;
	[SerializeField]
	private TextMeshProUGUI _description;
	[Space(10)]
	[SerializeField]
	private CanvasGroup _canvasGroup;
	[SerializeField]
	private GameObject _isUnlockedIndicator;


	public void Setup(bool isUnlocked)
	{
		_name.text = LocalizationManager.Instance.GetLocalizedValue(_nameLocalizationKey);
		_description.text = LocalizationManager.Instance.GetLocalizedValue(_descriptionLocalizationKey);

		_canvasGroup.alpha = isUnlocked ? .5f : 1f;
		_isUnlockedIndicator.SetActive(isUnlocked);
	}

	public int GetId() => _id;
}
