using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript_Marko : MonoBehaviour
{

    private static LevelManagerScript_Marko _instance;
    public static LevelManagerScript_Marko Instance { get { return _instance; } }


    public RoomDataSyncController RoomDataSyncController;
    public UIManagerScript_Marko UIManagerScript;



    public List<string> playerNames;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            playerNames = new List<string>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
