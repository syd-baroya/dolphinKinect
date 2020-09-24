using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ButterflyBehavior : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    private float[] t;
    private float[] rand_t;
    private float bloom = 90f;
    public GameObject m_butterfiles;
    public GameObject[] m_allButterFlies;
    private ParticleSystem[] m_particleTrails;
    private Bloom bloomLayer = null;
    private Vector3[] oldPos;
    private Vector3[] targetPos;
    public Texture startTexture;
    private Texture[] endTexture;
    private bool texChanged = false;
    public Gradient startGradient;
    private Gradient[] endGradient;
    private float moveToMiddleT = 0f;
    private Vector3 middle = Vector3.zero;
    private bool stopMoving = false;
    private bool stopEffect = false;
    // Start is called before the first frame update
    void Awake()
    {

        PostProcessVolume volume = m_butterfiles.GetComponentInChildren<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        bloomLayer.intensity.value = bloom;
        t = new float[m_allButterFlies.Length];
        rand_t = new float[m_allButterFlies.Length];
        for (int i = 0; i < t.Length; i++)
        {
            t[i] = 0.0f;
            rand_t[i] = 0.0f;
        }
            oldPos = new Vector3[m_allButterFlies.Length];
        targetPos = new Vector3[m_allButterFlies.Length];
        //startColor = Color.gray;
        //endTexture = GetComponent<Renderer>().material.color;

        endTexture = new Texture[m_allButterFlies.Length];
        m_particleTrails = new ParticleSystem[m_allButterFlies.Length];
        for (int i = 0; i < m_allButterFlies.Length; i++)
        {
            endTexture[i] = m_allButterFlies[i].GetComponentInChildren<Renderer>().material.GetTexture("_MainTex");
            m_particleTrails[i] = m_allButterFlies[i].GetComponentInChildren<ParticleSystem>();
            m_allButterFlies[i].GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", startTexture);
        }
        endGradient = new Gradient[m_particleTrails.Length];
        for (int i = 0; i < m_particleTrails.Length; i++)
        {
            var col = m_particleTrails[i].colorOverLifetime;
            endGradient[i] = col.color.gradient;
            col.color = startGradient;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!stopEffect)
        {
            float randX;
            float randY;
            float randZ;

            for (int i = 0; i < m_allButterFlies.Length; i++)
            {
                if (t[i] == 0.0f)
                {
                    randX = Random.Range(minX, maxX);
                    randY = Random.Range(minY, maxY);
                    randZ = Random.Range(minZ, maxZ);
                    rand_t[i] = Random.Range(0.005f, 0.02f);
                    targetPos[i] = new Vector3(randX, randY, randZ);
                    oldPos[i] = m_allButterFlies[i].transform.position;
                }
                else
                    m_allButterFlies[i].transform.position = Vector3.Lerp(oldPos[i], targetPos[i], t[i]);

                t[i] += rand_t[i];
                if (t[i] >= 1f)
                    t[i] = 0.0f;

            }
            if (bloom <= 30 && !texChanged)
            {
                for (int i = 0; i < m_allButterFlies.Length; i++)
                {

                    m_allButterFlies[i].GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", endTexture[i]);
                    var col = m_particleTrails[i].colorOverLifetime;
                    col.color = endGradient[i];
                }
                texChanged = true;
            }
        }
    }


    public void SetActive(bool active)
    {
        m_butterfiles.SetActive(active);
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
            for (int i = 0; i < m_allButterFlies.Length; i++)
            {

                transform.position = Vector3.Lerp(oldPos[i], middle, moveToMiddleT);
            }
            if (moveToMiddleT >= 1f)
                stopMoving = true;
        }
    }

    public bool getStopMoving()
    {
        return stopMoving;
    }

}
