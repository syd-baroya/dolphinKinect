using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyEyeBehavior : MonoBehaviour
{
    Vector2 offsets;
    float t = 0;
    bool transformToEye;
    bool flyingAround;
    float speed;
    float movingDeltaTime = 0;
    // Range over which height varies.
    float heightScale = 1.0f;

    // Distance covered per second along X axis of Perlin plane.
    float xScale = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<Renderer>().material.GetFloat("_Speed"); ;
        transformToEye = false;
        flyingAround = true;
        offsets = GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hi");

        if (transformToEye)
        {
            if (t <= 15f)
            {
                t += Time.deltaTime * 8;
                offsets.x = (((int)t) % 4) * 0.25f;
                offsets.y = 0.75f - (((int)t / 4) * 0.25f);
                GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offsets);
            }
            else
                StopEyeTransform();
        }
        if (flyingAround)
        {
            float height = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
            Vector3 pos = transform.position;
            float rand = Random.Range(0, 2);
            if (rand == 1)
                height = -height;
            rand = Random.Range(0, 3);
            if (rand==0)
                pos.x += height;
            if (rand == 1)
                pos.y += height;
            if (rand == 2)
                pos.z += height;
            transform.position = pos;
        }
    }

    public void StartEyeTransform()
    {
        StopFlapping();
        transformToEye = true;
        flyingAround = false;

    }

    public void StopEyeTransform()
    {
        transformToEye = false;
        t = 0f;
    }
    public void StopFlapping()
    {
        GetComponent<Renderer>().material.SetFloat("_Speed", 0f);
    }

    public void StartFlapping()
    {
        GetComponent<Renderer>().material.SetFloat("_Speed", speed);

    }

    public void PlayEffect()
    {
        enabled = true;
        flyingAround = true;
        movingDeltaTime += Time.deltaTime;
        if (movingDeltaTime > 3f)
            StartEyeTransform();
    }

}
