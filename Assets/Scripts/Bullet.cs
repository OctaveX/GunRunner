using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public float bulletRange = 2f;

	void Start () 
	{
		// Destroy the bullet after 2 seconds if it doesn't get destroyed before then
		Destroy(gameObject, bulletRange);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy, then find the enemy's script and call the Hurt function.
		if(col.tag == "Enemy")
		{
			col.gameObject.GetComponent<Enemy>().Hurt();
			Destroy (gameObject);
		}
		else if(col.tag == "EnemyGunner")
		{
			col.gameObject.GetComponent<EnemyGunner>().Hurt();
			Destroy (gameObject);
		}
		else if(col.tag == "EnemyFlyer")
		{
			col.gameObject.GetComponent<EnemyFlyer>().Hurt();
			Destroy (gameObject);
		}
		// Otherwise if the player manages to shoot himself, then just destroy the bullet
		else if(col.gameObject.tag != "Player" && col.gameObject.tag != "Bullet" && col.gameObject.tag != "Gun" && col.gameObject.tag != "Ground")
		{
			Destroy (gameObject);
		}
	}
}
