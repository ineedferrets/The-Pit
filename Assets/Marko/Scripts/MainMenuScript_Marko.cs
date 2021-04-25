using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript_Marko : MonoBehaviour
{
    public InputField Username;

    public Slider RedSlider;
    public Slider GreenSlider;
    public Slider BlueSlider;

    public Text _numberOfPlayersText;
    public Text _playerNamesText;

    public void StartGame()
    {
        GameLogicScript_Marko.Instance.PlayerName = Username.text;
        SceneManager.LoadScene("GameScene");
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
}
