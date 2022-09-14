using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteEquation : MonoBehaviour
{
    private float timer = 0.0f;

    private Vector2 currentTranslation;
    public bool executingNow = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTranslation = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (executingNow)
        {
           // Debug.Log(timer);
            timer += Time.deltaTime;

            //Debug.Log(Mathf.Sin(timer)/100);

            currentTranslation.y = currentTranslation.y + Mathf.Sin(timer)/100;
            currentTranslation.x = currentTranslation.x + timer/1000;

            transform.position = currentTranslation;
        }

        
    }

    void StartExecuting()
    {
        executingNow = true;
    }
}
