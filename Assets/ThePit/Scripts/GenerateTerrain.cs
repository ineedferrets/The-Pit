using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject floorNormalBlock;
    public GameObject wallNormal;
    public string normalBlockNetworkedName;

    public int x_min, x_max, y_min, y_max, z_min, z_max;

    /// <summary>
    /// Replicates in the network the cube spawning
    /// </summary>
    [SerializeField]
    private bool _replicateInNetwork;

    /// <summary>
    /// Used to instantiate over the network
    /// </summary>
    private Realtime _realtime;

    // Called before start
    private void Awake()
    {
        if (_replicateInNetwork)
        {
            // Get the Realtime component on this game object
            _realtime = FindObjectOfType<Realtime>();

            if (_realtime != null)
            {
                // Notify us when Realtime successfully connects to the room
                _realtime.didConnectToRoom += DidConnectToRoom;

            }

        }


    }

    // Start is called before the first frame update
    void Start()
    {
        // Generate floor on start only when not in networking
        if (!_replicateInNetwork)
            GenerateFloor(_replicateInNetwork);

        //for the walls N/S
        GameObject wallN = Instantiate(wallNormal, new Vector3(0, 0, z_min-1), Quaternion.identity);
        GameObject wallS = Instantiate(wallNormal, new Vector3(0, 0, z_max+1), Quaternion.identity);
        
        GameObject wallW = Instantiate(wallNormal, new Vector3(x_min-1, 0, 0), Quaternion.identity);
        wallW.transform.Rotate(0, 90, 0);
        GameObject wallE = Instantiate(wallNormal, new Vector3(x_max+1, 0, 0), Quaternion.identity);
        wallE.transform.Rotate(0, 90, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateFloor(bool replicate = false)
    {
        for (int x = x_min; x <= x_max; x++)
        {
            for (int y = y_min; y <= y_max; y++)
            {
                for (int z = z_min; z <= z_max; z++)
                {
                    GameObject floor = null;
                    // Local instantiation, no networking
                    if (!replicate)
                    {
                        floor = Instantiate(floorNormalBlock, new Vector3(x, y, z), Quaternion.identity);
                    }
                    // Instantiating over the network
                    else if (replicate && normalBlockNetworkedName != null)
                    {
                        floor = Realtime.Instantiate(normalBlockNetworkedName, 
                            ownedByClient: false,      
                            preventOwnershipTakeover: false,      
                            destroyWhenOwnerOrLastClientLeaves: true,      // Instruct the server to destroy this prefab when the owner (this client) disconnects. (This is true by default, but added to the example for clarity)
                            useInstance: _realtime);                            
                    }
                    floor.transform.parent = gameObject.transform;

                }
            }
        }

    }

    private void DidConnectToRoom(Realtime realtime)
    {
        if (_replicateInNetwork)
        {
            GenerateFloor(_replicateInNetwork);
        }
    }

}
