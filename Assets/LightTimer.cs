using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTimer : MonoBehaviour
{
    [SerializeField]
    private float maxIntensity = 5.0f;
    [SerializeField]
    private float minIntensity = 0.1f;
    [SerializeField]
    private Color lightColour = Color.red;
    [SerializeField]
    private float slowFlashRate = 0.5f;
    [SerializeField]
    private float fastFlashRate = 3.0f;

    private float timeSinceInstantiation = 0.0f;

    [SerializeField]
    private Light lightSource;
    [SerializeField]
    private Material lightBulbMaterial;
    [SerializeField]
    private Bombs bomb;

	private void Awake()
	{
        if (lightSource == null)
            lightSource = GetComponentInChildren<Light>();
        if (lightBulbMaterial == null)
            lightBulbMaterial = GetComponentInParent<Renderer>().material;
        if (bomb == null)
            bomb = GetComponentInParent<Bombs>();
	}

	private void Update()
	{
        timeSinceInstantiation += Time.deltaTime;

		if (bomb.countdown >= 1.0f)
		{
            lightSource.intensity = Mathf.Sin(timeSinceInstantiation) * (maxIntensity - minIntensity);
		}
	}

	private void OnValidate()
	{
        lightSource.color = lightColour;
        lightSource.intensity = maxIntensity;
	}

}
