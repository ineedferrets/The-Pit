using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript_Marko : MonoBehaviour
{
    
    private static GameLogicScript_Marko _instance;

    public static GameLogicScript_Marko Instance { get { return _instance; } }


    public string PlayerName;
    public Color PlayerColor;

    void Awake()
    {

        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            ExitGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
