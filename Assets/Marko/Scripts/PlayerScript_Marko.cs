using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerScript_Marko : MonoBehaviour
{

    public Realtime _realtime;
    public PlayerSyncScript PlayerSyncScript;

    public GameObject Player = null;

    public Color Color = default;
    public Color PreviousColor = default;

    // Start is called before the first frame update
    void Awake()
    {
        _realtime.didConnectToRoom += DidConnectToRoom;
        
    }

    // Update is called once per frame
    private void DidConnectToRoom(Realtime realtime)
    {
        // Instantiate the CubePlayer for this client once we've successfully connected to the room
        Player = Realtime.Instantiate("Player_Marko",                 // Prefab name
                            position: Vector3.up,          // Start 1 meter in the air
                            rotation: Quaternion.identity, // No rotation
                       ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
            preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                         useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.

        LevelManagerScript_Marko.Instance.RoomDataSyncController.IncreaseNumberOfPlayers();
        LevelManagerScript_Marko.Instance.RoomDataSyncController.AddPlayerName(GameLogicScript_Marko.Instance.PlayerName);
        
    }
}
