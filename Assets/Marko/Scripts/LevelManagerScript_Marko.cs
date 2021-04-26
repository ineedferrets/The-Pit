using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript_Marko : MonoBehaviour
{

    private static LevelManagerScript_Marko _instance;
    public static LevelManagerScript_Marko Instance { get { return _instance; } }


    public PlayerSyncController PlayerSyncController;
    public UIManagerScript_Marko UIManagerScript;



    public List<string> playerNames = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
