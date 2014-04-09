using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	public GameObject[] pickups;				// Array of pickup prefabs with the health pickup first and then weapons
	public float highHealthThreshold = 75f;		// If the player's health is above this, only generate weapon crates
	public float lowHealthThreshold = 25f;		// If the player's health is below this, only generate health crates
	public float dropRate = .2f;				// The probability that the pickup spawner will drop something

	private PlayerHealth playerHealth;

	void Awake ()
	{
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	public void DeliverPickup(Vector3 position)
	{	
		float random = Random.Range(0f, 1f);
		if(random <= dropRate){
			// If the player's health is above the high health threshold, then create a weapon
			if(playerHealth.health >= highHealthThreshold)
				Instantiate(pickups[Random.Range(1,4)], position, Quaternion.identity);
			// Otherwise, if it's below the low health threshold, create a health pickup
			else if(playerHealth.health <= lowHealthThreshold)
				Instantiate(pickups[0], position, Quaternion.identity);
			// Otherwise, if it's within this range, create either a health or weapon pickup
			else
			{
				int pickupIndex = Random.Range(0, pickups.Length);
				Instantiate(pickups[pickupIndex], position, Quaternion.identity);
			}
		}
	}
}
