using UnityEngine;
using System.Collections;

public class EnemyGunner : Enemy
{

	GameObject enemyGunObject;
	EnemyGun enemyGun;

	void Awake()
	{
		base.Awake ();
		enemyGunObject = transform.Find("enemyWeaponHolder/enemyGun").gameObject;
		enemyGun = enemyGunObject.GetComponent<EnemyGun>();
	}

	void FixedUpdate ()
	{
		// Handle walking
		if (!enemyGun.isFiring) {
			transform.Find("enemyWeaponHolder").gameObject.GetComponent<EnemyAimAssist>().enabled = false;

			blocked = Physics2D.Linecast (transform.position, transform.Find ("frontCheck").position, 1 << LayerMask.NameToLayer ("Ground"));
			frontGrounded = Physics2D.Linecast (transform.position, transform.Find ("frontGroundCheck").position, 1 << LayerMask.NameToLayer ("Ground"));

			if ((blocked || !frontGrounded) && !dead) {
					Flip ();
			}

			// Set the enemy's velocity
			rigidbody2D.velocity = new Vector2 (transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
		}
		// Handle shooting
		else{
			transform.Find("enemyWeaponHolder").gameObject.GetComponent<EnemyAimAssist>().enabled = true;
			rigidbody2D.velocity = new Vector2(0,0);
		}

		anim.SetFloat("enemySpeed", Mathf.Abs(rigidbody2D.velocity.x));

		// Kill the enemy if their HP has reached zero or below and stop them from shooting any more
		if (HP <= 0 && !dead){
			Death ();
			enemyGunObject.GetComponent<EnemyGun>().canFire = false;
		}
	}
	
	public void Flip()
	{
		transform.Find("enemyWeaponHolder").gameObject.GetComponent<EnemyAimAssist>().Flip();
	}
}
