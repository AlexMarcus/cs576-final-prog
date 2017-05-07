using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Linq;

public class EnemySpawner : MonoBehaviour {
	public string locationsStr = null;
	string[] locations;
	string[][] points, coors;
	int[] initForces;
	Vector3 spawnLoc, initDirection;
	public GameObject enemy;
	public GameObject target;
	GameObject enemyRef;
	Attack atkScript;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (locationsStr != null) {
			locations = locationsStr.Split (seperatePairs, System.StringSplitOptions.RemoveEmptyEntries);
		
			for (int i = 0; i < locations.Length; i++) {
				initForces [i] = getinitForce (locations [i]);
			}
			locationsStr = null;
			/*for (int i = 0; i < locations.Length; i++) {
				points [i] = locations [i].Split (seperatePoints, System.StringSplitOptions.RemoveEmptyEntries);
			}*/

			//mistake, have to seperate into coordinates before parsing
			for (int i = 0; i < locations.Length; i++) {
				coors[i] =  locations [i].Split (seperateAllCoors, System.StringSplitOptions.RemoveEmptyEntries);
			}

			for (int i = 0; i < locations.Length; i++) {
				spawnLoc = new Vector3 (float.Parse (coors [i] [0], CultureInfo.InvariantCulture.NumberFormat), target.transform.position.y, float.Parse (coors [i] [1], CultureInfo.InvariantCulture.NumberFormat));
				enemyRef = Instantiate (enemy, spawnLoc, target.transform.rotation);
				initDirection = new Vector3 (float.Parse (coors [i] [2], CultureInfo.InvariantCulture.NumberFormat), target.transform.position.y, float.Parse (coors [i] [3], CultureInfo.InvariantCulture.NumberFormat));
				atkScript = enemyRef.GetComponent<Attack> ();
				atkScript.player = target;
				atkScript.initDirection = initDirection;
			}

			print (initForces [0]);
			print (initForces [1]);
			print (initForces [2]);
			print (initForces [3]);
			print (initForces [4]);
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
