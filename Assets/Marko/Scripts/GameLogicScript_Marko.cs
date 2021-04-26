using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicScript_Marko : MonoBehaviour
{
    
    private static GameLogicScript_Marko _instance;
    public static GameLogicScript_Marko Instance { get { return _instance; } }

    public string PlayerName = "I forgot to choose username";
    public Color PlayerColor;

    public RoomDataSyncController RoomDataSyncController;
    public PlayerSyncController PlayerSyncController;
    public MainMenuScript_Marko MainMenuScript;
    public GenerateTerrain TerrainGenerator;

    public PlayerMovement playerMovement;

    public List<PlayerMovement> allPlayers = new List<PlayerMovement>();

    public List<string> playerNames = new List<string>();


    private bool quitGame = false;
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

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            ExitGame();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            RoomDataSyncController.SetClientWithTreasure(RoomDataSyncController.realtime.clientID);
        }
    }

    public void ExitGame()
    {
        if (quitGame == false)
        {
            RoomDataSyncController.DecreaseNumberOfPlayers();
            quitGame = true;
            StartCoroutine(ExitAfterTime(1)); //without this model doesn't register the change before quiting the app

        }
        
    }

    IEnumerator ExitAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Application.Quit();
    }

    public void GoToGameScene()
    {
        //GeneratePit();

        Vector3 position = new Vector3(-5, 5, -5);
        Vector3 randomness = new Vector3(Random.Range(-2f, 2f), Random.Range(2, 4), Random.Range(-2, 2));

        PlayerSyncController.gameObject.transform.position = position + randomness;


        MainMenuScript.HideInfo();
        MainMenuScript.HideUI();
    }

    public void GoToGameOverScene()
    {

        Vector3 position = new Vector3(100, 1, 0);
        Vector3 randomness = new Vector3(Random.Range(-2f, 2f), Random.Range(0, 4), Random.Range(-2, 2));
        PlayerSyncController.gameObject.transform.position = position + randomness;

    }

    public void GoToLobby()
    {

        Vector3 position = new Vector3(-100, 1, 0);
        Vector3 randomness = new Vector3(Random.Range(-2f, 2f), Random.Range(0, 4), Random.Range(-2, 2));
        PlayerSyncController.gameObject.transform.position = position + randomness;
 

        MainMenuScript.ShowInfo();
        MainMenuScript.ShowUI();
    }

    public void GeneratePit()
    {
        TerrainGenerator.GeneratePit();
    }
}
