using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
	public float healthBonus;	// The health boost given to the player upon picking up the crate
	public AudioClip collect;	// The audio clip to play when picking up the crate

	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone, then give them the health boost and remove the crate object
		if(other.tag == "Player")
		{
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
			playerHealth.health += healthBonus;
			playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);
			playerHealth.UpdateHealthBar();
			AudioSource.PlayClipAtPoint(collect,transform.position);
			Destroy(transform.root.gameObject);
		}
	}
}
