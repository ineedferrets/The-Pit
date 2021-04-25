using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public BombType bombType;
    public float delay;
    public float blastRadius;
    public float force;

    public GameObject particlesEffect;

    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }


    void Explode()
    {
        //Show effect
        Instantiate(particlesEffect, transform.position, transform.rotation);

        //Get nearby objects
        Collider[] objects = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearbyObject in objects) 
        {
            if (nearbyObject.gameObject.tag == "Ground")
            {
                //Destroy blocks that are allowed
                Block block = nearbyObject.GetComponent<Block>();
                if (block.canDestroy && block.blockType!=BlockType.Treasure)
                {
                    block.AddHealth(-5);
                }
            }
            else if (nearbyObject.gameObject.tag == "Player")
            {
                //Add force 
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, blastRadius);
                }

                //TODO: Remove points

            }
        }


        //Remove grenade
        Destroy(gameObject);
    }
}

public enum BombType
{
    TNT,
    Claymore
}