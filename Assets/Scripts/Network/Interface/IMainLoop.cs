
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainLoop
{
    void QueueInLoop(Packet packet);   //插入回调
}
