using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkEngine
{
    void Packet2NetworkEnginePendingPacketQueue(Packet packet);
}
