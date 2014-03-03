using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingRight = true;

	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	bool jumpPressed = false;

	float jumpTime = 0.5f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		anim.SetBool("Grounded", grounded);

		float move = Input.GetAxis("Horizontal");

		anim.SetFloat("Speed", Mathf.Abs(move));


		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && !facingRight){
			Flip ();
		}
		else if(move < 0 && facingRight){
			Flip ();
		}
	}

	void Update(){
		float vMove = Input.GetAxis("Jump");
		if(vMove > 0 && grounded && !jumpPressed){
			rigidbody2D.AddForce(new Vector2(0, 600f));
			grounded = false;
			jumpPressed = true;
		}

		if (vMove == 0){
			jumpPressed = false;
		}
		if (grounded)
			jumpTime = 0.5f;
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
