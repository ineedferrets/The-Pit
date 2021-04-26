using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameBlock : Block
{


    protected override void DestroyBlock()
    {
        GameLogicScript_Marko.Instance.PlayerName = GameLogicScript_Marko.Instance.MainMenuScript.Username.text;
        GameLogicScript_Marko.Instance.RoomDataSyncController.SetSceneName("GameScene");
        if (GameLogicScript_Marko.Instance.gameCompleted)
        {
            GameLogicScript_Marko.Instance.GeneratePit(reset: true);
            GameLogicScript_Marko.Instance.gameCompleted = false;
        }
        else
        {
            GameLogicScript_Marko.Instance.GeneratePit();
        }
    }
}
