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
	public GameObject Spawner;
	EnemySpawner spawnScript;
	public int port;

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

	void Start () {
		init ();
		spawnScript = Spawner.GetComponent<EnemySpawner> ();
	}

	void OnGUI(){
		Rect rectObj = new Rect (40, 10, 200, 400);
		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box(rectObj, "# UDPReceive\n 127.0.0.1 " + port + " #\n" +
			"Last Packet: " + lastReceivedUDPPacket +
			"\nAll Messages:" + allReceivedUDPPackets, style);
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
				byte[] data = client.Receive(ref anyIP);

				string text = Encoding.UTF8.GetString(data);
				print(text);
				/*text = text.TrimEnd(';');
				text = text.TrimEnd(';');*/
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