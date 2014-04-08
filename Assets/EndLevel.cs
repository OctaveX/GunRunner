using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {
	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			// TODO load a "Game Over" scene if it's the last level
			if (Application.loadedLevel == (Application.levelCount - 1)) Application.LoadLevel (Application.loadedLevel);
			else Application.LoadLevel (Application.loadedLevel + 1);
		}
	}
}
