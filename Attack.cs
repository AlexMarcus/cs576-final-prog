using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public AudioClip boom;
	public GameObject player;
	public Vector3 initDirection, test;
	public Rigidbody rb;
	public float force = 100;
	public int initForce;
	int forceCounter;

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource> ().playOnAwake = false;
		GetComponent<AudioSource> ().clip = boom;
		rb.GetComponent<Rigidbody> ();
		test = new Vector3 (2.47f, 1f, 3f);
		initForce = 3;
		forceCounter = 0;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (forceCounter < initForce) {
			rb.AddForce ((initDirection - transform.position).normalized * force * Time.smoothDeltaTime, ForceMode.Impulse);
			forceCounter++;
		}
		rb.AddForce ((player.transform.position - transform.position).normalized * (35) * Time.smoothDeltaTime);
	}

	void Update(){
		transform.LookAt (player.transform);
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position, force * .00015f);
		//rb.AddForce ((player.transform.position - transform.position).normalized * (10) * Time.smoothDeltaTime, ForceMode.VelocityChange);
		//transform.position = Vector3.Lerp (transform.position, player.transform.position, .001f);
	}
		
}