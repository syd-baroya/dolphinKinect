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
    Vector3 from;
    Vector3 to;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    private float flyingT;
    private float rand_t;


    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<Renderer>().material.GetFloat("_Speed"); ;
        transformToEye = false;
        flyingAround = true;
        offsets = GetComponent<Renderer>().material.GetTextureOffset("_MainTex");

        from = transform.position;
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
  
            if (flyingT == 0.0f)
            {
                float randX = Random.Range(minX, maxX);
                float randY = Random.Range(minY, maxY);
                float randZ = Random.Range(minZ, maxZ);
                rand_t = Random.Range(0.005f, 0.02f);
                to = new Vector3(randX, randY, transform.position.z);
                from = transform.position;
            }
            Vector3 pos = Vector3.Lerp(from, to, flyingT);
            transform.position = pos;
            flyingT += rand_t;
            if (flyingT >= 1f)
                flyingT = 0.0f;
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
