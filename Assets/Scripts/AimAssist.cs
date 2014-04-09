using UnityEngine;
using System.Collections;

public class AimAssist : MonoBehaviour {

	public bool facingRight = true;

	void Update(){
		//Aiming
		Vector3 mouse_pos = Input.mousePosition;
		Vector3 player_pos = Camera.main.WorldToScreenPoint(this.transform.position);
		mouse_pos.x = mouse_pos.x - player_pos.x;
		mouse_pos.y = mouse_pos.y - player_pos.y;
		
		if(facingRight){
			float angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			this.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, angle));
		}
		else{
			float angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			this.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, -angle - 180));
		}

		//If the input is moving the player right and the player is facing left, then flip the player
		if(mouse_pos.x >= transform.position.x && !facingRight)
			Flip();
		// Also, if the input is moving the player left and the player is facing right, then flip the player
		else if(mouse_pos.x < transform.position.x && facingRight)
			Flip();
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.parent.localScale;
		theScale.x *= -1;
		transform.parent.localScale = theScale;
	}

}


