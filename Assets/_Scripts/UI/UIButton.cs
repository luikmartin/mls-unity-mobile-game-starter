using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : Button
{
	private TextMeshProUGUI _label;

	private new void Awake()
	{
		base.Awake();

		_label = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (_label == null) return;

		_label.color = currentSelectionState == SelectionState.Pressed
			? colors.pressedColor
			: colors.normalColor;
	}
}
