using UnityEngine;
using System.Collections;

public class EnemyFlyer : MonoBehaviour {
	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged.
	public AudioClip[] deathClips;		// An array of audioclips that can play when the enemy dies.
	public GameObject hundredPointsUI;	// A prefab of 100 that appears when the enemy dies.
	public float deathSpinMin = -100f;			// A value to give the minimum amount of Torque when dying
	public float deathSpinMax = 100f;			// A value to give the maximum amount of Torque when dying
	
	public Rigidbody2D rocket;				// Prefab of the rocket.
	private float lastFireTime = 0f;
	public float fireRate = 1f;
	private bool canFire = true;

	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	float frontRadius = 0.2f;
	public LayerMask whatIsObstacle;
	private bool blocked = false;
	private bool frontGrounded = false;
	
	
	private bool dead = false;			// Whether or not the enemy is dead.
	private Score score;				// Reference to the Score script.
	
	
	void Awake()
	{
		// Setting up the references.
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
		//score = GameObject.Find("Score").GetComponent<Score>();
	}

	void Update(){
		if(Time.time > lastFireTime + fireRate) {
			canFire = true;
		}
		if(canFire){
			Fire();
		}
	}

	void FixedUpdate ()
	{
		// Create an array of all the colliders in front of the enemy.
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);
		
		blocked = Physics2D.Linecast(transform.position, transform.Find("frontCheck").position, 1 << LayerMask.NameToLayer("Ground"));

		if(blocked){
			// ... Flip the enemy and stop checking the other colliders.
			Flip ();
		}
		// Set the enemy's velocity to moveSpeed in the x direction.
		rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, 0f);	
		
		// If the enemy has one hit point left and has a damagedEnemy sprite...
		if(HP == 1 && damagedEnemy != null)
			// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
			ren.sprite = damagedEnemy;
		
		// If the enemy has zero or fewer hit points and isn't dead yet...
		if(HP <= 0 && !dead)
			// ... call the death function.
			Death ();
	}

	void Fire(){
		//yield return new WaitForSeconds(5);
		Rigidbody2D bulletInstance;
		Vector3 temp = new Vector3(0, -5f);
		
		Vector3 launchPoint = new Vector3(transform.position.x, transform.position.y - 3);
		
		bulletInstance = Instantiate(rocket, launchPoint, transform.rotation) as Rigidbody2D;
		bulletInstance.velocity = transform.rotation * temp;
		
		lastFireTime = Time.time;
		canFire = false;
	}

	public void Hurt()
	{
		// Reduce the number of hit points by one.
		HP--;
	}
	
	void Death()
	{
		// Find all of the sprite renderers on this object and it's children.
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();
		
		// Disable all of them sprite renderers.
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}
		
		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
		ren.enabled = true;
		ren.sprite = deadEnemy;
		
		// Increase the score by 100 points
		//score.score += 100;
		
		// Set dead to true.
		dead = true;
		
		// Allow the enemy to rotate and spin it by adding a torque.
		rigidbody2D.fixedAngle = false;
		rigidbody2D.AddTorque(Random.Range(deathSpinMin,deathSpinMax));
		
		// Find all of the colliders on the gameobject and set them all to be triggers.
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}
		
		// Play a random audioclip from the deathClips array.
		int i = Random.Range(0, deathClips.Length);
		//AudioSource.PlayClipAtPoint(deathClips[i], transform.position);
		
		// Create a vector that is just above the enemy.
		Vector3 scorePos;
		scorePos = transform.position;
		scorePos.y += 1.5f;
		
		// Instantiate the 100 points prefab at this point.
		Instantiate(hundredPointsUI, scorePos, Quaternion.identity);

		Destroy(gameObject);
	}
	
	
	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}

}
