using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBehaviour : MonoBehaviour
{
    public int speed = 40;
    public bool mine;
    
    private float startRotationX;
    private float endRotationX;

    private AudioSource source;
    private float pitch;

    private Quaternion initialRotation;
    //Manager manager;
    //private bool holdingTreasure;
    private PlayerMovement player;


    // Start is called before the first frame update
    void Start()
    {
        //var managerGo = GameObject.FindGameObjectWithTag("Manager");
        //if (managerGo != null)
        //    manager = managerGo.GetComponent<Manager>();
        //else
        //    Debug.LogError("Manager not found!");

        //holdingTreasure = false;
        player = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();

        initialRotation = transform.localRotation;
        startRotationX = -45f;
        endRotationX = 35f;
        ResetPick();
        source = GetComponent<AudioSource>();
        if (source != null)
            pitch = source.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if (mine)
        {
            float difference = endRotationX - startRotationX;
            transform.localRotation = Quaternion.AngleAxis(startRotationX + ((Mathf.Sin(speed * Time.time) / 2 + 0.5f) * difference), Vector3.right);
        }
    }

    void ResetPick()
    {
        mine = false;
        transform.localRotation = initialRotation;
    }

    public void Mine()
    {
        mine = true;
    }

    public void StopMining()
    {
        mine = false;
        ResetPick();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Block block = other.gameObject.GetComponent<Block>();
            if (block != null && block.canDestroy)
            {
                if (source != null)
                {
                    source.Play();
                    source.pitch = pitch + Random.Range(-0.5f, 0.5f);
                }
                else
                {
                    Debug.LogError("audiosource not present in player!");
                }

                if (block.blockType == BlockType.Treasure && !player.holdingTreasure)
                {
                    block.AddHealth(-block.health);

                    //SendMessage Player x has taken treasure
                    GameLogicScript_Marko.Instance.RoomDataSyncController.SetClientWithTreasure(GameLogicScript_Marko.Instance.RoomDataSyncController.realtime.clientID);

                    player.TakeTreasure();
                }
                else
                {
                    block.health--; // this updates the block internal logic as well
                }
                
                //if (block.health < 1)
                //{
                //    //TODO: Gives points to player that killed it - gameobject that holds the tool.

                //    // Destroy is handled inside of the block for networking reasons now
                //    //Destroy(other.gameObject);
                //}

            }
            else
            {
                Debug.LogError("The block couldn't be found... even though it is there?");
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            PlayerMovement otherPlayer = other.gameObject.GetComponent<PlayerMovement>();
            if(otherPlayer != null)
            {
                if (otherPlayer.holdingTreasure)
                {
                    otherPlayer.LoseTreasure();
                    player.TakeTreasure();

                }
            }
        }

    }

}
