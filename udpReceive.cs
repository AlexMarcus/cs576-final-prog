using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class udpReceive : MonoBehaviour {

	Thread receiveThread;
	UdpClient client;
	public GameObject enemySpawner;
	Spawner spawnScript;
	public int port;
	public IPAddress connectedIP;

	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets="";

	private static void Main(){
		udpReceive receiveObj = new udpReceive ();
		receiveObj.init ();

		string text = "";
		do {
			text = Console.ReadLine ();
		} while(!text.Equals ("exit"));
	}
	// Use this for initialization
	void Start () {
		init ();
		spawnScript = enemySpawner.GetComponent<Spawner> ();
	}

	private void init(){
		print ("udpReceive.init()");

		port = 12000;

		receiveThread = new Thread (new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}

	private void ReceiveData(){
		client = new UdpClient (port);
		//client.Client.Blocking = false;
		while (true) {
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				connectedIP = anyIP.Address;
				byte[] data = client.Receive(ref anyIP);

				string text = Encoding.UTF8.GetString(data);
				spawnScript.locationsStr = text;

				print(">>" + text);

				lastReceivedUDPPacket = text;
				allReceivedUDPPackets = allReceivedUDPPackets+text;
			}
			catch(Exception err){
				print (err.ToString ());
			}
		}
	}

	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}

	void OnDisable() 
	{ 
		if ( receiveThread!= null) 
			receiveThread.Abort(); 

		client.Close(); 
	} 
}