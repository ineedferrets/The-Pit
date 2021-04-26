using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgainBlock : Block
{

    protected override void DestroyBlock()
    {
        //GameLogicScript_Marko.Instance.RoomDataSyncController.SetSceneName("LobbyScene");
        GameLogicScript_Marko.Instance.GoToLobby();
        GameLogicScript_Marko.Instance.SetGameCompleted(true);
    }

}
