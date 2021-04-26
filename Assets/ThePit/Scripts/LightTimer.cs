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
    private Color bulbColour = Color.white;
    [SerializeField]
    private float bulbAlpha = 0.1f;
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
    [SerializeField]
    private AudioSource beepNoise;

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
        float pureSinValue;

        if (bomb.countdown >= 2.0f)
            pureSinValue = Mathf.Sin(timeSinceInstantiation * slowFlashRate);
        else
            pureSinValue = Mathf.Sin(timeSinceInstantiation * fastFlashRate);

        lightSource.intensity = pureSinValue * (maxIntensity - minIntensity) + (1 + minIntensity);
        float colourInterpolateFactor = pureSinValue / 2.0f + 0.5f;
        Color tempColor = Color.Lerp(bulbColour, lightColour, colourInterpolateFactor);
        lightBulbMaterial.color = new Color(tempColor.r, tempColor.g, tempColor.b, bulbAlpha);

        //Debug.Log(pureSinValue);

        if (bomb.countdown > 0.0f && 1.0f - pureSinValue < 0.08f )
            beepNoise.Play();
    }

	private void OnValidate()
	{
        lightSource.color = lightColour;
        lightSource.intensity = maxIntensity;
        lightBulbMaterial.color = new Color(lightColour.r, lightColour.g, lightColour.b, bulbAlpha);
	}

}
