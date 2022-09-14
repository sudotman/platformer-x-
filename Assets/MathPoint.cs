using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControls;

public class MathPoint : MonoBehaviour
{
    public bool mathEndPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("reached math point");

            if (!mathEndPoint)
                collision.gameObject.GetComponent<ConsoleController>().ExecuteEquation();
            else
                collision.gameObject.GetComponent<ConsoleController>().ResumeMovement();
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
