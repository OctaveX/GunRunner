using UnityEngine;
using System.Collections;

public class EnemyAimAssist : MonoBehaviour {
	public bool facingRight = true;

	void Update(){
		//Aiming
		Vector3 player_pos = GameObject.Find("hero").transform.position;
		Vector3 enemy_pos = transform.position;
		Vector3 aim_pos = new Vector3 ();
		aim_pos.x = player_pos.x - enemy_pos.x;
		aim_pos.y = player_pos.y - enemy_pos.y;

		// Set the gun's angle depending on if they're facing left or right
		if(facingRight){
			float angle = Mathf.Atan2 (aim_pos.y, aim_pos.x) * Mathf.Rad2Deg;
			this.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, angle));
		}
		else{
			float angle = Mathf.Atan2 (aim_pos.y, aim_pos.x) * Mathf.Rad2Deg;
			this.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, -angle - 180));
		}

		//If the input is moving the player right and the player is facing left, then flip the enemy
		if(player_pos.x >= transform.position.x && !facingRight)

			Flip();
		// Also, if the input is moving the player left and the player is facing right, then flip the enemy
		else if(player_pos.x < transform.position.x && facingRight)
			Flip();
	}

	public void Flip ()
	{
		// Switch the way the enemy is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.parent.localScale;
		theScale.x *= -1;
		transform.parent.localScale = theScale;
	}

}


