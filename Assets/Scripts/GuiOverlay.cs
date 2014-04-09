using UnityEngine;
using System.Collections;

public class GuiOverlay : MonoBehaviour {

	void OnGUI () {
		// Draw the player's current score, lives left, and current level
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleLeft;
		style.fontSize = 15;

		int levelDisplayed = (PlayerControl.level < 0 ? 0 : PlayerControl.level);
		int livesDisplayed = (PlayerControl.lives < 0 ? 0 : PlayerControl.lives);

		GUI.Label (new Rect (5, 0, 200, 30), "Score: " + (PlayerControl.score + PlayerControl.runningScore), style);
		GUI.Label (new Rect (5, 15, 200, 30), "Lives: " + livesDisplayed, style);
		GUI.Label (new Rect (5, 30, 200, 30), "Level: " + levelDisplayed, style);
	}
}
