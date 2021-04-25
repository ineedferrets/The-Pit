using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class RoomDataSyncController : RealtimeComponent<RoomDataModel>
{
    
    protected override void OnRealtimeModelReplaced(RoomDataModel previousModel, RoomDataModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.numberOfPlayersDidChange -= NumberOfPlayersDidChange;
            previousModel.playerNamesDidChange -= PlayerNamesDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.numberOfPlayers = 0;
                currentModel.playerNames = "";
            }

            UpdateNumberOfPlayers();
            UpdatePlayerNames();


            currentModel.numberOfPlayersDidChange += NumberOfPlayersDidChange;
            currentModel.playerNamesDidChange += PlayerNamesDidChange;
        }

    }

    private void NumberOfPlayersDidChange(RoomDataModel model, int value)
    {
        UpdateNumberOfPlayers();
    }

    private void PlayerNamesDidChange(RoomDataModel model, string value)
    {
        UpdatePlayerNames();
    }


    private void UpdateNumberOfPlayers()
    {
        LevelManagerScript_Marko.Instance.UIManagerScript.UpdateNumberOfPlayers(model.numberOfPlayers);
    }


    private void UpdatePlayerNames()
    {
        List<string> names = new List<string>();
        foreach(string name in model.playerNames.Split(';'))
        {
            names.Add(name);
        }

        LevelManagerScript_Marko.Instance.UIManagerScript.UpdatePlayerNames(names);
    }
    public void IncreaseNumberOfPlayers()
    {
        model.numberOfPlayers++;
    }

    public void DecreaseNumberOfPlayers()
    {
        model.numberOfPlayers--;
    }

    public void AddPlayerName(string name)
    {
        model.playerNames += name + ";";
    }


}
