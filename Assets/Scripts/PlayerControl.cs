using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump

	// Static variables for player state
	public static int score = 0;			// The current score achieved from previous levels
	public static int runningScore = 0;		// The score that the player could potentially achieve on the current level
	public static int lives = 0;			// The number of lives the player currently has
	public static int defaultLives = 3;		// The default number of lives the player starts with
	public static int level = 0;			// The level the player is currently on

	//Jumping Variables
	public float maxJumpV;					// The maximum upwards velocity
	public float minJumpV;					// The minimum upwards velocity 
	private bool jumpPressed = false;		// Whether or not the jump button has been pressed
	private bool grounded = false;			// Whether or not the player is grounded

	//Movement
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis

	//Audio and Animation
	public AudioClip introSound;			// The clip to play when the player enters the level
	public float introSoundVolume = 1.0f;	// The volume to play the intro sound at
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps
	public float jumpVolume = 1.0f;			// The volume to play the jump sound at
	private Animator anim;					// Reference to the player's animator component
	private bool playJumpAudio = false;		// Whether or not the jump audio is currently playing

	void Awake()
	{
		level = Application.loadedLevel;
		anim = GetComponent<Animator>();
		// Play intro sound
		AudioSource.PlayClipAtPoint(introSound, transform.position, introSoundVolume);
	}
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer
		grounded = Physics2D.Linecast(transform.position, transform.Find("groundCheck").position, 1 << LayerMask.NameToLayer("Ground"))
			|| Physics2D.Linecast(transform.position, transform.Find("groundCheckFront").position, 1 << LayerMask.NameToLayer("Ground"))
			|| Physics2D.Linecast(transform.position, transform.Find("groundCheckBack").position, 1 << LayerMask.NameToLayer("Ground"));  

		float verticalMove = Input.GetAxis("Jump");
		if(verticalMove > 0 && grounded && !jumpPressed){
			anim.SetTrigger("Jump");
			playJumpAudio = true;
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxJumpV);
			jumpPressed = true;
		}

		// Make the user press every time they want to jump
		if (verticalMove == 0){
			jumpPressed = false;
		}
		
		if(verticalMove == 0 && rigidbody2D.velocity.y > minJumpV){
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, minJumpV);
		}
	}


	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(h));
		rigidbody2D.velocity = new Vector2(h * maxSpeed, rigidbody2D.velocity.y);

		// Play a randomly selected jump clip from the array of available clips
		if(playJumpAudio){
			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position, jumpVolume);
			playJumpAudio = false;
		}
	}
}
