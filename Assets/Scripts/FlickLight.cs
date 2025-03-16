using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]

public class FlickLight : MonoBehaviour
{
    private Light lightToFlicker;
    [SerializeField, Range(0f, 3f)] private float minIntensity = 0.5f;
    [SerializeField, Range(0f, 3f)] private float maxIntensity = 1.2f;
    [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;

    private float currentTime;

    private void Awake()
    {
        if (lightToFlicker == null)
        {
            lightToFlicker = GetComponent<Light>();
        }

        ValidateIntensityBounds();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (!(currentTime >= timeBetweenIntensity)) return;
        lightToFlicker.intensity = Random.Range(minIntensity, maxIntensity);
        currentTime = 0;
    }

    private void ValidateIntensityBounds()
    {
        if (!(minIntensity > maxIntensity))
        {
            return;
        }

        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}
