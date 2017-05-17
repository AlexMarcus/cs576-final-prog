using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Linq;

public class Spawner : MonoBehaviour {
	public string locationsStr = null;
	string[] locations;
	string[][] points, coors;
	int[] initForces;
	Vector3 spawnLoc, initDirection;
	public Transform spawnPosition;
	public GameObject enemy;
	public GameObject Player;
	public bool playing = false;
	bool connected = false;
	GameObject enemyRef;
	Attack atkScript;
	Vector3 randomLocation, randomLocation2;

	//yes I know I spelled separate wrong
	char[] seperatePairs = {';'};
	char[] seperatePoints = {',' };
	char[] seperateCoors = { ' ' };
	char[] seperateAllCoors = { ' ', ',' };
	// Use this for initialization
	void Start () {
		initForces = new int[]{0, 0, 0, 0, 0};
		points = new string[5][];
		coors = new string[5][];
		InvokeRepeating ("SpawnEnemy", .5f, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (locationsStr == "connected") {
			connected = true;
		} else if (locationsStr == "disconnected") {
			connected = false;
		}
		else if (locationsStr != null && locationsStr != "connected" && playing) {
			locations = locationsStr.Split (seperatePairs, System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < locations.Length; i++) {
				initForces [i] = getinitForce (locations [i]);
			}
			locationsStr = null;
			/*for (int i = 0; i < locations.Length; i++) {
				points [i] = locations [i].Split (seperatePoints, System.StringSplitOptions.RemoveEmptyEntries);
			}*/

			for (int i = 0; i < locations.Length; i++) {
				coors[i] =  locations [i].Split (seperateAllCoors, System.StringSplitOptions.RemoveEmptyEntries);
			}

			for (int i = 0; i < locations.Length; i++) {
				spawnLoc = new Vector3 (float.Parse (coors [i] [0], CultureInfo.InvariantCulture.NumberFormat) + Player.transform.position.x, Player.transform.position.y, float.Parse (coors [i] [1], CultureInfo.InvariantCulture.NumberFormat) + Player.transform.position.z);
				enemyRef = Instantiate (enemy, spawnLoc, Player.transform.rotation);
				initDirection = new Vector3 (float.Parse (coors [i] [2], CultureInfo.InvariantCulture.NumberFormat) + Player.transform.position.x, Player.transform.position.y, float.Parse (coors [i] [3], CultureInfo.InvariantCulture.NumberFormat ) + Player.transform.position.z);
				atkScript = enemyRef.GetComponent<Attack> ();
				atkScript.initForce = initForces [i];
				atkScript.player = Player;
				atkScript.initDirection = initDirection;
			}

			print (initForces [0]);
			print (initForces [1]);
			print (initForces [2]);
			print (initForces [3]);
			print (initForces [4]);
		}
	}

	void SpawnEnemy(){
		//randomLocation = Random.insideUnitSphere * 10;
		if (playing && connected == false) {
			randomLocation = Random.onUnitSphere * 10;
			randomLocation.x += Player.transform.position.x;
			randomLocation.z += Player.transform.position.z;
			randomLocation.y = 0.0f;
			randomLocation2 = Random.onUnitSphere * 10;
			randomLocation2.y = Player.transform.position.y;
			enemyRef = Instantiate (enemy, randomLocation, enemy.transform.rotation);
			Attack atkScript = enemyRef.GetComponent<Attack> ();
			atkScript.player = Player;
			atkScript.initDirection = randomLocation2;
		}
	}

	int ForceDecision(float distance){
		if(distance > 14){
			return 7;
		}
		if(distance > 12){
			return 6;
		}
		if(distance > 10){
			return 5;
		}
		if(distance > 8){
			return 4;
		}
		if(distance > 6){
			return 3;
		}
		if(distance > 4){
			return 2;
		}
		else{
			return 1;
		}
	}

	int getinitForce(string distances){
		string[] coordinates = distances.Split (seperatePoints, System.StringSplitOptions.RemoveEmptyEntries);
		string[] coor1 = coordinates[0].Split (seperateCoors, System.StringSplitOptions.RemoveEmptyEntries);
		string[] coor2 = coordinates[1].Split (seperateCoors, System.StringSplitOptions.RemoveEmptyEntries);
		float x1 = float.Parse (coor1 [0], CultureInfo.InvariantCulture.NumberFormat);
		float x2 = float.Parse (coor2 [0], CultureInfo.InvariantCulture.NumberFormat);
		float y1 = float.Parse (coor1 [1], CultureInfo.InvariantCulture.NumberFormat);
		float y2 = float.Parse (coor2 [1], CultureInfo.InvariantCulture.NumberFormat);
		return ForceDecision(Mathf.Sqrt( ((x2-x1)*(x2-x1)) + ((y2-y1) * (y2-y1)) ));
	}
}
