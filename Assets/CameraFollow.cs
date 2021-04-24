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

    // Update is called once per frame
    void Update()
    {
        targetPosition = getMoveToPosition(targetTransform.position, relativeToTargetPosition, length);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
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
}
