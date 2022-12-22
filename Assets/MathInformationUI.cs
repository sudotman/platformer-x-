using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MathInformationUI : MonoBehaviour
{
    private TextMeshProUGUI mathText;
    
    // Start is called before the first frame update
    void Start()
    {
        mathText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("UpdateInput", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateInput()
    {
        mathText.text = GameManager.instance.consoleController.processedInput;
        //mathText.text = "test";
    }
}
