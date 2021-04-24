using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutout : MonoBehaviour
{
	[SerializeField] private Transform targetObject;
	[SerializeField] private LayerMask wallMask;
	[SerializeField] private float cutoutSize, falloffSize;

	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = GetComponent<Camera>();
	}

	private void Update()
	{
		Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);

		Vector3 offset = targetObject.position - transform.position;
		RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

		for (int i = 0; i < hitObjects.Length; ++i)
		{
			Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

			for (int j = 0; j < materials.Length; ++j)
			{
				materials[j].SetVector("_CutoutPos", cutoutPos);
				materials[j].SetFloat("_CutoutSize", cutoutSize);
				materials[j].SetFloat("_FalloffSize", falloffSize);
			}
		}
	}
}
