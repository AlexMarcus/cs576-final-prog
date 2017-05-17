using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreFollow : MonoBehaviour {
	public TextMeshPro display;
	public int score, prevScore;
	// Use this for initialization
	void Start () {
		display = GetComponent<TextMeshPro> ();
		score = 0;
		prevScore = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (score > PlayerPrefs.GetInt ("highscore")) {
			PlayerPrefs.SetInt ("highscore", score);
		}
		if (score > prevScore) {
			prevScore = score;
			display.SetText ("Score: " + score + "\t Highscore: " + PlayerPrefs.GetInt("highscore"));
		}
	}
}
