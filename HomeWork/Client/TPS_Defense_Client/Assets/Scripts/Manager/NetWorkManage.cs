using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

using System.Threading;
public class NetWorkManage
{
    private NetSocket client;
    public String host = "localhost";
    public Int32 port = 50000;
    private bool isConnected;
    private byte[] messTmp;
    private DataHolder dataHolder;
    private NetWorkManage(DataHolder holder)
    {
        dataHolder = holder;
        isConnected = false;
        messTmp = new byte[1024];
    }
    private static NetWorkManage m_Instance;
    public static NetWorkManage Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new NetWorkManage(DataHolder.Instance);
            return m_Instance;
        }
    }
    public bool Connect(String ip, Int32 port)
    {
 
        client = new NetSocket();
        try
        {
            Debug.Log(ip);
            Debug.Log(port);
            client.SetupSocket(ip,port);
            isConnected = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        if (isConnected)
        {
            Thread connectServer = new Thread(ReceiveMessage);
            connectServer.IsBackground = true;
            connectServer.Start();
        }
        return isConnected;
    }
    public void ReceiveMessage()
    {
        while (isConnected)
        {
            string temp = client.ReadSocket();
            if (temp != null)
            {
          
        
                if (temp == "exit")
                {
                    isConnected = false;
                    break;
                }
                dataHolder.AddMessage(temp);
            }
        }
        client.CloseSocket();
        return;
    }
    public void SendMessage(String message)
    {

        
        client.WriteSocket(message);
    }
}
