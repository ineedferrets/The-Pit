using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public Text countdownText;
    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.holdingTreasure)
        {
            countdownText.text = "Hold It For: " + ((int)player.countdown).ToString();
            
        }
        else
        {
            countdownText.text = "";
        }
        
    }
}
