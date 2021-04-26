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
            previousModel.sceneNameDidChange -= SceneNameDidChange;
            previousModel.winnerNameDidChange -= WinnerNameDidChange;
            previousModel.clientWithTreasureDidChange -= ClientWithTreasureDidChange;
            previousModel.gameCompletedDidChange -= GameCompletedDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.numberOfPlayers = 0;
                currentModel.playerNames = "";
                currentModel.sceneName = "LobbyScene";
                currentModel.winnerName = "";
                currentModel.clientWithTreasure = -1;
                currentModel.gameCompleted = false;
            }
            
            
            UpdateNumberOfPlayers();
            UpdatePlayerNames();

            currentModel.numberOfPlayersDidChange += NumberOfPlayersDidChange;
            currentModel.playerNamesDidChange += PlayerNamesDidChange;
            currentModel.sceneNameDidChange += SceneNameDidChange;
            currentModel.winnerNameDidChange += WinnerNameDidChange;
            currentModel.clientWithTreasureDidChange += ClientWithTreasureDidChange;
            currentModel.gameCompletedDidChange += GameCompletedDidChange;
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

    private void WinnerNameDidChange(RoomDataModel model, string value)
    {
        UpdateWinnerName();
    }

    private void ClientWithTreasureDidChange(RoomDataModel model, int value)
    {
        UpdateClientWithTreasure();
    }

    private void GameCompletedDidChange(RoomDataModel model, bool value)
    {
        UpdateGameCompleted(model.gameCompleted);
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

    private void UpdateWinnerName()
    {
        int clientID = int.Parse(model.winnerName.Split(';')[0]);
        string winnerName = model.winnerName.Split(';')[1];

        GameLogicScript_Marko.Instance.MainMenuScript.SetWinnerText(clientID, winnerName);    
    }

    private void UpdateClientWithTreasure()
    {
        
        foreach(PlayerMovement pm in GameLogicScript_Marko.Instance.allPlayers)
        {
            if (pm.clientID == model.clientWithTreasure)
            {
                pm.TakeTreasure();
            } 
            else
            {
                pm.LoseTreasure();
            }
        }
    }

    private void UpdateGameCompleted(bool value)
    {
        GameLogicScript_Marko.Instance.gameCompleted = value;
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

    public void SetWinnerName(string name)
    {
        model.winnerName =  realtime.clientID+";"+ name;
        // Set the win flag to true
        SetGameCompleted(true);
    }

    public void SetClientWithTreasure(int id)
    {
        model.clientWithTreasure = id;
    }

    public void SetGameCompleted(bool value)
    {
        model.gameCompleted = value;
    }

}
