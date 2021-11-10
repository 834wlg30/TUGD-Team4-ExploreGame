using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSockets : MonoBehaviour
{
    //For tutorial see: https://www.youtube.com/watch?v=_c-r1S4PZb4&t=0s
    public enum SocketId
    {
        Spine,
        RightHand
    }

    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();

    // Start is called before the first frame update
    void Start()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (var socket in sockets)
        {
            socketMap[socket.socketId] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketId socketId)
    {
        socketMap[socketId].Attach(objectTransform);
    }
}
