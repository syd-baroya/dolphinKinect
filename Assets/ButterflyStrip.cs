using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyStrip : MonoBehaviour
{
    public GameObject m_object;

    public float speed = 25;
    Vector3[] ogPositions;
    public int maxButterflies;
    float[] radii;
    float[] speeds;
    float[] timeCounter;
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        ogPositions = new Vector3[maxButterflies];
        radii = new float[maxButterflies];
        speeds = new float[maxButterflies];
        timeCounter = new float[maxButterflies];
        foreach (Transform child in transform)
        {
            
            ogPositions[i] = child.position;
            i++;
        }
        for(int j = 0; j < maxButterflies; j++)
        {
            speeds[j] = Random.Range(0.5f, 0.9f);
            timeCounter[j] = Random.Range(-2f, -5f); ;
            radii[j] = Random.Range(0.4f, 0.6f);
        }
    }
    // Update is called once per frame
    void Update()
    {
      
       

    }

    public void PlayEffect()
    {
        m_object.SetActive(true);
        float z = transform.position.z;
        int i = 0;
        foreach (Transform child in transform)
        {
            if (i >= maxButterflies)
                break;
            timeCounter[i] += Time.deltaTime; // multiply all this with some speed variable (* speed);

            float y = Mathf.Cos(timeCounter[i] * speeds[i]);
            float x = Mathf.Sin(timeCounter[i] * speeds[i]);
            if (timeCounter[i] * speeds[i] >= 0.65f)
            {
                timeCounter[i] = Random.Range(-2f, -5f); ;
                //child.position = ogPositions[i];
            }
            else
                child.position = new Vector3(radii[i] * x, radii[i] * y, z);
            i++;
        }
    }
    public bool StopEffect()
    {
        float z = transform.position.z;
        int i = 0;
        int counterDone = 0;
        foreach (Transform child in transform)
        {
            if (i >= maxButterflies)
                break;
            timeCounter[i] += Time.deltaTime; // multiply all this with some speed variable (* speed);

            float y = Mathf.Cos(timeCounter[i] * speeds[i]);
            float x = Mathf.Sin(timeCounter[i] * speeds[i]);
            if (timeCounter[i] * speeds[i] >= 0.65f)
            {
                counterDone++;
            }
            else
                child.position = new Vector3(radii[i] * x, radii[i] * y, z);
            i++;
        }

        if (counterDone >= maxButterflies)
        {
            for(int j = 0; j<maxButterflies; j++)
                timeCounter[j] = Random.Range(-2f, -5f);
            m_object.SetActive(false);
            return true;
        }
        return false;

    }
    public void SetActive(bool active)
    {
        m_object.SetActive(active);
    }
}
