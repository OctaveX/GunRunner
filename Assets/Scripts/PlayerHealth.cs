using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;					// The player's health
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged
	public float ouchClipVolume = 1.0f;			// The volume at which to play the hurt clip
	public AudioClip[] deathClips;				// Array of clips to play when the player dies
	public float deathClipVolume = 1.0f;		// The volume at which to play the death clip
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player

	private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar
	private float lastHitTime;					// The time at which the player was last hit
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health)
	private PlayerControl playerControl;		// Reference to the PlayerControl script
	private Animator anim;						// Reference to the Animator on the playe
	private GameObject gunObject;				// Reference to the player's gun object

	void Awake ()
	{
		playerControl = GetComponent<PlayerControl>();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		gunObject = GameObject.Find("Gun");
		// Getting the intial scale of the healthbar
		healthScale = healthBar.transform.localScale;
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "EnemyBullet"){
			// If there has been enough time since a previous hit, and the player still has health, then take damage
			if (Time.time > lastHitTime + repeatDamagePeriod) 
			{
				AttackPlayer(col.transform);
			}
		}
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		// If there has been enough time since a previous hit, and the player still has health, then take damage
		if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyFlyer" || col.gameObject.tag == "EnemyGunner" || col.gameObject.tag == "Spike")
		{
			AttackPlayer(col.transform);
		}
	}

	void AttackPlayer(Transform colTransform)
	{
		if (Time.time > lastHitTime + repeatDamagePeriod) 
		{
			if(health > 0f)
			{
				TakeDamage(colTransform); 
				lastHitTime = Time.time;
				gunObject.GetComponent<Gun>().gunType = 0;
			}
			// If the player has no health, let them drop off the stage
			else
			{
				// Disable all physics on colliders
				Collider2D[] cols = GetComponents<Collider2D>();
				foreach(Collider2D c in cols)
				{
					c.isTrigger = true;
				}
				
				// Move the player's sprites to the front
				SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
				foreach(SpriteRenderer s in spr)
				{
					s.sortingLayerName = "UI";
				}
				
				// Disable player movement and shooting
				GetComponent<PlayerControl>().enabled = false;
				GetComponentInChildren<Gun>().enabled = false;
				StartCoroutine(Die());
			}
		}
	}


	void TakeDamage (Transform enemy)
	{
		// Animate the player being hurt, play the hurt clip and remove HP
		playerControl.jump = false;

		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
		rigidbody2D.AddForce(hurtVector * hurtForce);

		health -= damageAmount;
		UpdateHealthBar();

		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position, ouchClipVolume);
		anim.SetTrigger ("Hurt");
	}

	IEnumerator Die ()
	{
		AudioSource.PlayClipAtPoint(deathClips[0], transform.position, deathClipVolume);
		anim.SetTrigger("Die");
		PlayerControl.runningScore = 0;

		yield return new WaitForSeconds (3);
		PlayerControl.lives--;
		if (PlayerControl.lives >= 0 ) Application.LoadLevel(Application.loadedLevel);
		else
		{
			PlayerControl.level--;
			Application.LoadLevel ("GameOver");
		}
	}

	public void UpdateHealthBar ()
	{
		// Set the health bar's colour based on the HP value and change the scale of the health bar object
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}
}
