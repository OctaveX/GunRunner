using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public AudioClip introSound;
	public float introSoundVolume = 1.0f;
	public static int score = 0;
	public static int runningScore = 0;
	public static int lives = 0;
	public static int defaultLives = 3;
	public static int level = 0;

	//public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	//Jumping Variables
	public float maxJumpV;
	public float minJumpV;
	private bool jumpPressed = false;
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.

	//Movement
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	//Audio and Taunting(maybe removed)
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpVolume = 1.0f;
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.

	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Animator anim;					// Reference to the player's animator component.

	bool playJumpAudio = false;

	void Awake()
	{
		level = Application.loadedLevel;

		// Setting up references.
		groundCheck = transform.Find("groundCheckFront");
		anim = GetComponent<Animator>();

		AudioSource.PlayClipAtPoint(introSound, transform.position, introSoundVolume);
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		//CircleCollider2D bottom = CircleCollider2D.GetComponent();

		grounded = Physics2D.Linecast(transform.position, transform.Find("groundCheck").position, 1 << LayerMask.NameToLayer("Ground"))
			|| Physics2D.Linecast(transform.position, transform.Find("groundCheckFront").position, 1 << LayerMask.NameToLayer("Ground"))
				|| Physics2D.Linecast(transform.position, transform.Find("groundCheckBack").position, 1 << LayerMask.NameToLayer("Ground"))
				|| Physics2D.Linecast(transform.position, transform.Find("wallCheckFront").position, 1 << LayerMask.NameToLayer("Ground"))
				|| Physics2D.Linecast(transform.position, transform.Find("wallCheckBack").position, 1 << LayerMask.NameToLayer("Ground"));  

		float verticalMove = Input.GetAxis("Jump");
		if(verticalMove > 0 && grounded && !jumpPressed){
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			
			playJumpAudio = true;

			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxJumpV);
			//grounded = false;
			jumpPressed = true;
		}

		//Makes user press every time they want to jump
		if (verticalMove == 0){
			jumpPressed = false;
		}
		
		if(verticalMove == 0 && rigidbody2D.velocity.y > minJumpV){
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, minJumpV);
		}
	}


	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		rigidbody2D.velocity = new Vector2(h * maxSpeed, rigidbody2D.velocity.y);

		if(playJumpAudio){
			//Play a random jump audio clip.
			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position, jumpVolume);
			playJumpAudio = false;
		}
	}

	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!audio.isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				audio.clip = taunts[tauntIndex];
				audio.Play();
			}
		}
	}


	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}
}
