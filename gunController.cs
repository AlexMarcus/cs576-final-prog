using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZEffects;

public class gunController : MonoBehaviour {

	public GameObject controllerLeft;
	private SteamVR_Controller.Device device;
	private SteamVR_TrackedObject trackedObj;

	public GameObject scoreKeeper;
	scoreFollow scoreScript;

	private SteamVR_TrackedController controller;

	public EffectTracer TracerEffect;
	public Transform muzzleTransform;
	public Transform tracerTransform;
	// Use this for initialization
	void Start () {
		controller = controllerLeft.GetComponent<SteamVR_TrackedController> ();
		controller.TriggerClicked += TriggerPressed;
		controller.PadClicked += PadPressed;
		trackedObj = controllerLeft.GetComponent<SteamVR_TrackedObject> ();
		scoreScript = scoreKeeper.GetComponent<scoreFollow> ();
	}

	private void TriggerPressed(object sender, ClickedEventArgs e){
		ShootWeapon();
		GetComponent<AudioSource> ().Play ();
		Debug.Log ("shooting left");
	}

	private void PadPressed(object sender, ClickedEventArgs e){
		scoreKeeper.GetComponent<MeshRenderer> ().enabled = !scoreKeeper.GetComponent<MeshRenderer> ().enabled;
	}
	
	public void ShootWeapon(){
		RaycastHit hit = new RaycastHit ();
		Ray ray = new Ray (muzzleTransform.position, muzzleTransform.forward);

		device = SteamVR_Controller.Input ((int)trackedObj.index);
		device.TriggerHapticPulse (750);
		TracerEffect.ShowTracerEffect (tracerTransform.position, tracerTransform.forward, 250f);

		if (Physics.Raycast (ray, out hit, 5000f)) {
			if (hit.collider.attachedRigidbody) {
				Debug.Log ("pow " + hit.collider.gameObject.name);
				if (hit.transform.gameObject.tag != "MainCamera"){
					if (hit.transform.gameObject.tag != "cube") {
						Destroy (hit.transform.gameObject);
					} else {
						scoreScript.score += 1;
						hit.transform.gameObject.GetComponent<MeshRenderer> ().enabled = false;
						hit.collider.enabled = false;
						hit.transform.gameObject.GetComponent<AudioSource> ().Play ();
						Destroy (hit.transform.gameObject, .3f);
					}
				}
			}
		}
	}
}
