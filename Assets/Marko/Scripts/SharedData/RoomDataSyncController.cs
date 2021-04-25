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
                currentModel.sceneName = "LobbyScene";
            }

            
            UpdateNumberOfPlayers();
            UpdatePlayerNames();
            //UpdateScene();

            currentModel.numberOfPlayersDidChange += NumberOfPlayersDidChange;
            currentModel.playerNamesDidChange += PlayerNamesDidChange;
            currentModel.sceneNameDidChange += SceneNameDidChange;
        }

    }

    private void NumberOfPlayersDidChange(RoomDataModel model, int value)
    {
        UpdateNumberOfPlayers();
    }

    private void PlayerNamesDidChange(RoomDataModel model, string value)
    {
        //UpdatePlayerNames();
    }

    private void SceneNameDidChange(RoomDataModel model, string value)
    {
        UpdateScene();
    }

    private void UpdateNumberOfPlayers()
    {
        GameLogicScript_Marko.Instance.MainMenuScript.SetNumberOfPlayers(model.numberOfPlayers);
    }


    private void UpdatePlayerNames()
    {
        List<string> names = new List<string>();
        foreach(string name in model.playerNames.Split(';'))
        {
            names.Add(name);
        }

        GameLogicScript_Marko.Instance.MainMenuScript.SetPlayerNames(names);
    }

    private void UpdateScene()
    {
        switch (model.sceneName)
        {
            case "LobbyScene":
                GameLogicScript_Marko.Instance.GoToLobby();
                break;
            case "GameScene":
                GameLogicScript_Marko.Instance.GoToGameScene();
                break;
            case "GameOverScene":
                GameLogicScript_Marko.Instance.GoToGameOverScene();
                break;
            default: 
                Debug.Log("Unknown scene");  
                break;
        }
        
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

    public void SetSceneName(string name)
    {
        model.sceneName = name;
    }


}
