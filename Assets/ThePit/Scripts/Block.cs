using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool canDestroy;
    public BlockType blockType;
    //hits to destroy block
    public int health { get { return this._health; } set { SetHealth(value); } }
    [SerializeField]
    private int _health = 3;

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

    public void AddHealth(int amount)
    {
        _health += amount;
        if (_health <= 0)
        {
            _health = 0;
            DestroyBlock();
        }
    }

    private void SetHealth(int newHealth)
    {
        _health = newHealth;
        if (_health <= 0)
        {
            _health = 0;
            DestroyBlock();
        }

        if (health >= 3)
            GetComponent<Renderer>().material.SetFloat("_BlendAmount", 0.0f);
        else if (health == 2)
            GetComponent<Renderer>().material.SetFloat("_BlendAmount", 0.2f);
        else if (health == 1)
            GetComponent<Renderer>().material.SetFloat("_BlendAmount", 1.0f);

    }

    protected virtual void DestroyBlock()
    {
        var terrainGenerator = transform.parent.GetComponent<GenerateTerrain>();
        if (terrainGenerator != null)
        {
            if (terrainGenerator.replicateInNetwork)
            {
                Normal.Realtime.Realtime.Destroy(this.gameObject);

            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}

public enum BlockType 
{ 
    Floor, 
    Bedrock,
    Treasure
}