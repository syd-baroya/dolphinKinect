using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ButterflyBehavior : MonoBehaviour
{
    private float bloom = 150f;
    public GameObject m_butterfiles;
    private Bloom bloomLayer = null;
    // Start is called before the first frame update
    void Start()
    {
        PostProcessVolume volume = m_butterfiles.GetComponentInChildren<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        bloomLayer.intensity.value = bloom;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        bloomLayer.intensity.value = bloomLayer.intensity.value - 1;
        bloom--;
    }

}
