using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0.14f, 0.32f, 1.58f);

    }
}
