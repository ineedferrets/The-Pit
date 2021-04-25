using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameBlock : Block
{


    protected override void DestroyBlock()
    {
        GameLogicScript_Marko.Instance.RoomDataSyncController.SetSceneName("GameScene");
    }
}
