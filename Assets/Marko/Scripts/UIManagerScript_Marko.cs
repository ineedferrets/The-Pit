using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript_Marko : MonoBehaviour
{

    public Text _numberOfPlayersText;
    public Text _playerNamesText;
    public void UpdateNumberOfPlayers(int numberOfPlayers)
    {
        _numberOfPlayersText.text = "Number of players: " + numberOfPlayers.ToString();
    }

    public void UpdatePlayerNames(List<string> playerNames)
    {
        _playerNamesText.text = "";
        foreach(string name in playerNames)
        {
            _playerNamesText.text += name + "\n";
        }
    }
}
