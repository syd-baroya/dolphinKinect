using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    private float bloom = 90f;
    private Bloom bloomLayer = null;
    public GameObject m_butterfly;
    private float moveToMiddleT = 0f;
    private Vector3 middle = Vector3.zero;
    private bool stopMoving = false;
    private bool stopEffect = false;
    private Vector3 start;
    private bool eyeTransform = false;
    private void Awake()
    {
        PostProcessVolume volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        bloomLayer.intensity.value = bloom;
    }
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
        if (!stopEffect)
        {
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
            start = transform.position;
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


        if (movingDeltaTime > 3f && !eyeTransform)
        {
            StartEyeTransform();
            movingDeltaTime = 0f;
            eyeTransform = true;
           
        }
        else if(!eyeTransform)
        {
            movingDeltaTime += Time.deltaTime;
            flyingAround = true;
        }
    }
    public void SetActive(bool active)
    {
        m_butterfly.SetActive(active);
        stopEffect = !active;

    }
    public void SetBloom(float value)
    {
        bloomLayer.intensity.value = value;
        bloom = value;
    }

    public float GetBloom()
    {
        return bloom;
    }

    public void IncrBloom()
    {
        bloomLayer.intensity.value = bloomLayer.intensity.value + 1;
        bloom++;
    }
    public void DecrBloom()
    {
        bloomLayer.intensity.value -= 0.5f;
        bloom -= 0.5f;
    }
    public void StopEffect()
    {
        stopEffect = true;
        if (!stopMoving)
        {
            moveToMiddleT += Time.deltaTime;
            
            transform.position = Vector3.Lerp(start, middle, moveToMiddleT);
            if (moveToMiddleT >= 1f)
                stopMoving = true;
        }
    }

    public bool getStopMoving()
    {
        return stopMoving;
    }
}
