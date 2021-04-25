using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBlock : Block
{
    protected override void DestroyBlock()
    {
        GameLogicScript_Marko.Instance.RoomDataSyncController.SetSceneName("GameOverScene");
    }
}
