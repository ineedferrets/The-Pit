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


    public GameObject Rules;
    public GameObject Controls;
    public GameObject UI;

    /*
    private static MainMenuScript_Marko _instance;
    public static MainMenuScript_Marko Instance { get { return _instance; } }

    void Awake()
    {

        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    */

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
}
