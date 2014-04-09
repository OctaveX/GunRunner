using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float moveSpeed = 2f;		// Enemy speed
	public int HP = 2;					// The enemy's Hit Point count
	public AudioClip[] deathClips;		// An array of audioclips that can play when the enemy dies
	public float deathVolume = 1.0f;	// The volume to play the death clip at
	public int points;					// The number of points to award the player after defeating the enemy

	protected Animator anim;			// Reference to the enemy's animator
	protected bool blocked = false;		// A flag indicating whether or not the enemy has run into a wall or not	
	protected bool frontGrounded = false;// A flag indicating whether or not the eneny is about to walk off a platform
	protected bool dead = false;		// Whether or not the enemy is dead
	protected PickupSpawner pickupSpawner;// Reference to the pickup spawner
	
	protected void Awake()
	{
		pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
		anim = transform.root.gameObject.GetComponent<Animator> ();
	}

	void FixedUpdate ()
	{
		blocked = Physics2D.Linecast(transform.position, transform.Find("frontCheck").position, 1 << LayerMask.NameToLayer("Ground"));
		frontGrounded = Physics2D.Linecast(transform.position, transform.Find("frontGroundCheck").position, 1 << LayerMask.NameToLayer("Ground"));

		if((blocked || !frontGrounded)&& !dead)
				Flip ();

		// Set the enemy's velocity
		rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);	
		anim.SetFloat("enemySpeed", Mathf.Abs(rigidbody2D.velocity.x));
			
		// Kill the enemy if their HP has reached zero or below
		if(HP <= 0 && !dead)
			Death ();
	}
	
	public void Hurt()
	{
		HP--;
		anim.SetTrigger ("Hurt");
	}
	
	protected void Death()
	{
		dead = true;

		// Increase the player's running score for this level
		PlayerControl.runningScore += points;

		// Find all of the colliders on the gameobject and set them all to be triggers
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}

		// Play a clip from the list of possible death clips, spawn a pickup, and destroy the enemy object
		int i = Random.Range(0, deathClips.Length);
		AudioSource.PlayClipAtPoint(deathClips[i], transform.position, deathVolume);
		pickupSpawner.DeliverPickup(transform.position);
		Destroy(gameObject, 0);
	}


	public void Flip()
	{
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
