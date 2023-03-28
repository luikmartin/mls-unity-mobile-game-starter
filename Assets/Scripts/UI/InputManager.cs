using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
	#region Events
	public delegate void StartTouch(Vector2 position, float time);
	public event StartTouch OnStartTouch;

	public delegate void EndTouch(Vector2 position, float time);
	public event EndTouch OnEndTouch;
	#endregion

	private PlayerControls _playerControls;
	private Camera _mainCamera;


	public override void Awake()
	{
		base.Awake();

		_playerControls = new PlayerControls();
		_mainCamera = Camera.main;
	}

	private void OnEnable() => _playerControls.Enable();

	private void OnDisable() => _playerControls.Disable();

	private void Start()
	{
		_playerControls.Touch.PrimaryContact.started += context => StartTouchPrimary(context);
		_playerControls.Touch.PrimaryContact.canceled += context => EndTouchPrimary(context);
	}

	private void StartTouchPrimary(InputAction.CallbackContext context) =>
		OnStartTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);

	private void EndTouchPrimary(InputAction.CallbackContext context) =>
		OnEndTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);

	public Vector2 PrimaryPosition() => Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
}
