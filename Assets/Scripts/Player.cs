using System;
using Mirror;


public class Player : NetworkBehaviour
{
    [SyncVar]
    public bool isPC;

    public static event Action<Player, string> OnMessage;

    [Command]
    public void CmdSend(string message)
    {
        if (message.Trim() != "")
            RpcReceive(message.Trim());
    }

    [ClientRpc]
    public void RpcReceive(string message)
    {
        OnMessage?.Invoke(this, message);
    }
}
