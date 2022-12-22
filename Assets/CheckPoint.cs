using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ConsoleController>().checkpointPos = transform.position;
            collision.gameObject.GetComponent<ConsoleController>().checkpointPos.y += 1f;
            Debug.Log("reached checkpoint");
            //collision.gameObject.GetComponent<ConsoleController>().MoveToStart();
        }

    }
}
