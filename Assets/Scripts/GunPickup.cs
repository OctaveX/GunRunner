using UnityEngine;
using System.Collections;

public class GunPickup : MonoBehaviour
{
	public AudioClip pickupClip;	// Audio clip to play when a gun is picked up
	public float volume;			// The volume to play that audio clip at
	public int gunNum;				// The gun number for the weapon picked up
	
	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone then play the sound effect and give them the new weapon
		if(other.tag == "Player")
		{
			AudioSource.PlayClipAtPoint(pickupClip, transform.position, volume);
			Gun gun = other.GetComponentInChildren<Gun>();
			gun.gunType = gunNum;
			Destroy(transform.root.gameObject);
		}
	}
}
