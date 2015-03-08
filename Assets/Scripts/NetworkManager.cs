using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject playerPrefab;
	private const string typeName = "VoxelLoopMMO";
	private const string gameName = "Europe #1";
	private HostData[] hostList;
	public static string user = database.user;
	public static string nametag;

	// Use this for initialization
	void Start () {

		if (SystemInfo.graphicsDeviceID == 0) {
			print ("Welcome to the Curtis' Linux Dedicated Server!");
			StartServer();
		} 
		else {
			print ("Running in client mode!");
			//ClientConnect();
			ConnectToServer();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// Server stuff, not really related to the client
	void StartServer() {
		Network.InitializeServer(4, 25000, false);
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized() {
		print ("Server is running! Waiting for connections.");
	}

	//Annnnddd... Back to the client code!
	//void ClientConnect() {
	//	print ("Connecting to master server...");
	//	MasterServer.RequestHostList(typeName);
	//}

	//void OnMasterServerEvent(MasterServerEvent msEvent)
	//{
	//	if (msEvent == MasterServerEvent.HostListReceived)
	//		hostList = MasterServer.PollHostList();
	//	print ("Master server polled! Recieved server list.");
	//
	//	Connection();
	//}

	//void Connection()
	//This is actually obselete at the moment, some error message stops server joining via this method
	//{
	//	if (hostList != null) {
	//		for (int i = 0; i < hostList.Length; i++) {
	//			print ("Master server poll returned:");
	//			print (hostList [i].gameName);
	//			JoinServer (hostList [i]);
	//		}
	//	} 
	//}

	//void JoinServer(HostData hostData)
	//{
	//	print ("Connecting to server...");
	//	Network.Connect(hostData.gameName);
	//}

	void ConnectToServer()
	{
		Network.Connect ("37.59.30.49:9987", 25000);
	}
	
	void OnConnectedToServer()
	{
		print ("Connected to server!");
		{
			SpawnPlayer ();
		}
	}
	private void SpawnPlayer()
	{
		{
			Object newPlayer = Network.Instantiate(playerPrefab, new Vector3(50f, 10f, 50f), Quaternion.identity, 0);
			newPlayer.name = user;
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		if (Network.isServer) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		}
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (Network.isClient) {
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Application.LoadLevel("login");
		}
	}
}