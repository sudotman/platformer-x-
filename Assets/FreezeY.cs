using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeY : MonoBehaviour
{

    private float defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, defaultPos, transform.position.z);
    }
}
