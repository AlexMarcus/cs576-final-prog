using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameReset : MonoBehaviour {

	public GameObject restart;
	public GameObject scoreKeeper;
	scoreFollow script;

	void start(){
		print ("Headset start");
	}

	void OnTriggerEnter(){
		script = scoreKeeper.GetComponent<scoreFollow> ();
		script.score = 0;
		//script.prevScore = -1;
		Debug.Log ("something is colliding");
		GetComponent<AudioSource> ().Play ();
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("cube");
		if(cubes.Length > 0){
			foreach (GameObject cube in cubes) {
				Destroy (cube);
			}
		}
		Instantiate (restart);
	}
}
