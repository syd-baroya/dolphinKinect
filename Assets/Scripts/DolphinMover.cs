using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinMover : MonoBehaviour
{
    public float radius = 30;
    public float speed = 10;
    public float speed_up_down = 1;
    public float speed_tilt = 1;
    public float maxTilt = 15;
    public float maxUpAndDown = 3;

    private float animClipLength;
    private Animation myAnim;
    private bool ascentStopped = false;
    private bool descentStopped = false;
    private void Awake()
    {
        myAnim = GetComponent<Animation>();
        var clip = GetComponent<Animation>().GetClip("fastswim2");
        animClipLength = clip.length;
    }
   
    Vector3 middle;
    // Start is called before the first frame update
    void Start()
    {
        middle = transform.position;
        transform.Translate(new Vector3(radius, 0, 0));

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
        var tiltAngle = myAnim["fastswim2"].time / animClipLength * 2 * Mathf.PI;
        float tilt = Mathf.Cos(tiltAngle * speed_tilt) * maxTilt - transform.rotation.eulerAngles.x;

        transform.RotateAround(middle, Vector3.up, speed * Time.deltaTime);

        transform.Rotate(tilt, 0, 0);
        transform.position = new Vector3(transform.position.x, middle.y + Mathf.Cos(tiltAngle * speed_up_down) * maxUpAndDown, transform.position.z);
    }

    //public void startAscent()
    //{
    //    //if (myAnim["swim"].time % animClipLength < 1)
    //    //{
    //    float mod_swim = myAnim["swim"].time % animClipLength;
    //    Debug.Log(mod_swim);
    //    float tiltAngle = mod_swim < 1 ? (2 * Mathf.PI) : (mod_swim / animClipLength * 2 * Mathf.PI);
    //        float tilt = Mathf.Cos(tiltAngle * 2) * 60 - transform.rotation.eulerAngles.x;
    //        transform.Rotate(tilt, 0, 0);
    //        if (tiltAngle*2 >= (2 * Mathf.PI))
    //        {
    //            Debug.Log("ascent stopped");
    //            ascentStopped = true;
    //            descentStopped = false; 
    //        }
    //    //}
    //    //else
    //    //    this.swimAround();
    //}

    //public void startDescent()
    //{
    //    //if (myAnim["swim"].time % animClipLength < 1)
    //    //{
    //        float mod_swim = myAnim["swim"].time % animClipLength;
    //    Debug.Log(mod_swim);
    //    float tiltAngle = mod_swim < 1 ? (2 * Mathf.PI) : (mod_swim / animClipLength * 2 * Mathf.PI);
    //        float tilt = -Mathf.Cos(tiltAngle * speed_tilt) * 60 - transform.rotation.eulerAngles.x;
    //        transform.Rotate(tilt, 0, 0);
    //        Debug.Log(tiltAngle);
    //        Debug.Log(tilt);

    //        if (tiltAngle >= (2 * Mathf.PI))
    //        {
    //            Debug.Log("descent stopped");
    //            descentStopped = true;
    //            ascentStopped = false;
    //        }
    //    //}
    //    //else
    //    //    this.swimAround();
    //}

    //public bool getDescentStopped()
    //{
    //    return descentStopped;
    //}

    //public bool getAscentStopped()
    //{
    //    return ascentStopped;
    //}

}
