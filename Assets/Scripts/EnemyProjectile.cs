using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
	public GameObject explosion;		// Optional prefab for the explosion effect
	public float bulletRange = 1.2f;	// The range of the bullet
	
	void Start () 
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, bulletRange);
	}
	
	void OnExplode()
	{
		// Instantiate the explosion where the rocket is with the random rotation
		if(explosion)
		{
			Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
			Instantiate(explosion, transform.position, randomRotation);
		}
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits the player, explode and destroy the projectile
		if(col.tag == "Player")
		{
			OnExplode();
			Destroy (gameObject);
		}
		// Also, do the same thing for the other required tags
		else if(col.tag != "Enemy" && col.tag != "EnemyFlyer" && col.tag != "EnemyGunner" && col.tag != "Gun" && col.tag != "Ground")
		{
			OnExplode();
			Destroy (gameObject);
		}
	}
}
