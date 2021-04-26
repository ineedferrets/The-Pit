using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Bombs : MonoBehaviour
{
    public BombType bombType;
    public float delay;
    public float blastRadius;
    public float force;

    public GameObject particlesEffect;
    public List<Renderer> renderers;
    public Collider collider;
    public AudioClip explosionSoundFx;
    public AudioClip fuseSoundFx;
    public AudioSource source;

    public float countdown { get; private set; }
    bool hasExploded = false;

    [SerializeField]
    private float destroyGameObjectDelay = 2.0f;
    private bool toBeDestroyed = false;

    private RealtimeView _realtimeView;

    private void Awake()
    {
        _realtimeView = GetComponent<RealtimeView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        if (GetComponent<Renderer>() != null)
            renderers.Add(GetComponent<Renderer>());
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        if (GetComponent<Collider>() != null)
            collider = GetComponent<Collider>();
        else
            collider = GetComponentInChildren<Collider>();

        if (bombType == BombType.TNT)
		{
            source.clip = fuseSoundFx;
            source.loop = true;
            source.Play();
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (toBeDestroyed)
            destroyGameObjectDelay -= Time.deltaTime;

        countdown -= Time.deltaTime;

        if (bombType == BombType.Claymore && countdown > 0.9f && countdown < 1.1f)
		{
            source.PlayOneShot(explosionSoundFx);
		}

        if (countdown <= 0f && !hasExploded)
        {

            Explode();
            hasExploded = true;
        }

        if (destroyGameObjectDelay <= 0.0f)
		{
            // online?
            if (_realtimeView != null && _realtimeView.isOwnedLocallySelf)
            {
                Realtime.Destroy(gameObject);
            }
            // offline?
            else
            {
                Destroy(gameObject);
            }
        }
    }


    void Explode()
    {
        //Show effect
        Instantiate(particlesEffect, transform.position, transform.rotation);

        if (bombType == BombType.TNT)
        {
            source.Stop();
            source.PlayOneShot(explosionSoundFx);
        }

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
        //Destroy(gameObject);
        setInvisible();
        toBeDestroyed = true;
    }

    private void setInvisible()
	{
        foreach (Renderer renderer in renderers)
            renderer.enabled = false;
        collider.enabled = false;
	}
}

public enum BombType
{
    TNT,
    Claymore
}