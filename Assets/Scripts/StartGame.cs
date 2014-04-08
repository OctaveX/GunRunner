using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	void OnClick ()
	{
		PlayerControl.score = 0;
		PlayerControl.lives = PlayerControl.defaultLives;
		Application.LoadLevel (1);
	}
}
