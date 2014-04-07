using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.

	//Variables to handle fire rate
	private float lastFireTime = 0f;
	public float fireRate = 2f;
	public bool canFire = true;
	public bool isFiring = false;
	
	private EnemyGunner playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	
	private bool facingRight = true;

	// Use this for initialization
	void Awake () {
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<EnemyGunner>();
	}
	
	// Update is called once per frame
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
	
	void Fire(){
		// ... set the animator Shoot trigger parameter and play the audioclip.
		anim.SetTrigger("Shoot");
		audio.Play();
		
		// ... instantiate the rocket facing right and set it's velocity to the right. 
		Rigidbody2D bulletInstance;
		EnemyRocket bullet;
		
		Vector3 temp = new Vector3(speed, 0);
		//AimAssist AimScript = (AimAssist)transform.Find("enemyWeaponHolder").GetComponent("EnemyAimAssist");

		GameObject Aimer = transform.parent.gameObject;
		EnemyAimAssist AimScript = Aimer.GetComponent<EnemyAimAssist>();

		Transform barrel = transform.Find("enemyBarrel");

		if(AimScript.facingRight){
			bulletInstance = Instantiate(rocket, barrel.transform.position, barrel.transform.rotation) as Rigidbody2D;
			bulletInstance.velocity = barrel.transform.rotation * temp;
		}
		else{
			bulletInstance = Instantiate(rocket, barrel.transform.position, Quaternion.Inverse(barrel.transform.rotation) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
//			bullet = (Rocket)bulletInstance.GetComponent("Rocket");
//			bullet.bulletRange = .2f;
			bulletInstance.velocity = Quaternion.Inverse(barrel.transform.rotation) * -temp;
		}
	}
}
