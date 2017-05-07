using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public GameObject player;
	public Rigidbody rb;
	public Vector3 initDirection;
	bool initForce = false;

	// Use this for initialization
	void Start () {
		rb.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!initForce) {
			rb.AddForce ((initDirection - transform.position).normalized * 100 * Time.smoothDeltaTime, ForceMode.Impulse);
			initForce = true;
		}
		rb.AddForce ((player.transform.position - transform.position).normalized * 35 * Time.smoothDeltaTime);
	}

	void Update(){
		transform.LookAt (player.transform);
		//transform.position = Vector3.Lerp (transform.position, player.transform.position, .001f);
	}
}
