using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.
using TMPro;

public class GameManager : Singleton<GameManager>
{
	private static readonly int PLAYER_INITIAL_FOOD = 100;

	public float turnDelay = 0.1f;                          //Delay between each Player turn.
	public int playerFoodPoints = PLAYER_INITIAL_FOOD;                      //Starting value for Player food points.
	public bool playersTurn = true;       //Boolean to check if it's players turn, hidden in inspector but public.

	private GameUIManager _gameUIManager;
	[SerializeField]
	private TextMeshProUGUI levelText;                                 //Text to display current level number.
																	   // private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.
	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 1;                                  //Current level number, expressed in game as "Day 1".
	private List<Enemy> enemies;                            //List of all Enemy units, used to issue them move commands.
	private bool enemiesMoving;                             //Boolean to check if enemies are moving.
	private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.


	public override void Awake()
	{
		_gameUIManager = FindObjectOfType<GameUIManager>();

		//Assign enemies to a new List of Enemy objects.
		enemies = new List<Enemy>();

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();

		base.Awake();
	}

	private void Start() => InitGame();

	public void Pause() => Time.timeScale = 0;

	public void Resume() => Time.timeScale = 1;

	public void Restart()
	{
		level = 0;
		playerFoodPoints = PLAYER_INITIAL_FOOD;

		Reset();

		LoadNewDay();
	}

	public void Reset()
	{
		enemies.Clear();

		boardScript.Clear();
	}

	//private void UpdateStatsAndSave()
	//{
	//	var saveFile = SaveManager.instance.saveFile;
	//	var highestCore = saveFile.highScore;

	//	saveFile.highScore = highestCore >= playerFoodPoints ? highestCore : playerFoodPoints;

	//	SaveManager.instance.Save();
	//}

	public void LoadNewDay()
	{
		level++;

		_gameUIManager.LoadNextDay(level);
	}

	//Initializes the game for each level.
	public void InitGame()
	{
		//While doingSetup is true the player can't move, prevent player from moving while title card is up.
		doingSetup = true;

		//Set the text of levelText to the string "Day" and append the current level number.
		levelText.text = "Day " + level;

		//Call the SetupScene function of the BoardManager script, pass it current level number.
		boardScript.SetupScene(level);

		doingSetup = false;
	}

	//Update is called every frame.
	void Update()
	{
		//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
		if (playersTurn || enemiesMoving || doingSetup)
		{
			return;
		}

		//Start moving enemies.
		StartCoroutine(MoveEnemies());
	}

	//Call this to add the passed in Enemy to the List of Enemy objects.
	public void AddEnemyToList(Enemy script)
	{
		//Add Enemy to List enemies.
		enemies.Add(script);
	}

	//GameOver is called when the player reaches 0 food points
	public void GameOver()
	{
		//Set levelText to display number of levels passed and game over message
		levelText.text = "After " + level + " days, you starved.";


		//Disable this GameManager.
		//enabled = false;

		//UpdateStatsAndSave();
	}

	//Coroutine to move enemies in sequence.
	IEnumerator MoveEnemies()
	{
		//While enemiesMoving is true player is unable to move.
		enemiesMoving = true;

		//Wait for turnDelay seconds, defaults to .1 (100 ms).
		yield return new WaitForSeconds(turnDelay);

		//If there are no enemies spawned (IE in first level):
		if (enemies.Count == 0)
		{
			//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
			yield return new WaitForSeconds(turnDelay);
		}

		//Loop through List of Enemy objects.
		foreach (var enemy in enemies)
		{
			//Call the MoveEnemy function of Enemy at index i in the enemies List.
			enemy.Move();

			//Wait for Enemy's moveTime before moving next Enemy,
			yield return new WaitForSeconds(enemy.moveTime);
		}
		//Once Enemies are done moving, set playersTurn to true so player can move.
		playersTurn = true;

		//Enemies are done moving, set enemiesMoving to false.
		enemiesMoving = false;
	}
}
