using UnityEngine;
using System.Collections;

public class Bazooka : Gun {
	void Fire(){
		// ... set the animator Shoot trigger parameter and play the audioclip.
		//anim.SetTrigger("Shoot");
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
	}

}
