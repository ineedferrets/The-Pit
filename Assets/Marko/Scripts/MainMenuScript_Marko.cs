using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript_Marko : MonoBehaviour
{
    public InputField Username;

    public Slider RedSlider;
    public Slider GreenSlider;
    public Slider BlueSlider;

    public Text _numberOfPlayersText;
    public Text _playerNamesText;

    public TextMesh _winnerText;


    public GameObject Rules;
    public GameObject Controls;
    public GameObject UI;


    public void StartGame()
    {
        GameLogicScript_Marko.Instance.PlayerName = Username.text;
        GameLogicScript_Marko.Instance.GoToGameScene();
    }

    public void SetColor()
    {
        Color c = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1);
        GameLogicScript_Marko.Instance.PlayerSyncController.SetPlayerColor(c);
    }


    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        _numberOfPlayersText.text = "Number of players: " + numberOfPlayers.ToString();
    }

    public void SetPlayerNames(List<string> playerNames)
    {
        _playerNamesText.text = "";
        foreach (string name in playerNames)
        {
            _playerNamesText.text += name + "\n";
        }
    }

    public void ShowInfo()
    {
        Rules.SetActive(true);
        Controls.SetActive(true);
    }
    public void HideInfo()
    {
        Rules.SetActive(false);
        Controls.SetActive(false);
    }
    public void ShowUI()
    {
        UI.SetActive(true);
    }
    public void HideUI()
    {
        UI.SetActive(false);
    }

    public void SetWinnerText(int client, string name)
    {
        
        _winnerText.text = "The winner is " + name + ".";
        if (client == GameLogicScript_Marko.Instance.RoomDataSyncController.realtime.clientID)
        {
            _winnerText.text += "\n You did it!! Woooohoooo!!";
        } else
        {
            _winnerText.text += "\n You get a participation trophy";
        }
        

    }
}
