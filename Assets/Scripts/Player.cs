using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour {

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private static string user = database.user;
	private Component FPCScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isClient) {
		
			if (GetComponentInChildren<NetworkView> ().isMine) {
				print ("Created ma' player! Username: " + user);
				FPCScript = GameObject.Find(user).GetComponent("FirstPersonContro/ller");
				FPCScript = enabled;
			} else {
				print ("Found someone elses player!");
				GetComponentInChildren<Camera> ().enabled = false;
				GetComponentInChildren<AudioListener> ().enabled = false;
				GetComponentInChildren<CharacterController> ().enabled = false;
				GameObject.Find(user).GetComponent<TextMesh> ().text = user;
			}
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting) {
			syncPosition = GetComponent<Rigidbody>().position;
			stream.Serialize (ref syncPosition);
		} else {
			stream.Serialize (ref syncPosition);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncStartPosition = GetComponent<Rigidbody>().position;
			syncEndPosition = syncPosition;
		}
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}
}
