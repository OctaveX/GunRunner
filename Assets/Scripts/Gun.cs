using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.

	//Variables to handle fire rate
	private float lastFireTime = 0f;
	public float fireRate = .1f;
	private bool canFire = true;
	
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	
	private bool facingRight = true;

	// Use this for initialization
	void Awake () {
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
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
		// ... set the animator Shoot trigger parameter and play the audioclip.
		anim.SetTrigger("Shoot");
		audio.Play();
		
		
		// ... instantiate the rocket facing right and set it's velocity to the right. 
		Rigidbody2D bulletInstance;
		
		Vector3 temp = new Vector3(speed, 0);

		//bool face = GameObject.Find("Gun").GetComponent<AimAssist>().facingRight
		GameObject Aimer = GameObject.Find("gunHolder");
		AimAssist AimScript = (AimAssist)Aimer.GetComponent("AimAssist");

		GameObject barrel = GameObject.Find("Barrel");

		if(AimScript.facingRight){
			bulletInstance = Instantiate(rocket, barrel.transform.position, barrel.transform.rotation) as Rigidbody2D;
			bulletInstance.velocity = barrel.transform.rotation * temp;
		}
		else{
			bulletInstance = Instantiate(rocket, barrel.transform.position, Quaternion.Inverse(barrel.transform.rotation) * Quaternion.Euler(0, 0, 180)) as Rigidbody2D;
			bulletInstance.velocity = Quaternion.Inverse(barrel.transform.rotation) * -temp;
		}
		GameObject player = GameObject.Find("hero");
		//player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, player.rigidbody2D.velocity.y + 10f);
	}
}
