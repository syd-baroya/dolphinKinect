using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DolphinMover : MonoBehaviour
{
    private float bloom = 0f;
    public GameObject m_dolphin;
    private Bloom bloomLayer = null;
    private float t = 0.0f;
    public float radius = 5;
    public float speed = 25;
    public float speed_up_down = 0.3f;
    public float speed_tilt = 0.3f;
    public float maxTilt = 7.5f;
    public float maxUpAndDown = 0.3f;

    private Animator myAnimator;
    private Vector3 middle;
    private bool stopMoving = false;
    private Vector3 start;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
   
    // Start is called before the first frame update
    void Start()
    {
        middle = transform.position;
        transform.Translate(new Vector3(radius, 0, 0));
        transform.Rotate(new Vector3(0, 90, 0));

        PostProcessVolume volume = m_dolphin.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        bloomLayer.intensity.value = bloom;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void setRotation(Quaternion rot)
    {
        transform.rotation = rot;
    }

    public void swimAround()
    {
        var clipCurrTime = myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        var tiltAngle = clipCurrTime * 2 * Mathf.PI;
        float tilt = Mathf.Cos(tiltAngle * speed_tilt) * maxTilt - transform.rotation.eulerAngles.z;

        transform.RotateAround(middle, Vector3.up, speed * Time.deltaTime);

        transform.Rotate(0, 0, tilt);
        transform.position = new Vector3(transform.position.x, middle.y + Mathf.Cos(tiltAngle * speed_up_down) * maxUpAndDown, transform.position.z);
        start = transform.position;

    }

    public bool getStopMoving()
    {
        return stopMoving;
    }

    public void StartingWave()
    {
        if (!stopMoving)
        {

            //float max_r = Vector3.Distance(start, middle);
            //float t = Mathf.InverseLerp(0.0f, max_r, Vector3.Distance(transform.position, middle));
            //transform.position = Vector3.Lerp(start, middle, t);

            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, middle, t);
            if (t >= 1.0f)
                stopMoving = true;
        }
    }

    public void SetActive(bool active)
    {
        m_dolphin.SetActive(active);
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
