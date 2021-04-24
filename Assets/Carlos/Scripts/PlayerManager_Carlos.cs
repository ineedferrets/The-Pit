using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerManager_Carlos : MonoBehaviour
{
    private Realtime _realtime;
    [SerializeField]
    private string _prefabName;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        // Instantiate the My Player prefab for this client once we've successfully connected to the room
        Realtime.Instantiate(_prefabName,      // Prefab name
                                 ownedByClient: true,      // Make sure the RealtimeView on this prefab is owned by this client
                      preventOwnershipTakeover: true,      // Prevent other clients from calling RequestOwnership() on the root RealtimeView or any children.
            destroyWhenOwnerOrLastClientLeaves: true,      // Instruct the server to destroy this prefab when the owner (this client) disconnects. (This is true by default, but added to the example for clarity)
                                   useInstance: realtime); // Use the instance of Realtime that fired the didConnectToRoom event.
    }
}
