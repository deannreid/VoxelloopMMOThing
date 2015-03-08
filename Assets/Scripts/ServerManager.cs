using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (SystemInfo.graphicsDeviceID == 0) {
			Application.LoadLevel ("main");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
