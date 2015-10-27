using UnityEngine;
using System.Collections;

public sealed class Game : UnitySingleton<Game> {

    void Start()
    {
        Socket.Instance.Init();
    }
}
