using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D bulletRigidBody;		// Player bullet prefab
	public float speed = 20f;				// The player's fire rate

	//Variables to handle fire rate
	private float lastFireTime = 0f;
	private float fireRate = .1f;
	private bool canFire = true;			

	public int gunType = 0;					// The gun currently being used by the player

	private Animator anim;					// Reference to the Animator component.
	
	void Awake () {
		anim = transform.root.gameObject.GetComponent<Animator>();
	}

	void Update () {
		if(Input.GetButton("Fire1"))
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
	
	void Fire(){
		// Play the gunshot animation and audioclip
		anim.SetTrigger("Shoot");
		audio.Play();
		
		// Create the bullet depending on where the player is aiming
		Rigidbody2D bulletInstance;
		Bullet bullet;
		GameObject Aimer = GameObject.Find("gunHolder");
		AimAssist AimScript = Aimer.GetComponent<AimAssist>();
		GameObject barrel = GameObject.Find("Barrel");
		Vector3 temp = new Vector3(speed, 0);

		if(gunType == 1){
			if(AimScript.facingRight){
				fireRate = 1f;
				for (int i = 20; i >= -20; i -= 5){
					bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, barrel.transform.rotation * Quaternion.Euler(0,0,i)) as Rigidbody2D;
					bullet = bulletInstance.GetComponent<Bullet>();
					bullet.bulletRange = .25f;
					bulletInstance.velocity = barrel.transform.rotation * Quaternion.Euler(0,0,i) * temp ;
				}
			}
			else{
				for (int i = 20; i >= -20; i -= 5){
					bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, Quaternion.Inverse(barrel.transform.rotation * Quaternion.Euler(0,0,i)) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
					bullet = bulletInstance.GetComponent<Bullet>();
					bullet.bulletRange = .25f;
					bulletInstance.velocity = Quaternion.Inverse(barrel.transform.rotation)  * Quaternion.Inverse(Quaternion.Euler(0,0,i)) * -temp;
				}
			}
		}
		else if(gunType == 2){
			if(AimScript.facingRight){
				fireRate = .3f;
				for (int i = 5; i >= -5; i -= 10){
					bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, barrel.transform.rotation * Quaternion.Euler(0,0,i)) as Rigidbody2D;
					bullet = bulletInstance.GetComponent<Bullet>();
					bullet.bulletRange = .5f;
					bulletInstance.velocity = barrel.transform.rotation * Quaternion.Euler(0,0,i) * temp ;
				}
			}
			else{
				for (int i = 5; i >= -5; i -= 10){
					bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, Quaternion.Inverse(barrel.transform.rotation * Quaternion.Euler(0,0,i)) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
					bullet = bulletInstance.GetComponent<Bullet>();
					bullet.bulletRange = .5f;
					bulletInstance.velocity = Quaternion.Inverse(barrel.transform.rotation)  * Quaternion.Inverse(Quaternion.Euler(0,0,i)) * -temp;
				}
			}
		}
		else if(gunType == 3){
			fireRate = .1f;
			if(AimScript.facingRight){
				bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, barrel.transform.rotation) as Rigidbody2D;
				bullet = bulletInstance.GetComponent<Bullet>();
				bullet.bulletRange = 1f;
				bulletInstance.velocity = barrel.transform.rotation * temp;
			}
			else{
				bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, Quaternion.Inverse(barrel.transform.rotation) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
				bullet = bulletInstance.GetComponent<Bullet>();
				bullet.bulletRange = 1f;
				bulletInstance.velocity = Quaternion.Inverse(barrel.transform.rotation) * -temp;
			}
		}
		else {
			fireRate = .4f;
			if(AimScript.facingRight){
				bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, barrel.transform.rotation) as Rigidbody2D;
				bullet = bulletInstance.GetComponent<Bullet>();
				bullet.bulletRange = 2f;
				bullet.bulletRange = .6f;
				bulletInstance.velocity = barrel.transform.rotation * temp;
			}
			else{
				bulletInstance = Instantiate(bulletRigidBody, barrel.transform.position, Quaternion.Inverse(barrel.transform.rotation) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
				bullet = bulletInstance.GetComponent<Bullet>();
				bullet.bulletRange = 2f;
				bullet.bulletRange = .6f;
				bulletInstance.velocity = Quaternion.Inverse(barrel.transform.rotation) * -temp;
			}
		}
	}
}
