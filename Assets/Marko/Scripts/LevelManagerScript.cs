using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{

    private static LevelManagerScript _instance;
    public static LevelManagerScript Instance { get { return _instance; } }


    public RoomDataScript RoomDataScript;
    public UIManagerScript UIManagerScript;

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
