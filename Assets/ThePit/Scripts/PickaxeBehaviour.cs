using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBehaviour : MonoBehaviour
{
    public int speed = 5;
    public bool mine;
    
    private float startRotationX;
    private float endRotationX;

    private Quaternion initialRotation;


    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.localRotation;
        startRotationX = -45f;
        endRotationX = 35f;
        ResetPick();
    }

    // Update is called once per frame
    void Update()
    {
        if (mine)
        {
            float difference = endRotationX - startRotationX;
            transform.localRotation = Quaternion.AngleAxis(startRotationX + ((Mathf.Sin(3 * Time.time) / 2 + 0.5f) * difference), Vector3.right);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block.health--;
            if (block.health < 1)
            {
                //TODO: Gives points to player that killed it - gameobject that holds the tool.
                Destroy(gameObject);
            }
        }

    }

}
