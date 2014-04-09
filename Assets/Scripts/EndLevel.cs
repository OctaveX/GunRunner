using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	public AudioClip finishClip;		// The audio clip to play when the player finishes a level
	public float clipVolume = 1.0f;		// The volume to play the finishing clip at
	private bool finished = false;		// A flag to determine whether or not the player has trigger the end level event yet

	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone, then trigger the finish event
		if(other.tag == "Player" && !finished)
		{
			StartCoroutine(Finish());
		}
	}

	IEnumerator Finish ()
	{
		// First, disable the player's movement, increase their score, and play the finish sound
		finished = true;
		AudioSource.PlayClipAtPoint(finishClip, transform.position, clipVolume);
		GameObject.Find ("hero").GetComponent<PlayerControl> ().enabled = false;
		PlayerControl.score += PlayerControl.runningScore;
		PlayerControl.runningScore = 0;

		// Then, after 3 seconds, increase the player's level count and load the new level
		yield return new WaitForSeconds (3);
		if (PlayerControl.level < (Application.levelCount-2))
						PlayerControl.level = Application.loadedLevel+1;
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
