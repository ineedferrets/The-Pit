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
    Manager manager;
    private bool caughtTreasure;


    // Start is called before the first frame update
    void Start()
    {
        var managerGo = GameObject.FindGameObjectWithTag("Manager");
        if (managerGo != null)
            manager = managerGo.GetComponent<Manager>();
        else
            Debug.LogError("Manager not found!");
        caughtTreasure = false;

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
    {//TODO
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

                block.health--; // this updates the block internal logic as well
                if (block.health < 1)
                {
                    //TODO: Gives points to player that killed it - gameobject that holds the tool.

                    // Destroy is handled inside of the block for networking reasons now
                    //Destroy(other.gameObject);
                }

            }
            else if (block != null && block.blockType==BlockType.Treasure && !caughtTreasure)
            {
                caughtTreasure = true;
                GameObject player = gameObject.transform.parent.gameObject;
                if (manager != null)
                    manager.PlayerCaughtTreasure(player);
                block.AddHealth(-5);
              
            }
            else
            {
                Debug.LogError("The block couldn't be found... even though it is there?");
            }
        }

    }

}
