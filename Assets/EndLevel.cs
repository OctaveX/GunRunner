using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	public AudioClip finishClip;

	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			StartCoroutine(Finish());
		}
	}

	IEnumerator Finish ()
	{
		AudioSource.PlayClipAtPoint(finishClip, transform.position);
		GameObject.Find ("hero").GetComponent<PlayerControl> ().enabled = false;
		yield return new WaitForSeconds (3);

		// TODO load a "Game Over" scene if it's the last level
		if (Application.loadedLevel == (Application.levelCount - 1)) Application.LoadLevel (Application.loadedLevel);
		else Application.LoadLevel (Application.loadedLevel + 1);
	}
}
