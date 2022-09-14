using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControls;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("reached end");
            collision.gameObject.GetComponent<ConsoleController>().MoveToStart();
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
