using UnityEngine;
using System.Collections;

public class EnemyFlyer : Enemy
{

	public Rigidbody2D rocket;			// Enemy rocket prefab
	public float fireRate = 1f;			// The rate at which the flyer can drop rockets

	private bool canFire = true;		// A flag to signal whether or not the flyer can drop bombs
	private float lastFireTime = 0f;	// The last time that a rocket was dropped
	private float travelTime = 2f;		// The amount of time to fly before flipping
	private float lastFlipTime;			// The last time that the flyer was flipped
	private bool canFlip = false;		// A flag to signal whether or not the flyer can flip horizontally

	void Update(){
		if(Time.time > lastFireTime + fireRate) {
			canFire = true;
		}
		if(canFire){
			Fire();
			lastFireTime = Time.time;
			canFire = false;
		}
	}

	void FixedUpdate ()
	{
		blocked = Physics2D.Linecast(transform.position, transform.Find("frontCheck").position, 1 << LayerMask.NameToLayer("Ground"));

		if(Time.time > lastFlipTime + travelTime) {
			canFlip = true;
		}

		if(blocked || canFlip){
			Flip ();
			lastFlipTime = Time.time;
			canFlip = false;
		}

		rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, 0f);	

		if(HP <= 0 && !dead)
			Death ();
	}

	void Fire(){
		Rigidbody2D bulletInstance;
		Vector3 temp = new Vector3(0, -5f);
		Vector3 launchPoint = new Vector3(transform.position.x, transform.position.y - 1);

		bulletInstance = (Rigidbody2D)Instantiate(rocket, launchPoint, transform.rotation * Quaternion.Euler(0, 0, -180));
		EnemyProjectile bullet = bulletInstance.GetComponent<EnemyProjectile>();
		bullet.bulletRange = 5f;
		bulletInstance.velocity = transform.rotation * temp;
	}
}
