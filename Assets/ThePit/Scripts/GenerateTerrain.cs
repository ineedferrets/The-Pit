using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject floorNormalBlock, floorBedrockBlock, treasure;
    public GameObject wallNormal;

    public Light light;
    public LightType lightType = LightType.Point;

    public int x_min, x_max, y_min, y_max, z_min, z_max;
    public int bedrockStartPoint, bedrockSecondLayer;

    private int chest_x, chest_y, chest_z;
    //Manager manager;

    /// <summary>
    /// For networking, call RealtimeGenerateTerrain.SetGenerationStarted
    /// </summary>
    public bool generationStarted;
    /// <summary>
    /// For networking, call RealtimeGenerateTerrain.SetGenerationCompleted
    /// </summary>
    public bool generationCompleted;

    /// <summary>
    /// Replicates in the network the cube spawning
    /// </summary>
    public bool replicateInNetwork { get; }
    /// <summary>
    /// Do we need to network code at all?
    /// </summary>
    [SerializeField]
    private bool _replicateInNetwork;

    /// <summary>
    /// Used to instantiate over the network
    /// </summary>
    private Realtime _realtime;

    /// <summary>
    /// Handles sync of generate terrain info
    /// </summary>
    private RealtimeGenerateTerrain _realtimeGenerateTerrain;


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

            // Get ref for syncing
            _realtimeGenerateTerrain = GetComponent<RealtimeGenerateTerrain>();
            if (_realtimeGenerateTerrain != null)
            {
                // Nothing here?

            }

        }
        else
        {

            //var auxGo = GameObject.FindGameObjectWithTag("Manager");
            //if (auxGo != null)
                //manager = auxGo.GetComponent<Manager>();


        }


    }

    // Start is called before the first frame update
    void Start()
    {

        GeneratePit();

    }

    public void GeneratePit()
    {
        // Generate floor on start only when not in networking
        if (!_replicateInNetwork)
        {
            GenerateChestPos();
            GenerateFloor(_replicateInNetwork);
        }

        float xQuadScale = 1.08f * (x_max - x_min);
        float zQuadScale = 1.08f * (z_max - z_min);

        //for the walls N/S
        GameObject wallN = Instantiate(wallNormal, new Vector3(0, 0, z_min - 0.6f), Quaternion.identity);
        wallN.transform.localScale = new Vector3(xQuadScale, wallN.transform.localScale.y, wallN.transform.localScale.z);
        GameObject wallS = Instantiate(wallNormal, new Vector3(0, 0, z_max + 0.6f), Quaternion.identity);
        wallS.transform.localScale = new Vector3(xQuadScale, wallN.transform.localScale.y, wallN.transform.localScale.z);

        GameObject wallW = Instantiate(wallNormal, new Vector3(x_min - 0.6f, 0, 0), Quaternion.identity);
        wallW.transform.localScale = new Vector3(zQuadScale, wallN.transform.localScale.y, wallN.transform.localScale.z);
        wallW.transform.Rotate(0, 90, 0);
        GameObject wallE = Instantiate(wallNormal, new Vector3(x_max + 0.6f, 0, 0), Quaternion.identity);
        wallE.transform.localScale = new Vector3(zQuadScale, wallN.transform.localScale.y, wallN.transform.localScale.z);
        wallE.transform.Rotate(0, 90, 0);

        GameObject roof = Instantiate(wallNormal, new Vector3(0, y_max + 10.0f, 0.0f), Quaternion.identity);
        roof.transform.Rotate(90, 0, 0);
        roof.transform.localScale = new Vector3(xQuadScale, zQuadScale, 0.0f);

        GameObject lightObject = new GameObject("Light");
        lightObject.transform.position = roof.transform.position - new Vector3(0.0f, 3.0f, 0.0f);
        light = lightObject.AddComponent<Light>();
        light.type = lightType;
        light.intensity = 30;
        lightObject.active = false;
        lightObject.active = true;

        //if(manager != null)
        //manager.createPlayer(0, 3, 0);

    }

    private void GenerateFloor(bool replicate = false)
    {
        // If somehow, the generation has started or is completed (maybe by another client over the network), bail
        if (generationStarted || generationCompleted)
            return;

        // If we are the first to generate terrain, mark the generation as started
        if (replicate && _realtimeGenerateTerrain)
            _realtimeGenerateTerrain.SetGenerationStarted(true);
        else
            generationStarted = true;

        for (int x = x_min; x <= x_max; x++)
        {
            for (int y = y_min; y <= y_max; y++)
            {
                for (int z = z_min; z <= z_max; z++)
                {
                    GameObject floor = null;
                    GameObject block = PickBlock(x, y, z);
                    // Parse block name for network
                    string blockName = "";
                    if (block != null)
                        blockName = block.name + "_Network";

                    // Local instantiation, no networking
                    if (!replicate)
                    {
                        floor = Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
                    }
                    // Instantiating over the network
                    else if (replicate && !string.IsNullOrEmpty(blockName))
                    {
                        // Instantiate network
                        floor = Realtime.Instantiate(blockName,
                            position: new Vector3(x, y, z),
                            rotation: Quaternion.identity,
                            ownedByClient: false,      
                            preventOwnershipTakeover: false,      
                            destroyWhenOwnerOrLastClientLeaves: true,      // Instruct the server to destroy this prefab when the owner (this client) disconnects. (This is true by default, but added to the example for clarity)
                            useInstance: _realtime
                            );                            
                    }

                    // Parent block to terrain generator gameobject
                    floor.transform.parent = gameObject.transform;

                }
            }
        }

        // Mark the generation as finished
        if (replicate && _realtimeGenerateTerrain)
            _realtimeGenerateTerrain.SetGenerationCompleted(true);
        else
            generationCompleted = true;

    }

    private void GenerateChestPos()
    {
        chest_x = Random.Range(x_min, x_max);
        chest_z = Random.Range(z_min, z_max);
        chest_y = Random.Range(y_min + 1, y_min + 4); //guaranted in the last three rows (before the very last)
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
            else if (x == chest_x && y == chest_y + 1 && z == chest_z) //block above treasure
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
            // Attempt to get ownership...
            _realtimeGenerateTerrain.RequestOwnership();
            // If we don't own the generator, bail
            if (!_realtimeGenerateTerrain.isOwnedLocallySelf)
                return;

            UpdateGenerationFlags(_realtimeGenerateTerrain);            
            GenerateChestPos();
            GenerateFloor(_replicateInNetwork);
        }
    }

    private void UpdateGenerationFlags(RealtimeGenerateTerrain realtimeGenerator)
    {
        if (realtimeGenerator != null)
        {
            generationStarted = realtimeGenerator.GetGenerationStarted();
            generationCompleted = realtimeGenerator.GetGenerationCompleted();
        }
        else
        {
            Debug.LogError("Attepmting to update terrain generation flags but realtimeGenerator not found!");
        }
    }

}
