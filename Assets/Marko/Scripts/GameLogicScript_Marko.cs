using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Normal.Realtime;

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

    public GameObject StartCube;

    public PlayerMovement playerMovement;

    public List<PlayerMovement> allPlayers = new List<PlayerMovement>();

    public List<string> playerNames = new List<string>();


    public bool gameStarted;
    public bool gameCompleted;

    public AudioClip countdownTimer;
    public AudioClip mainTheme;
    public AudioClip gameOver;

    private bool _ownershipRequested;

    public GameObject TNT;
    public GameObject Claymore;
    private Coroutine _spawnTNTCoroutine;
    private float _timeToSpawnTNT = 1f;


    private bool quitGame = false;
    void Awake()
    {
        // get refs
        RoomDataSyncController = GetComponent<RoomDataSyncController>();
        StartCube = FindObjectOfType<StartGameBlock>().gameObject;
        MainMenuScript = FindObjectOfType<MainMenuScript_Marko>();
        TerrainGenerator = FindObjectOfType<GenerateTerrain>();

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

    private void OnDestroy()
    {
        // clear ownership if we own the gamemanager
        if (RoomDataSyncController.isOwnedLocallySelf)
        {
            RoomDataSyncController.ClearOwnership();
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

    IEnumerator SpawnTNT()
    {
        string TNTPrefab = "";
        string ClaymorePrefab = "";
        Vector3 spawnPos = new Vector3();

        if (TNT == null || Claymore == null)
        {
            Debug.LogError("TNT or Claymore prefab missing in gamemanager!");
        }
        else
        {
            TNTPrefab = TNT.name;
            ClaymorePrefab = Claymore.name;

        }
        // while we are the owners of this manager...
        while (RoomDataSyncController.isOwnedLocallySelf)
        {
            if (gameStarted)
            {
                // Calculate random spawn position
                spawnPos.x = Random.Range(TerrainGenerator.x_min, TerrainGenerator.x_max);
                spawnPos.y = Random.Range(TerrainGenerator.y_min, TerrainGenerator.y_max);
                spawnPos.z = Random.Range(TerrainGenerator.z_min, TerrainGenerator.z_max);
                // Spawn a TNT over the network
                Realtime.Instantiate(TNTPrefab, position: spawnPos, rotation: Quaternion.identity);

                // Calculate random spawn position
                spawnPos.x = Random.Range(TerrainGenerator.x_min, TerrainGenerator.x_max);
                spawnPos.y = Random.Range(TerrainGenerator.y_min, TerrainGenerator.y_max);
                spawnPos.z = Random.Range(TerrainGenerator.z_min, TerrainGenerator.z_max);
                // Spawn a TNT over the network
                Realtime.Instantiate(ClaymorePrefab, position: spawnPos, rotation: Quaternion.identity);

                // wait for a frame
                yield return new WaitForSeconds(_timeToSpawnTNT);


            }
        }
    }

    public void GoToGameScene()
    {

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
        RoomDataSyncController.SetClientWithTreasure(-1);
    }

    public void GoToLobby()
    {

        StartCube.SetActive(true);

        Vector3 position = new Vector3(-100, 1, 0);
        Vector3 randomness = new Vector3(Random.Range(-2f, 2f), Random.Range(0, 4), Random.Range(-2, 2));
        PlayerSyncController.gameObject.transform.position = position + randomness;
 

        MainMenuScript.ShowInfo();
        MainMenuScript.ShowUI();
    }

    public void GeneratePit(bool reset = false)
    {
        TerrainGenerator.GeneratePit(reset: reset);
    }

    /// <summary>
    /// Starts the coroutine for spawning bombs
    /// </summary>
    public void SpawnBombs()
    {
        // Start coroutine for TNT spawning during play
        _spawnTNTCoroutine = StartCoroutine(SpawnTNT());

    }

    public void SetGameCompleted(bool value)
    {
        gameCompleted = value;
        RoomDataSyncController.SetGameCompleted(gameCompleted);
    }

    public void TreasureTaken()
	{
        AudioSource source = GetComponent<AudioSource>();

        if (source != null)
        {
            source.clip = countdownTimer;
            source.Play();
        }
	}

    public void GameOver()
	{
        AudioSource source = GetComponent<AudioSource>();

        if (source != null)
		{
            source.clip = mainTheme;
            source.PlayDelayed(3.0f);
            source.PlayOneShot(gameOver);
		}

    }
}
