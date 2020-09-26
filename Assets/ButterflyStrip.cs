using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyStrip : MonoBehaviour
{
    Vector3 middle;
    public float speed = 25;

    // Start is called before the first frame update
    void Start()
    {
        middle = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetComponentInChildren<Transform>().transform.position);
        GetComponentInChildren<Transform>().transform.RotateAround(middle, -Vector3.forward, speed * Time.deltaTime);
    }
}
