using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class BombSpawner : MonoBehaviour
{
    /// <summary>
    /// Used to instantiate over the network
    /// </summary>
    private RealtimeView _realtimeView;

    public GameObject TNT;
    public GameObject Claymore;
    private Coroutine _spawnTNTCoroutine;
    private float _timeToSpawnTNT = 1f;


    // Start is called before the first frame update
    void Start()
    {
        // Get the Realtime component on this game object
        _realtimeView = GetComponent<RealtimeView>();

    }

    IEnumerator SpawnTNT()
    {
        string TNTPrefab = "";
        string ClaymorePrefab = "";
        Vector3 spawnPos = new Vector3();
        int x_min = GameLogicScript_Marko.Instance.TerrainGenerator.x_min;
        int x_max = GameLogicScript_Marko.Instance.TerrainGenerator.x_max;
        int y_min = GameLogicScript_Marko.Instance.TerrainGenerator.y_min;
        int y_max = GameLogicScript_Marko.Instance.TerrainGenerator.y_max;
        int z_min = GameLogicScript_Marko.Instance.TerrainGenerator.z_min;
        int z_max = GameLogicScript_Marko.Instance.TerrainGenerator.z_max;

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
        while (_realtimeView.isOwnedLocallySelf)
        {
            if (GameLogicScript_Marko.Instance.gameStarted)
            {
                // Calculate random spawn position
                spawnPos.x = Random.Range(x_min, x_max);
                spawnPos.y = Random.Range(y_min, y_max);
                spawnPos.z = Random.Range(z_min, z_max);
                // Spawn a TNT over the network
                Realtime.Instantiate(TNTPrefab, position: spawnPos, rotation: Quaternion.identity);

                // Calculate random spawn position
                spawnPos.x = Random.Range(x_min, x_max);
                spawnPos.y = Random.Range(y_min, y_max);
                spawnPos.z = Random.Range(z_min, z_max);
                // Spawn a TNT over the network
                Realtime.Instantiate(ClaymorePrefab, position: spawnPos, rotation: Quaternion.identity);

                // wait for a frame
                yield return new WaitForSeconds(_timeToSpawnTNT);

            }
        }
    }

    public Coroutine SpawnBombs()
    {
        return StartCoroutine(SpawnTNT());
    }

}
