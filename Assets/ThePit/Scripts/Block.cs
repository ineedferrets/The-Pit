using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Block : MonoBehaviour
{
    public bool canDestroy;
    public BlockType blockType;
    //hits to destroy block
    public int health { get { return this._health; } set { SetHealth(value); } }
    [SerializeField]
    private int _health = 3;

    // Realtime references
    private RealtimeHealth _realtimeHealth;

    private void Awake()
    {
        // Get reference to realtimeview
        _realtimeHealth = GetComponent<RealtimeHealth>();
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
        bool destroyBlock = false;

        _health = newHealth;

        if (_health <= 0)
        {
            _health = 0;
            destroyBlock = true;
        }

        // Attempt to sync network
        if (_realtimeHealth != null)
            _realtimeHealth.SetHealth(_health);

        // Update drawing before destroying
        if (health >= 3)
            GetComponent<Renderer>().material.SetFloat("_BlendAmount", 0.0f);
        else if (health == 2)
            GetComponent<Renderer>().material.SetFloat("_BlendAmount", 0.2f);
        else if (health == 1)
            GetComponent<Renderer>().material.SetFloat("_BlendAmount", 1.0f);

        // Do we need to destroy block?
        if (destroyBlock)
            DestroyBlock();

    }

    protected virtual void DestroyBlock()
    {
        if (_realtimeHealth != null)
        {
            Realtime.Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}

public enum BlockType 
{ 
    Floor, 
    Bedrock,
    Treasure
}