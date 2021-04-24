using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool canDestroy;
    public BlockType blockType;
    public int health; //hits to destroy block

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Tool")
    //    {
    //        health--;
    //        if (health < 1)
    //        {
    //            //TODO: Gives points to player that killed it - gameobject that holds the tool.
    //            Destroy(gameObject);
    //        }
    //    }
    //}

}

public enum BlockType 
{ 
    Floor, 
    Treasure
}