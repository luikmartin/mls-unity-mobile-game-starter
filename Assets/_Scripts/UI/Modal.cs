using TMPro;
using UnityEngine;

public class Modal : Singleton<Modal>
{
	public delegate void OnCloseDelegate();
	public static event OnCloseDelegate OnCloseEvent;
	public static void OnClose() => OnCloseEvent?.Invoke();

	public delegate void OnConfirmDelegate();
	public static event OnConfirmDelegate OnConfirmEvent;
	public static void OnConfirm() => OnConfirmEvent?.Invoke();

	public delegate void OnCancelDelegate();
	public static event OnCancelDelegate OnCancelEvent;
	public static void OnCancel() => OnCancelEvent?.Invoke();


	[SerializeField]
	private GameObject _modal;

	[Space(10)]
	[SerializeField]
	private GameObject _background;

	[Space(10)]
	[SerializeField]
	private TextMeshProUGUI _title;
	[SerializeField]
	private TextMeshProUGUI _message;

	[Space(10)]
	[SerializeField]
	private TextMeshProUGUI _confirmButtonLabel;
	[SerializeField]
	private TextMeshProUGUI _cancelButtonLabel;

	[Space(10)]
	[SerializeField]
	private GameObject _closeButton;

	public void Open(ModalConfig config)
	{
		_modal.SetActive(true);

		_title.text = config.title;
		_message.text = config.message;

		_confirmButtonLabel.transform.parent.gameObject.SetActive(config.confirmButtonLabel != null);
		_confirmButtonLabel.text = config.confirmButtonLabel;

		_cancelButtonLabel.transform.parent.gameObject.SetActive(config.cancelButtonLabel != null);
		_cancelButtonLabel.text = config.cancelButtonLabel;

		_closeButton.SetActive(config.isCloseButtonVisible);
		_background.SetActive(config.isBackgroundVisible);
	}

	public void Close()
	{
		_modal.SetActive(false);

		OnClose();
	}

	public void Confirm()
	{
		_modal.SetActive(false);

		OnConfirm();
	}

	public void Cancel()
	{
		_modal.SetActive(false);

		OnCancel();
	}
}

public class ModalConfig
{
	public string title;
	public string message;
	public string confirmButtonLabel;
	public string cancelButtonLabel;
	public bool isCloseButtonVisible;
	public bool isBackgroundVisible;

	public ModalConfig()
	{
		isCloseButtonVisible = true;
		isBackgroundVisible = true;
	}
}
