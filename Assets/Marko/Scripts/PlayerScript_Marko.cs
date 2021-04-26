using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class PlayerScript_Marko : MonoBehaviour
{

    public Realtime _realtime;

    private static PlayerScript_Marko _instance;
    public static PlayerScript_Marko Instance { get { return _instance; } }

    public string PlayerPrefabName;
    public GameObject GameManagerPrefab;

    void Awake()
    {

        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _realtime.didConnectToRoom += DidConnectToRoom;
        }
        else
        {
            Destroy(gameObject);
        }
           }

    private void DidConnectToRoom(Realtime realtime)
    {

        Vector3 position = new Vector3(-100, 1, 0);
        Vector3 randomness = new Vector3(Random.Range(-2f, 2f), Random.Range(0, 4), Random.Range(-2, 2));

        if (string.IsNullOrEmpty(PlayerPrefabName))
        {
            Debug.LogError("The Player prefab name hasn't been set! Aborting network instantiation");
            return;
        }

        if (GameManagerPrefab == null)
        {
            Debug.LogError("The gamemanager prefab hasn't been set! Aborting network instantiation");
            return;
        }


        // Instantiate the CubePlayer for this client once we've successfully connected to the room
        GameObject gameManager = Realtime.Instantiate(GameManagerPrefab.name,                 // Prefab name                            
                       ownedByClient: false               // Make sure the RealtimeView on this prefab is owned by this client
                         );


        // Instantiate the CubePlayer for this client once we've successfully connected to the room
        GameObject player = Realtime.Instantiate(PlayerPrefabName,                 // Prefab name
                            position: position + randomness,          // Start 1 meter in the air
                            rotation: Quaternion.identity, // No rotation
                       ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
            preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                         useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.


        GameLogicScript_Marko.Instance.playerMovement = player.GetComponent<PlayerMovement>();
        GameLogicScript_Marko.Instance.PlayerSyncController = player.GetComponent<PlayerSyncController>();
        GameLogicScript_Marko.Instance.RoomDataSyncController.IncreaseNumberOfPlayers();

        GameLogicScript_Marko.Instance.playerMovement.clientID = realtime.clientID;
    }


}
