using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private const string typeName = "TestMyGame1";
    private const string gameName = "RoomName";

    private bool isRefreshingHostList = false;
    private HostData[] hostList;

    public GameObject playerPrefab;


	public bool isServerUseProxy = false;
	public bool isClientUseProxy = false;

	public string proxyIP = "1.1.1.1";
	public string proxyPort = "1111";

	void Start()
	{
		SetProxyData();
		MasterServer.UnregisterHost();
	}

	void SetProxyData()
	{
		Network.proxyIP = proxyIP;
		int port = 0;
		if (int.TryParse(proxyPort, out port))
		{
			Network.proxyPort = port;
		}

	}

    void OnGUI()
    {
		
		proxyIP = GUI.TextField(new Rect(100, 0, 250, 50), proxyIP);
		proxyPort = GUI.TextField(new Rect(100, 50, 250, 50), proxyPort);

		if (GUI.Button(new Rect(100, 100, 250, 50), "SetProxyData"))
		{
			SetProxyData();
		}

        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 200, 250, 50), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 50), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }

    private void StartServer()
    {
		Network.useProxy = isServerUseProxy;
        Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
		SpawnPlayer("Player_S");
    }


    void Update()
    {
        if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            isRefreshingHostList = false;
            hostList = MasterServer.PollHostList();
        }
    }

    private void RefreshHostList()
    {
        if (!isRefreshingHostList)
        {
            isRefreshingHostList = true;
            MasterServer.RequestHostList(typeName);
        }
    }


    private void JoinServer(HostData hostData)
	{
		Network.useProxy = isClientUseProxy;
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        SpawnPlayer("Player_C");
    }


    private void SpawnPlayer(string name)
    {
		GameObject go = (GameObject)Network.Instantiate(playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
		go.name = name;
    }
}
