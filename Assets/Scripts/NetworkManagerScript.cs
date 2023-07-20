using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
﻿using System;
using System.Linq;
using System.Numerics;
using System.Net;
using System.Net.Sockets;


public class NetworkManagerScript : NetworkManager
{
    // Called by UI element NetworkAddressInput.OnValueChanged
    public void SetHostname(string hostname)
    {
        networkAddress = hostname;
    }

        public void SetBase36Hostname(string hostname)
    {
        try
        {
        networkAddress = ToAddr(Decode(hostname));
        }
        catch (Exception)
        {}
    }

    public struct CreatePlayerMessage : NetworkMessage
    {
        public bool isPC;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
        if (NetworkServer.connections.Count != 1)
        {
            conn.Send(new CreatePlayerMessage { isPC = false });
        }
        else
        {
            conn.Send(new CreatePlayerMessage { isPC = true });
        }
    }

    void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
    {
        // create a gameobject using the name supplied by client
        GameObject playergo = Instantiate(playerPrefab);
        playergo.GetComponent<Player>().isPC = createPlayerMessage.isPC;

        // set it as the player
        NetworkServer.AddPlayerForConnection(connection, playergo);
    }

    static string ToAddr(long address)
    {
        return IPAddress.Parse(address.ToString()).ToString();
    }

    public static long Decode(string value)
        {
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty value.");
            value = value.ToUpper();
            bool negative = false;
            if (value[0] == '-')
            {
                negative = true;
                value = value.Substring(1, value.Length - 1);
            }
            if (value.Any(c => !Digits.Contains(c)))
                throw new ArgumentException("Invalid value: \"" + value + "\".");
            var decoded = 0L;
            for (var i = 0; i < value.Length; ++i)
                decoded += Digits.IndexOf(value[i]) * (long)BigInteger.Pow(Digits.Length, value.Length - i - 1);
            return negative ? decoded * -1 : decoded;
        }
}
