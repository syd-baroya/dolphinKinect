using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinMover : MonoBehaviour
{
    public float radius = 20;
    public float speed = 20;
    public float speed_up_down = 0.3f;
    public float speed_tilt = 0.3f;
    public float maxTilt = 30;
    public float maxUpAndDown = 6;

    private float animClipLength;
    private Animator myAnimator;
    
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        var m_CurrentClipInfo = myAnimator.GetCurrentAnimatorClipInfo(0);
        animClipLength = m_CurrentClipInfo[0].clip.length;
    }
   
    Vector3 middle;
    // Start is called before the first frame update
    void Start()
    {
        middle = transform.position;
        transform.Translate(new Vector3(radius, 0, 0));
        transform.Rotate(new Vector3(0, 90, 0));


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

    }

}
