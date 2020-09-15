using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLightBehavior : MonoBehaviour
{
    //private float lightIntensity;
    private float startIntensity;
    private float endIntensity;

    // Start is called before the first frame update
    void Start()
    {
        //lightIntensity = GetComponent<Light>().intensity;
        startIntensity = GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUp(float minY, float maxY, float currY)
    {
        float t = Mathf.InverseLerp(minY, maxY, currY);
        GetComponent<Light>().intensity = Mathf.Lerp(startIntensity, 0.0f, t);
        endIntensity = GetComponent<Light>().intensity;
    }

    public void UpdateDown(float minY, float maxY, float currY)
    {
        float t = Mathf.InverseLerp(maxY, minY, currY);
        GetComponent<Light>().intensity = Mathf.Lerp(endIntensity, startIntensity, t);

    }
}
