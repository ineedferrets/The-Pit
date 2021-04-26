using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBlock : Block
{
    protected override void DestroyBlock()
    {
        GameLogicScript_Marko.Instance.RoomDataSyncController.SetSceneName("GameOverScene");
        GameLogicScript_Marko.Instance.RoomDataSyncController.SetWinnerName(GameLogicScript_Marko.Instance.PlayerName);
    }
}
