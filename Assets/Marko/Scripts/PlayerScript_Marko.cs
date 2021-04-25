using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class PlayerScript_Marko : MonoBehaviour
{

    public Realtime _realtime;

    private static PlayerScript_Marko _instance;
    public static PlayerScript_Marko Instance { get { return _instance; } }

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
        
        Vector3 position = new Vector3(Random.Range(-2f, 2f), Random.Range(1,5), Random.Range(-2,2));
        
        // Instantiate the CubePlayer for this client once we've successfully connected to the room
        GameObject player = Realtime.Instantiate("Player_Marko2",                 // Prefab name
                            position: position,          // Start 1 meter in the air
                            rotation: Quaternion.identity, // No rotation
                       ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
            preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                         useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.

        GameLogicScript_Marko.Instance.PlayerSyncController = player.GetComponent<PlayerSyncController>();
        GameLogicScript_Marko.Instance.RoomDataSyncController.IncreaseNumberOfPlayers();


        //LevelManagerScript_Marko.Instance.RoomDataSyncController.AddPlayerName(GameLogicScript_Marko.Instance.PlayerName);

    }


}
