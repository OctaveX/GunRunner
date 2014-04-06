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


		//Debug.Log(mouse_pos.x + " " + transform.position.x);
		//If the input is moving the player right and the player is facing left...
		if(mouse_pos.x >= transform.position.x && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(mouse_pos.x < transform.position.x && facingRight)
			// ... flip the player.
			Flip();


		/*Testing aiming with thumbsticks*/
//		if(Input.GetAxis("Mouse Y") == 0 || Input.GetAxis("Mouse X") == 0){
//			float angle = Mathf.Atan2 (Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * Mathf.Rad2Deg;
//			Debug.Log(Input.GetAxisRaw("Mouse X") +" "+ Input.GetAxisRaw("Mouse Y"));
//			if(facingRight){
//				this.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, angle));
//			}
//			else{
//				this.transform.localRotation = Quaternion.Euler (new Vector3(0, 0, -angle - 180));
//			}
//		
//		}
//
//		//If the input is moving the player right and the player is facing left...
//		if(Input.GetAxis("Mouse X") > 0 && !facingRight)
//			// ... flip the player.
//			Flip();
//		// Otherwise if the input is moving the player left and the player is facing right...
//		else if(Input.GetAxis("Mouse X") > 0 && facingRight)
//			// ... flip the player.
//			Flip();
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.parent.localScale;
		theScale.x *= -1;
		transform.parent.localScale = theScale;
	}

}


