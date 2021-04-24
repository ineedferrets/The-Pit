using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject floorNormalBlock;
    public GameObject wallNormal;

    public int x_min, x_max, y_min, y_max, z_min, z_max;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = x_min; x <= x_max; x++)
        {
            for (int y = y_min; y <= y_max; y++)
            {
                for (int z = z_min; z <= z_max; z++)
                {
                    GameObject floor = Instantiate(floorNormalBlock, new Vector3(x, y, z), Quaternion.identity);
                    floor.transform.parent = gameObject.transform;                
                }
            }
        }

        //for the walls N/S
        GameObject wallN = Instantiate(wallNormal, new Vector3(0, 0, z_min-1), Quaternion.identity);
        GameObject wallS = Instantiate(wallNormal, new Vector3(0, 0, z_max+1), Quaternion.identity);
        
        GameObject wallW = Instantiate(wallNormal, new Vector3(x_min-1, 0, 0), Quaternion.identity);
        wallW.transform.Rotate(0, 90, 0);
        GameObject wallE = Instantiate(wallNormal, new Vector3(x_max+1, 0, 0), Quaternion.identity);
        wallE.transform.Rotate(0, 90, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
