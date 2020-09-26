using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychadelicBehavior : MonoBehaviour
{
    public GameObject m_object;
    private float fade_t = 0f;
    private float angle = 3f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        m_object.transform.Rotate(Vector3.forward, angle/5f);
    }

    public bool PlayEffect()
    {
        if (fade_t < 1f)
        {
            fade_t += Time.deltaTime * 0.5f;
            Color fading_color = Color.Lerp(Color.black, Color.white, fade_t);
            GetComponent<Renderer>().material.SetColor("_Color", fading_color);
            m_object.SetActive(true);
        }
        else
        {
            fade_t = 1f;
            return true;
        }
        return false;
    }
    public bool StopEffect()
    {
        if (fade_t > 0f)
        {
            fade_t -= Time.deltaTime * 0.5f;
            Color fading_color = Color.Lerp(Color.white, Color.black, 1.0f - fade_t);
            GetComponent<Renderer>().material.SetColor("_Color", fading_color);
        }
        else
        {
            m_object.SetActive(false);
            fade_t = 0f;
            return true;
        }
        return false;
    }

    public void SetActive(bool active)
    {
        m_object.SetActive(active);
    }
}
