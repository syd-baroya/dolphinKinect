using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public float maxCameraYDelta = 10.0f;
    private bool stopMoving = false;
    private float startY = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void setRenderingPath(RenderingPath rPath)
    {
        if(GetComponentInChildren<Camera>().renderingPath != rPath)
            GetComponentInChildren<Camera>().renderingPath = rPath;

    }

    public void StartingWave()
    {
        if (!stopMoving)
        {
            float t = Mathf.InverseLerp(0.0f, maxCameraYDelta, (transform.position.y + maxCameraYDelta) - startY);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startY, startY + maxCameraYDelta, t), transform.position.z);
            if (t >= 1.0f)
                stopMoving = true;
        }
    }
}
