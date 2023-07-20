using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;
using System.Linq;
using System.Numerics;
using System.Net;
using System.Net.Sockets;

public class HandleIP : MonoBehaviour
{

    public Button buttonStart;
    public Text header;
    public Text connectionCode;


    void Start() 
    {
        buttonStart.onClick.AddListener(ButtonStart);
        connectionCode.text = $"Connection Code:\n{GetBase36IP()}";
    }

    void Update() 
    {
        if (NetworkServer.connections.Count != 2)
        {
            buttonStart.interactable = false;
            header.text = "Awaiting Connection";
        }
        else
        {
            buttonStart.interactable = true;
            header.text = "Connection Established";
        }
    }

    public void ButtonStart()
    {
        Debug.Log("Game Start");
    }

    private static string GetBase36IP()
    {
        IPHostEntry host;
            string localIP = "0.0.0.0";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }

        Debug.Log(localIP);
        return Encode36(IPToInt(localIP));
    }

    private static long IPToInt(string addr)
    {
        IPAddress address = IPAddress.Parse(addr);
        byte[] addressBytes = address.GetAddressBytes();
        // This restriction is implicit in your existing code, but
        // it would currently just lose data...
        if (addressBytes.Length != 4)
        {
            throw new ArgumentException("Must be an IPv4 address");
        }
        int networkOrder = BitConverter.ToInt32(addressBytes, 0);
        return (uint) IPAddress.NetworkToHostOrder(networkOrder);
    }

    private static string Encode36(long value)
        {
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (value == long.MinValue)
            {
                //hard coded value due to error when getting absolute value below: "Negating the minimum value of a twos complement number is invalid.".
                return "-1Y2P0IJ32E8E8";
            }
            bool negative = value < 0;
            value = Math.Abs(value);
            string encoded = string.Empty;
            do
                encoded = Digits[(int)(value % Digits.Length)] + encoded;
            while ((value /= Digits.Length) != 0);
            return negative ? "-" + encoded : encoded;
        }



}