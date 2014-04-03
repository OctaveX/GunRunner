﻿using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
	
	
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
		if(Input.GetButtonDown("Fire1"))
		{
			Fire();
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

		if(AimScript.facingRight){
			bulletInstance = Instantiate(rocket, transform.position, transform.rotation) as Rigidbody2D;
			bulletInstance.velocity = transform.rotation * temp;
		}
		else{
			bulletInstance = Instantiate(rocket, transform.position, Quaternion.Inverse(transform.rotation)) as Rigidbody2D;
			bulletInstance.velocity = Quaternion.Inverse(transform.rotation) * -temp;
		}
	}
}
