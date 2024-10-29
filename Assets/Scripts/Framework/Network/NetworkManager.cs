using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    
    [Header("Server Config")]
    public string m_server_IP = "127.0.0.1";

    public int m_server_Port = 5500;
    public bool isConnect = false;

    public TcpClient m_player_socket;
    public NetworkStream m_myStream;
    
    public StreamReader m_myReader;
    public StreamWriter m_myWriter;

    private byte[] asyncBuffer;
    public bool shouldHandleData = false;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ConnectGameServer();
    }

    private void OnDestroy()
    {
        if (m_player_socket != null)
        {
            CloseConnection();
        }
    }

    void ConnectGameServer()
    {
        if (m_player_socket != null)
        {
            if (m_player_socket.Connected || isConnect) return;

            CloseConnection();
        }

        m_player_socket = new TcpClient();
        m_player_socket.ReceiveBufferSize = 4096;
        m_player_socket.SendBufferSize = 4096;
        m_player_socket.NoDelay = false;
        Array.Resize(ref asyncBuffer, 8192);
        m_player_socket.BeginConnect(m_server_IP, m_server_Port, new AsyncCallback(OnConnectedCallBack), m_player_socket);
        isConnect = true;
    }

    void OnConnectedCallBack(IAsyncResult result)
    {
        if (m_player_socket != null)
        {
            m_player_socket.EndConnect(result);
            if (m_player_socket.Connected)
            {
                m_player_socket.NoDelay = true;
                m_myStream = m_player_socket.GetStream();
                m_myStream.BeginRead(asyncBuffer, 0, 8192, OnReceive, null);
            }
            else
            {
                isConnect = false;
            }
        }
    }

    void OnReceive(IAsyncResult result)
    {
        if (m_player_socket != null)
        {
            if (m_player_socket == null) return;

            int byteArray = m_myStream.EndRead(result);
            byte[] mBytes = null;
            Array.Resize(ref mBytes, byteArray);
            Buffer.BlockCopy(asyncBuffer, 0, mBytes, 0, byteArray);

            if (byteArray == 0)
            {
                Debug.Log("Disconnected from the server");
                m_player_socket.Close();
                return;
            }

            if (m_player_socket != null)
            {
                m_myStream.BeginRead(asyncBuffer, 0, byteArray, OnReceive, null);
            }
        }
    }

    void CloseConnection()
    {
        if (m_player_socket != null)
        {
            m_player_socket.Close();
            m_player_socket = null;
        }
    }
}
