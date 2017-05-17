using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starter : MonoBehaviour {

	public GameObject Spawn;
	public GameObject score;
	public GameObject audio;
	scoreFollow scoreScript;
	Spawner script;
	// Use this for initialization
	void Start(){
		Spawn = GameObject.FindGameObjectWithTag ("Spawner");
		script = Spawn.GetComponent<Spawner> ();
		score = GameObject.Find ("scoreKeeper");
		scoreScript = score.GetComponent<scoreFollow> ();
		script.playing = false;
	}

	void OnDestroy(){
		scoreScript.prevScore = -1;
		script.playing = true;
		Instantiate (audio);
	}
}
