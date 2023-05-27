﻿using UnityEngine;
using TMPro;
using Cinemachine;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
	public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
	public int pointsPerFood = 10;              //Number of points to add to player food points when picking up a food object.
	public int pointsPerSoda = 20;              //Number of points to add to player food points when picking up a soda object.
	public int wallDamage = 1;                  //How much damage a player does to a wall when chopping it.
	public TextMeshProUGUI foodText;                        //UI Text to display current player food total.
	public AudioClip moveSound1;                //1 of 2 Audio clips to play when player moves.
	public AudioClip moveSound2;                //2 of 2 Audio clips to play when player moves.
	public AudioClip eatSound1;                 //1 of 2 Audio clips to play when player collects a food object.
	public AudioClip eatSound2;                 //2 of 2 Audio clips to play when player collects a food object.
	public AudioClip drinkSound1;               //1 of 2 Audio clips to play when player collects a soda object.
	public AudioClip drinkSound2;               //2 of 2 Audio clips to play when player collects a soda object.
	public AudioClip gameOverSound;             //Audio clip to play when player dies.

	private Animator animator;                  //Used to store a reference to the Player's animator component.
	private int food;                           //Used to store player food points total during level.
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
	private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
#endif
	public bool canMove = true;


	private void Awake()
	{
		foodText = GameObject.Find("Game Score").GetComponent<TextMeshProUGUI>();
	}

	//Start overrides the Start function of MovingObject
	protected override void Start()
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

		//Get the current food point total stored in GameManager.instance between levels.
		food = GameManager.Instance.playerFoodPoints;

		//Set the foodText to reflect the current player food total.
		foodText.text = "Food: " + food;

		var cinemachineVirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
		cinemachineVirtualCamera.Follow = transform;

		//Call the Start function of the MovingObject base class.
		base.Start();
	}

	public void Move(int xDir, int yDir)
	{
		if (GameManager.Instance.playersTurn && canMove)
		{
			AttemptMove<Wall>(xDir, yDir);
		}
	}

	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		//Every time player moves, subtract from food points total.
		food--;

		//Update food text display to reflect current score.
		foodText.text = "Food: " + food;

		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove<T>(xDir, yDir);

		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;

		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move(xDir, yDir, out hit))
		{
			//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
			SoundManager.Instance.RandomizeSfx(moveSound1, moveSound2);
		}

		//Since the player has moved and lost food points, check if the game has ended.
		CheckIfGameOver();

		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		GameManager.Instance.playersTurn = false;
	}

	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove<T>(T component)
	{
		//Set hitWall to equal the component passed in as a parameter.
		Wall hitWall = component as Wall;

		//Call the DamageWall function of the Wall we are hitting.
		hitWall.DamageWall(wallDamage);

		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		animator.SetTrigger("playerChop");
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D(Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if (other.CompareTag("Exit"))
		{
			canMove = false;

			GameManager.Instance.playerFoodPoints = food;

			GameManager.Instance.LoadNewDay();
		}

		//Check if the tag of the trigger collided with is Food.
		else if (other.CompareTag("Food"))
		{
			//Add pointsPerFood to the players current food total.
			food += pointsPerFood;

			//Update foodText to represent current total and notify player that they gained points
			foodText.text = "+" + pointsPerFood + " Food: " + food;

			//Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
			SoundManager.Instance.RandomizeSfx(eatSound1, eatSound2);

			//Disable the food object the player collided with.
			other.gameObject.SetActive(false);
		}

		//Check if the tag of the trigger collided with is Soda.
		else if (other.CompareTag("Soda"))
		{
			//Add pointsPerSoda to players food points total
			food += pointsPerSoda;

			//Update foodText to represent current total and notify player that they gained points
			foodText.text = "+" + pointsPerSoda + " Food: " + food;

			//Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
			SoundManager.Instance.RandomizeSfx(drinkSound1, drinkSound2);

			//Disable the soda object the player collided with.
			other.gameObject.SetActive(false);
		}
	}

	//LoseFood is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public void LoseFood(int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger("playerHit");

		//Subtract lost food points from the players total.
		food -= loss;

		//Update the food display with the new total.
		foodText.text = "-" + loss + " Food: " + food;

		//Check to see if game has ended.
		CheckIfGameOver();
	}

	//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
	private void CheckIfGameOver()
	{
		//Check if food point total is less than or equal to zero.
		if (food <= 0)
		{
			//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
			SoundManager.Instance.PlaySingle(gameOverSound);

			//Stop the background music.
			SoundManager.Instance.musicSource.Stop();

			//Call the GameOver function of GameManager.
			GameManager.Instance.GameOver();
		}
	}
}
