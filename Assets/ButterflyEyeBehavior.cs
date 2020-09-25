using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ButterflyEyeBehavior : MonoBehaviour
{
    Vector2 offsets;
    float[] t;
    float[] speedToEye;
    float[] flappingSpeed;
    float movingDeltaTime = 0;
   
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    private float[] flyingT;
    private float[] rand_t;
    private float bloom = 90f;
    private Bloom bloomLayer = null;
    public GameObject m_butterfly;
    private float moveToMiddleT = 0f;
    private Vector3 middle = Vector3.zero;
    private bool stopMoving = false;

    public GameObject[] m_allButterFlies;
    private Vector3[] oldPos;
    private Vector3[] targetPos;
    private bool moveToMIddle = false;

    private void Awake()
    {
        PostProcessVolume volume = m_butterfly.GetComponentInChildren<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        bloomLayer.intensity.value = bloom;
        flyingT = new float[m_allButterFlies.Length];
        rand_t = new float[m_allButterFlies.Length];
        t = new float[m_allButterFlies.Length];
        speedToEye = new float[m_allButterFlies.Length];
        flappingSpeed = new float[m_allButterFlies.Length];
        for (int i = 0; i < flyingT.Length; i++)
        {
            flyingT[i] = 0.0f;
            rand_t[i] = 0.0f;
            t[i] = 0.0f;
            speedToEye[i] = Random.Range(0, 5);
            flappingSpeed[i] = m_allButterFlies[i].GetComponent<Renderer>().material.GetFloat("_Speed");

        }
        oldPos = new Vector3[m_allButterFlies.Length];
        targetPos = new Vector3[m_allButterFlies.Length];
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StartEyeTransform()
    {
        StopFlapping();
        for (int i = 0; i < m_allButterFlies.Length; i++)
        {
            if (t[i] <= 35f && speedToEye[i] <= 0)
            {
                t[i] += Time.deltaTime * 36;
                offsets.x = (((int)t[i]) % 6) * (1f / 6f);
                offsets.y = (5f / 6f) - (((int)t[i] / 6) * (1f / 6f));
                m_allButterFlies[i].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offsets);
            }
            else
                speedToEye[i] -= Time.deltaTime;
        }
    }

    public void StopFlapping()
    {
        for (int i = 0; i < m_allButterFlies.Length; i++)
        {
            m_allButterFlies[i].GetComponent<Renderer>().material.SetFloat("_Speed", 0f);
        }
    }

    public void StartFlapping()
    {
        for (int i = 0; i < m_allButterFlies.Length; i++)
        {
            m_allButterFlies[i].GetComponent<Renderer>().material.SetFloat("_Speed", flappingSpeed[i]);
        }
    }

    public void PlayEffect()
    {

        if (movingDeltaTime > 3f)
        {
            StartEyeTransform();
            for (int i = 0; i < m_allButterFlies.Length; i++)
            {
                targetPos[i] = m_allButterFlies[i].transform.position;
            }
        }
        else
        {
            movingDeltaTime += Time.deltaTime;

            for (int i = 0; i < m_allButterFlies.Length; i++)
            {
                if (flyingT[i] == 0.0f)
                {
                    float randX = Random.Range(minX, maxX);
                    float randY = Random.Range(minY, maxY);
                    rand_t[i] = Random.Range(0.005f, 0.02f);
                    targetPos[i] = new Vector3(randX, randY, m_allButterFlies[i].transform.position.z);
                    oldPos[i] = m_allButterFlies[i].transform.position;
                }
                Vector3 pos = Vector3.Lerp(oldPos[i], targetPos[i], flyingT[i]);
                m_allButterFlies[i].transform.position = pos;
                if(flyingT[i] < 1f)
                    flyingT[i] += rand_t[i];

            }
        }
    }
    public void SetActive(bool active)
    {
        m_butterfly.SetActive(active);

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
        if (!stopMoving)
        {
            if (!moveToMIddle)
                for (int i = 0; i < m_allButterFlies.Length; i++)
                {
                    if (t[i] >= 0f)
                    {
                        t[i] -= Time.deltaTime * 36;
                        offsets.x = (((int)t[i]) % 6) * (1f/6f);
                        offsets.y = (5f / 6f) - (((int)t[i] / 6) * (1f / 6f));
                        m_allButterFlies[i].GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offsets);
                    }

                    else
                        moveToMIddle = true;
                }
            else
            {
                StartFlapping();
                moveToMiddleT += Time.deltaTime*0.5f;
                for (int i = 0; i < m_allButterFlies.Length; i++)
                {
                    m_allButterFlies[i].transform.position = Vector3.Lerp(targetPos[i], middle, moveToMiddleT);

                }
                if (moveToMiddleT >= 1f)
                {
                    stopMoving = true;
                }
            }
        }
    }
    public bool getMoveToMiddle()
    {
        return moveToMIddle;
    }
    public void setMoveToMiddle(bool m)
    {
        moveToMIddle = m;
    }
    public bool getStopMoving()
    {
        return stopMoving;
    }
}
