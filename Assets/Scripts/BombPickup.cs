using UnityEngine;
using System.Collections;

public class BombPickup : MonoBehaviour
{
	public AudioClip pickupClip;		// Sound for when the bomb crate is picked up.
	public float volume;

	private Animator anim;				// Reference to the animator component.
	private bool landed = false;		// Whether or not the crate has landed yet.
	public int gunNum;

	void Awake()
	{
		// Setting up the reference.
		anim = transform.root.GetComponent<Animator>();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			// ... play the pickup sound effect.
			AudioSource.PlayClipAtPoint(pickupClip, transform.position, volume);

			Gun gun = other.GetComponentInChildren<Gun>();
			gun.gunType = gunNum;
			// Destroy the crate.
			Destroy(transform.root.gameObject);
		}
	}
}
