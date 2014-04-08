using UnityEngine;
using System.Collections;

public class EnemyGunner : MonoBehaviour
{
	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged.
	public AudioClip[] deathClips;		// An array of audioclips that can play when the enemy dies.
	public GameObject hundredPointsUI;	// A prefab of 100 that appears when the enemy dies.
	public float deathSpinMin = -100f;			// A value to give the minimum amount of Torque when dying
	public float deathSpinMax = 100f;			// A value to give the maximum amount of Torque when dying
	public float deathVolume = 1.0f;
	public int points;

	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private float frontRadius = 0.2f;
	public LayerMask whatIsObstacle;
	private bool blocked = false;
	private bool frontGrounded = false;
	private Animator anim;

	private bool dead = false;			// Whether or not the enemy is dead.
	private PickupSpawner pickupSpawner;	// Reference to the pickup spawner.

	GameObject enemyGunObject;

	void Awake()
	{
		// Setting up the references.
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;

		//Reference to pickup spawner script
		pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();

		enemyGunObject = transform.Find("enemyWeaponHolder/enemyGun").gameObject;
		anim = transform.root.gameObject.GetComponent<Animator> ();
	}

	void FixedUpdate ()
	{

		EnemyGun enemyGun = enemyGunObject.GetComponent<EnemyGun>();
		// Walk
		if (!enemyGun.isFiring) {
			transform.Find("enemyWeaponHolder").gameObject.GetComponent<EnemyAimAssist>().enabled = false;

			// Create an array of all the colliders in front of the enemy.
			Collider2D[] frontHits = Physics2D.OverlapPointAll (frontCheck.position, 1);

			blocked = Physics2D.Linecast (transform.position, transform.Find ("frontCheck").position, 1 << LayerMask.NameToLayer ("Ground"));

			frontGrounded = Physics2D.Linecast (transform.position, transform.Find ("frontGroundCheck").position, 1 << LayerMask.NameToLayer ("Ground"));

			if ((blocked || !frontGrounded) && !dead) {
					// ... Flip the enemy and stop checking the other colliders.
					Flip ();
			}
			// Set the enemy's velocity to moveSpeed in the x direction.
			rigidbody2D.velocity = new Vector2 (transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);

		}
		// Begin shooting
		else{
			transform.Find("enemyWeaponHolder").gameObject.GetComponent<EnemyAimAssist>().enabled = true;
			rigidbody2D.velocity = new Vector2(0,0);
		}
		anim.SetFloat("enemySpeed", Mathf.Abs(rigidbody2D.velocity.x));
		// If the enemy has one hit point left and has a damagedEnemy sprite...
		if (HP == 1 && damagedEnemy != null)
			// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
			ren.sprite = damagedEnemy;

		// If the enemy has zero or fewer hit points and isn't dead yet...
		if (HP <= 0 && !dead){
			// ... call the death function.
			Death ();
			enemyGunObject.GetComponent<EnemyGun>().canFire = false;
		}
	}

	public void Hurt()
	{
		// Reduce the number of hit points by one.
		HP--;
		anim.SetTrigger ("Hurt");
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

		// Increase the score
		PlayerControl.score += points;

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
		AudioSource.PlayClipAtPoint(deathClips[i], transform.position, deathVolume);

		// Create a vector that is just above the enemy.
		Vector3 scorePos;
		scorePos = transform.position;
		scorePos.y += 1.5f;

		// Instantiate the 100 points prefab at this point.
		if(hundredPointsUI) Instantiate(hundredPointsUI, scorePos, Quaternion.identity);

		pickupSpawner.DeliverPickup(transform.position);
		Destroy(gameObject, 0);
	}


	public void Flip()
	{
		transform.Find("enemyWeaponHolder").gameObject.GetComponent<EnemyAimAssist>().Flip();
	}
}
