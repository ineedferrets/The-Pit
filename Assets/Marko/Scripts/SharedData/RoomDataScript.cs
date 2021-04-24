using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RoomDataScript : RealtimeComponent<RoomDataModel>
{

    protected override void OnRealtimeModelReplaced(RoomDataModel previousModel, RoomDataModel currentModel)
    {
            if (previousModel == null)
            {
                Debug.Log("No previous model");
            }

            if (currentModel == null)
            {
                Debug.Log("No current model");
            }

            LevelManagerScript.Instance.UIManagerScript.UpdateNumberOfPlayers(currentModel.numberOfPlayers);

    }

    public void PlayerConnected()
    {
        model.numberOfPlayers += 1;
    }

    public void PlayerDisconnected()
    {
        model.numberOfPlayers -= 1;
    }
}
