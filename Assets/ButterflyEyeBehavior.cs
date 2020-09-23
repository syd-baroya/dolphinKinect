using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyEyeBehavior : MonoBehaviour
{
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        //enabled = false;
        ps = GetComponent<ParticleSystem>();
        //var main = ps.main;
        //main.startColor = Random.ColorHSV(0f, 1f, 0f, 1f, 0.5f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ps == null)
        {
            enabled = false;
            return;
        }

        var main = ps.main;
        main.startColor = Random.ColorHSV(0f, 1f, 0f, 1f, 0.2f, 1f, 1f, 1f);
    }
}
