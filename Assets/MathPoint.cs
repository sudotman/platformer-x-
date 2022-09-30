using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControls;

public class MathPoint : MonoBehaviour
{
    public bool mathEndPoint;
    BoxCollider2D boxCollider2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("reached math point");

            if (!mathEndPoint)
            {
                collision.gameObject.GetComponent<ConsoleController>().ExecuteEquation();

                GameManager.instance.lastDisabledCollider = boxCollider2D;
                boxCollider2D.enabled = false;
            }        
            else
            {
                collision.gameObject.GetComponent<ConsoleController>().ResumeMovement();

                GameManager.instance.lastDisabledCollider.enabled = true;
            }
                

            
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
