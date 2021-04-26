using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class C_GenerateTerrain : MonoBehaviour
{
    public GameObject floorNormalBlock, floorBedrockBlock, treasure;
    public GameObject wallNormal;
    public string normalBlockNetworkedName;

    public int x_min, x_max, y_min, y_max, z_min, z_max;
    public int bedrockStartPoint, bedrockSecondLayer;

    private int chest_x, chest_y, chest_z;
    //Manager manager;

    /// <summary>
    /// Replicates in the network the cube spawning
    /// </summary>
    public bool replicateInNetwork { get; }
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
        //manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        // Generate floor on start only when not in networking
        if (!_replicateInNetwork)
            GenerateChestPos();
            GenerateFloor(_replicateInNetwork);

        //for the walls N/S
        GameObject wallN = Instantiate(wallNormal, new Vector3(0, 0, z_min-1), Quaternion.identity);
        GameObject wallS = Instantiate(wallNormal, new Vector3(0, 0, z_max+1), Quaternion.identity);
        
        GameObject wallW = Instantiate(wallNormal, new Vector3(x_min-1, 0, 0), Quaternion.identity);
        wallW.transform.Rotate(0, 90, 0);
        GameObject wallE = Instantiate(wallNormal, new Vector3(x_max+1, 0, 0), Quaternion.identity);
        wallE.transform.Rotate(0, 90, 0);

        //manager.createPlayer(0, 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateFloor(bool replicate = false)
    {
        
        for (int y = y_min; y <= y_max; y++)
        {
            for (int x = x_min; x <= x_max; x++)
            {
                for (int z = z_min; z <= z_max; z++)
                {
                    GameObject block = PickBlock(x, y, z);

                    GameObject floor = null;
                    floor = Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
                    floor.transform.parent = gameObject.transform;
                }
            }
        }


        //for (int x = x_min; x <= x_max; x++)
        //{
        //    for (int y = y_min; y <= y_max; y++)
        //    {
        //        for (int z = z_min; z <= z_max; z++)
        //        {
        //            GameObject floor = null;
        //            // Local instantiation, no networking
        //            if (!replicate)
        //            {
        //                floor = Instantiate(floorNormalBlock, new Vector3(x, y, z), Quaternion.identity);
        //            }
        //            // Instantiating over the network
        //            else if (replicate && !string.IsNullOrEmpty(normalBlockNetworkedName))
        //            {
        //                floor = Realtime.Instantiate(normalBlockNetworkedName,
        //                    position: new Vector3(x, y, z),
        //                    rotation: Quaternion.identity,
        //                    ownedByClient: false,      
        //                    preventOwnershipTakeover: false,      
        //                    destroyWhenOwnerOrLastClientLeaves: true,      // Instruct the server to destroy this prefab when the owner (this client) disconnects. (This is true by default, but added to the example for clarity)
        //                    useInstance: _realtime
        //                    );                            
        //            }
        //            floor.transform.parent = gameObject.transform;

        //        }
        //    }
        //}

    }

    private void GenerateChestPos()
    {
        chest_x = Random.Range(x_min, x_max);
        chest_z = Random.Range(z_min, z_max);
        chest_y = Random.Range(y_min+1, y_min+4); //guaranted in the last three rows (before the very last)
    }

    private GameObject PickBlock(int x, int y, int z)
    {
        if (y == y_min)
        {
            return floorBedrockBlock;
        }
        else if (y < bedrockStartPoint)
        {
            if (x == chest_x && y == chest_y && z == chest_z) //treasure block
            {
                return treasure;
            }
            else if (x == chest_x && y == chest_y+1 && z == chest_z) //block above treasure
            {
                return floorNormalBlock;
            }

            if (y > bedrockSecondLayer && Random.value > 0.95)//otherwise can be bedrock - %5 percent chance (1 - 0.95 is 0.05)
            {
                return floorBedrockBlock;
            }
            else if (y <= bedrockSecondLayer && Random.value > 0.85) //higher if last area - %15 percent chance
            {
                return floorBedrockBlock;
            }
            
        }
        return floorNormalBlock;
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        if (_replicateInNetwork)
        {
            GenerateFloor(_replicateInNetwork);
        }
    }

}
