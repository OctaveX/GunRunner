using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	void OnGUI () {
		GUIStyle bigStyle = GUI.skin.GetStyle("Label");
		bigStyle.alignment = TextAnchor.MiddleCenter;
		bigStyle.fontSize = 50;

		GUI.Label (new Rect (0, -40, Screen.width, Screen.height), "Game Over", bigStyle);

		GUIStyle smallStyle = GUI.skin.GetStyle("Label");
		smallStyle.alignment = TextAnchor.MiddleCenter;
		smallStyle.fontSize = 20;

		GUI.Label (new Rect (0, 20, Screen.width, Screen.height), "Score: " + PlayerControl.score, smallStyle);
		GUI.Label (new Rect (0, 40, Screen.width, Screen.height), "Levels Completed: " + (PlayerControl.level) + "/" + (Application.levelCount - 2), smallStyle);
		GUI.Label (new Rect (0, 140, Screen.width, Screen.height), "'Fire' to continue...", smallStyle);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1"))
		{
			Application.LoadLevel(0);
		}
	}
}
