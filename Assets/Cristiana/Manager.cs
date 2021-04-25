using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerWithTreasure;
    public GameObject treasure;

    public bool holdingTreasure;
    public float timer;

    float countdown;

    // Start is called before the first frame update
    void Start()
    {
        countdown = timer;
        holdingTreasure = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingTreasure)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && playerWithTreasure != null)
            {
                //Win(player);
                Debug.Log("Win");
            }
        }
    }

    public void createPlayer(int x, int y, int z)
    {
        Instantiate(player, gameObject.transform);
    }

    public void PlayerCaughtTreasure(GameObject player)
    {
        //treasure = block;
        playerWithTreasure = player;
        holdingTreasure = true;
        countdown = timer;
    }
}
