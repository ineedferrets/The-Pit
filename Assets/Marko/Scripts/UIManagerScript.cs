using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Text _numberOfPlayersText;

    public void UpdateNumberOfPlayers(int numberOfPlayers)
    {
        _numberOfPlayersText.text = "Number of players: " + numberOfPlayers.ToString();
    }
}
