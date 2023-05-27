using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class SwipeDetection : MonoBehaviour
{
	[SerializeField]
	private float _minimumDistance = .2f;
	[SerializeField]
	private float _maximumTime = 1f;
	[SerializeField, Range(0, 1f)]
	private float _directionThreshold = .9f;
	[SerializeField]
	private GameObject _trail;

	private InputManager _inputManager;
	private Coroutine _trailCoroutine;

	private Vector2 _startPosition;
	private float _startTime;

	private Vector2 _endPosition;
	private float _endTime;


	private void Awake() => _inputManager = GetComponent<InputManager>();

	private void OnEnable()
	{
		_inputManager.OnStartTouch += SwipeStart;
		_inputManager.OnEndTouch += SwipeEnd;
	}

	private void OnDisable()
	{
		_inputManager.OnStartTouch -= SwipeStart;
		_inputManager.OnEndTouch -= SwipeEnd;
	}

	private void SwipeStart(Vector2 position, float time)
	{
		_startPosition = position;
		_startTime = time;

		_trail.transform.position = position;

		StartCoroutine(SetTrailActive());

		_trailCoroutine = StartCoroutine(Trail());
	}

	private void SwipeEnd(Vector2 position, float time)
	{
		_endPosition = position;
		_endTime = time;

		_trail.SetActive(false);

		DetectSwipe();

		StopCoroutine(_trailCoroutine);
	}

	private void DetectSwipe()
	{
		if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance
			&& (_endTime - _startTime) <= _maximumTime)
		{
			Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);

			var direction = _endPosition - _startPosition;
			var direction2D = new Vector2(direction.x, direction.y);

			SwipeDirection(direction2D);
		}
	}

	private void SwipeDirection(Vector2 direction)
	{
		var player = FindObjectOfType<Player>();

		if (Vector2.Dot(Vector2.up, direction) > _directionThreshold)
		{
			//Debug.Log("Swipe Up");

			player.Move(0, 1);
		}
		else if (Vector2.Dot(Vector2.down, direction) > _directionThreshold)
		{
			//Debug.Log("Swipe Down");

			player.Move(0, -1);
		}
		else if (Vector2.Dot(Vector2.left, direction) > _directionThreshold)
		{
			//Debug.Log("Swipe Left");

			player.Move(-1, 0);
		}
		else if (Vector2.Dot(Vector2.right, direction) > _directionThreshold)
		{
			//Debug.Log("Swipe Right");

			player.Move(1, 0);
		}
	}

	private IEnumerator Trail()
	{
		while (true)
		{
			_trail.transform.position = _inputManager.PrimaryPosition();

			yield return null;
		}
	}

	private IEnumerator SetTrailActive()
	{
		yield return new WaitForSeconds(.05f);

		_trail.SetActive(true);
	}
}

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}
