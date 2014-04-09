using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
	public Rigidbody2D bullet;				// Prefab of the bullet
	public float speed = 20f;				// The speed the bullet will fire at

	//Variables to handle fire rate
	private float lastFireTime = 0f;
	public float fireRate = 2f;
	public bool canFire = true;
	public bool isFiring = false;

	void Update () {
		if(isFiring)
		{
			if(Time.time > lastFireTime + fireRate) {
				canFire = true;
			}
			if(canFire){
				Fire();
				lastFireTime = Time.time;
				canFire = false;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) 
	{
		// Stop moving and start shooting if the player enters its detection zone
		if(collider.tag == "Player")
		{
			isFiring = true;
		}
	}
	
	void OnTriggerExit2D (Collider2D collider) 
	{
		// Start moving and stop shooting if the player leaves its detection zone
		if(collider.tag == "Player")
		{
			isFiring = false;
		}
	}
	
	void Fire()
	{
		audio.Play();
		
		GameObject Aimer = transform.parent.gameObject;
		EnemyAimAssist AimScript = Aimer.GetComponent<EnemyAimAssist>();
		Transform barrel = transform.Find("enemyBarrel");

		Rigidbody2D bulletInstance;
		Vector3 temp = new Vector3(speed, 0);
		if(AimScript.facingRight){
			bulletInstance = Instantiate(bullet, barrel.position, barrel.rotation) as Rigidbody2D;
			bulletInstance.velocity = barrel.rotation * temp;
		}
		else{
			bulletInstance = Instantiate(bullet, barrel.position, Quaternion.Inverse(barrel.rotation) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
			bulletInstance.velocity = Quaternion.Inverse(barrel.rotation) * -temp;
		}
	}
}
