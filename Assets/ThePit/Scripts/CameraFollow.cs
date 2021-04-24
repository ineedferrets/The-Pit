using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    [SerializeField] private Vector3 relativeToTargetPosition;

    [SerializeField] private float length;

    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private float speed;


    public Transform Target;
    public Transform Obstruction;

    float zoomSpeed = 2f;

    void Start()
    {   
        //for camera obstruction purposes
        Obstruction = Target;        
    }


    // Update is called once per frame
    void Update()
    {
        targetPosition = getMoveToPosition(targetTransform.position, relativeToTargetPosition, length);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
    }

    void LateUpdate()
    {
        ViewObstructed();        
    }

    private void OnValidate()
	{
        targetPosition = getMoveToPosition(targetTransform.position, relativeToTargetPosition, length);
        transform.position = targetPosition;
    }

    private Vector3 getMoveToPosition(Vector3 targetCurrentPosition, Vector3 targetPosition, float length)
	{
        return targetCurrentPosition + targetPosition.normalized * length;
	}

    //Checking for obstructions
    void ViewObstructed()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Target.position - transform.position, out hit, 4.5f))
        {
            if(hit.collider.gameObject.tag != "Player")
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if (Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }

            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

                if (Vector3.Distance(transform.position, Target.position) < 4.5f)
                {
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }

            }
        }

    }
}
