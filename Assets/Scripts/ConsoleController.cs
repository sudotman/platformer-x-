using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControls;
using UnityEngine.SceneManagement;

class ValidFunction
{
    public int index;
    public string functionName;

    public ValidFunction(string functionName, int index)
    {
        this.index = index;
        this.functionName = functionName;
    }
}

public class ConsoleController : MonoBehaviour
{
    public PlayerController playerController;

    private Vector3 startPos;

    private bool validExpression = false;

    public bool disableConsole = false;

    string input;

    bool focusable = true;
    bool simulatingNow = false;

    bool showHelp;

    private Rect windowRect = new Rect(0, 0, Screen.width, Screen.height);


    private ValidFunction[] functions = {new ValidFunction("Sin",0), new ValidFunction("Cos",1), new ValidFunction("Tan",1), new ValidFunction("Linear",2), new ValidFunction("Log",3), new ValidFunction("ASin",4), new ValidFunction("ACos", 5), new ValidFunction("ATan", 6) };
    int currentFunction;

    //Math commands
    public static MathCommands platformer_basic;
    public static MathCommands platformer_speed;
    public static MathCommands TEST_MATH_2;
    public static MathCommands reset;
    public static MathCommands HELP;
    public static MathCommands<int> MATH_WITH_PARAM;

    public List<object> commandList;

    private float translationSpeed = 1.0f;

    //for different operators
    //0 - addition, 1 - subtraction, 2 - multiplication, 3 - division
    //a function is defined as fn:Sin(x)
    float prefixAdd = 0.0f;
    float prefixMultiply = 0.0f;
    float suffixAdd = 0.0f;
    float suffixMultiply = 0.0f;

    float prefixTemp = 0.0f;
    float suffixTemp = 0.0f;

    float functionMultiplier = 1;

    bool prefixAdditionCushion = false;
    bool suffixAdditionCushion = false;

    int executedCommandsNavigator = 0;
    List<string> executedCommandsArray = new List<string>();

    string[] currentOperators;


    private void Awake()
    {
        validExpression = true;

        platformer_basic = new MathCommands("platformer(", "tests the math commands", "platformer(x) = prefix * math function * suffix", () =>
        {
            string temp = input.Substring(input.IndexOf('(') + 1);
            Debug.Log(input.IndexOf('='));

            string tempInput = input.Substring(input.IndexOf('=') + 2);

            translationSpeed = 1;
            Debug.Log("called " + tempInput);

            string[] translationTempArray = temp.Split('x');


            string tempFunctionMultiplierString = string.Empty;

            //combine the string from translationtemp array split to one string
            foreach(string s in translationTempArray)
            {
                
                tempFunctionMultiplierString += s;
            }

            tempFunctionMultiplierString = tempFunctionMultiplierString.Substring(tempFunctionMultiplierString.LastIndexOf('('));

            string[] subdividedTempFunctionMultiplierString = tempFunctionMultiplierString.Split('(',')');

            if (subdividedTempFunctionMultiplierString[1].Equals(""))
            {
                functionMultiplier = 1;
            }
            else
            {
                functionMultiplier = float.Parse(subdividedTempFunctionMultiplierString[1]);
            }
          
            if (translationTempArray.Length == 0)
            {
                translationSpeed = 1;
            }
            else
            {
                float.TryParse(translationTempArray[0], out translationSpeed);
                Debug.LogError(translationSpeed);

                if (translationSpeed == 0)
                {
                    translationSpeed = 1;
                }
            }
           
            currentOperators = tempInput.Split(' ');

            //first note down all the operators
            //see if there are some additions then allow the user passing 0 otherewise force the user to pass on only non-negative values
            AdditionCheck();
            int index = 0;
            foreach (string s in currentOperators)
            {
                CheckSystem(s, index);
                index++;
            }
        });

        platformer_speed = new MathCommands("platformer_speed(", "tests the math commands", "platformer_speed(2x) = prefix * math function * suffix", () =>
        {
            string temp = input.Substring(17);
            input = input.Substring(16);
            Debug.Log("called " + input);


            string[] tempArray = temp.Split('x');

            float.TryParse(tempArray[0], out translationSpeed);
            Debug.LogError(translationSpeed);

            if (translationSpeed == 0)
            {
                translationSpeed = 1;
            }

            string[] operators = input.Split(' ');

            int index = 0;
            

            foreach (string s in operators)
            {
                CheckSystem(s, index);
                index++;
            }



        });

        TEST_MATH_2 = new MathCommands("test_math_2", "tests the math commands 2", "test_math_2", () =>
        {
            Debug.Log("called 2");
        });

        reset = new MathCommands("reset", "reset the scene", "reset", () =>
        {
            Debug.Log("called reset");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        MATH_WITH_PARAM = new MathCommands<int>("math_with_param", "tests the math commands 2", "math_with_param <amount>", (x) =>
        {
            Debug.Log("called 3");
        });

        HELP = new MathCommands("help", "show the list of expressions", "help", () =>
           {
               showHelp = true;
           });

        commandList = new List<object>
        {
            platformer_basic,
            platformer_speed,
            TEST_MATH_2,
            reset,
            MATH_WITH_PARAM,
            HELP
        };

    }

    void CheckSystem(string s, int index)
    {

        if (index == 0)
        {
            // Prefix
            if (!NumberCheck(s, index))
                FailedChecks();
        }
        else if (index == 1) // Prefix Operator
        {
            if (!OperatorCheck(s, index))
                FailedChecks();
        }
        else if (index == 2) //Math Function
        {
            if (!FunctionCheck(s))
                FailedChecks();
        }
        else if (index == 3) //Math Operator
        {
            if (!OperatorCheck(s, index))
                FailedChecks();
        }
        else if (index == 4) // Suffix
        {
            if (NumberCheck(s, index))
            {
                PassedChecks();
            }
            else
            {
                FailedChecks();
            }
        }
    }

    bool FunctionCheck(string s)
    {
        int temp = 0;
        foreach(ValidFunction funs in functions)
        {
            Debug.LogError("namess " + funs.functionName);
            temp++;
            if (temp == functions.Length + 1)
            {
                return false;
            }
            else if (s.Contains(funs.functionName))
            {
                currentFunction = temp - 1;
                goto BreakKaro;
            }
        }

        BreakKaro:
            return true;
    }

    void PassedChecks()
    {
        if (validExpression)
        {
            Debug.Log("passed checks");


            this.ResumeMovement();
        }
        else
        {
            Debug.LogAssertion("failed checks");
        }
        
    }

    void AdditionCheck()
    {
        for (int i = 0; i < currentOperators.Length; i++)
        {
            if (i == 1)
            {
                if (currentOperators[i].Equals("+"))
                {
                    prefixAdditionCushion = true;
                }
            }
            else if (i == 3)
            {
                if (currentOperators[i].Equals("+"))
                {
                    suffixAdditionCushion = true;
                }
            }
        }
    }

    bool OperatorCheck(string s, int index)
    {
        switch (s)
        {
            case "*":
                if (index == 1)
                {
                    prefixAdd = 0;

                    if (prefixTemp != 0)
                        prefixMultiply = prefixTemp;
                    else
                    {
                        FailedChecks();
                        return false;
                    }
                }
                return true;
            case "+":
                if (index == 1)
                {
                    prefixMultiply = 1;

                    if (prefixTemp != 0)
                    {
                        prefixAdd = prefixTemp;
                    }
                    else
                    {
                        FailedChecks();
                        return false;
                    }
                }
                return true;
            case "-":
                return true;
            case "/":
                return true;
            default:
                return false;
        }
    }

    bool RetrospectiveSuffixOperatorCheck(string s, int index)
    {
        switch (s)
        {
            case "*":
                suffixAdd = 0;

                if (suffixTemp != 0)
                    suffixMultiply = suffixTemp;
                else
                {
                    FailedChecks();
                    return false;
                }
                return true;
            case "+":
                suffixMultiply = 1;

                if (suffixTemp != 0)
                {
                    suffixAdd = suffixTemp;
                }
                else
                {
                    if (suffixAdditionCushion)
                    {
                        suffixAdd = 0;
                    }
                    else
                    {
                        FailedChecks();
                        return false;
                    }
                }
                return true;
            case "-":
                return true;
            case "/":
                return true;
            default:
                return false;
        }
    }

    bool NumberCheck(string s, int index)
    {
        float.TryParse(s, out float test);

        if (test == 0)
        {
            prefixTemp = 0;

            if (index == 0)
            {
                if (prefixAdditionCushion)
                {
                    prefixTemp = test;
                    return true;
                }
            }
            else if (index == 4)
            {
                if (suffixAdditionCushion)
                {
                    suffixTemp = test;
                    return true;
                }
            }
            return false;
        }
        else
        {
            if (index == 0)
            {
                prefixTemp = test;
            }
            else if (index == 4)
            {
                suffixTemp = test;
                RetrospectiveSuffixOperatorCheck(currentOperators[index - 1], index - 1);
            }
            return true;
        }
    }

    void FailedChecks()
    {
        validExpression = false;
        Debug.LogError("failed checks");

    }

    // Start is called before the first frame update
    void Start()
    {
        focusable = true;

        startPos = playerController.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (executingNow)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;

            Debug.Log(Mathf.Sin(timer) / 100);

            // a better solution than hardcoding different functions should exist but alas i cant find it (or realize it right now) so there it is [OH WAIT, MAYBE I CAN DERIVE THE SIN IN REAL TIME AND SET BORDER VALUES, I WILL EXPLORE LATER]

            if(currentFunction == 0)
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Sin(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd/100);
            }
            else if(currentFunction == 1)
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Cos(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }
            else if(currentFunction == 2)
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Tan(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }
            else if(currentFunction == 3) //linear
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * ((functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }
            else if(currentFunction == 4) // log
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Log(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }
            else if (currentFunction == 5) // asin
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Asin(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }
            else if (currentFunction == 6) // acos
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Acos(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }
            else if (currentFunction == 7) // atan
            {
                currentTranslation.y = currentTranslation.y + (prefixAdd / 100 + prefixMultiply * (Mathf.Atan(functionMultiplier * timer * 2) / 100) * suffixMultiply + suffixAdd / 100);
            }


            Debug.LogError("add " + prefixAdd + "multiply " + prefixMultiply + "add " + suffixAdd + "multiply " + suffixMultiply);

            currentTranslation.x = currentTranslation.x + timer / (100 * (1 / translationSpeed));

            transform.position = currentTranslation;
        }

    }

    Vector2 scroll;

    private void OnGUI()
    {
        if (!disableConsole)
        {
            float y = 0f;

            if (showHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

                for (int i = 0; i < commandList.Count; i++)
                {
                    MathCommandsBase command = commandList[i] as MathCommandsBase;

                    string label = $"{command.commandFormat} - {command.commandDescription}";

                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                    GUI.Label(labelRect, label);
                }

                GUI.EndScrollView();

                y += 100;
            }


            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);

            GUI.SetNextControlName("mathsInput");
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

            if (focusable)
                GUI.FocusControl("mathsInput");
            else
            {
                GUI.FocusControl(null);
            }

            if (Event.current.keyCode == KeyCode.Return)
            { //Event.current.keyCode instead of Input.GetKeyUp because the textfield wouldn't let this one get through...
                if (focusable)
                {
                    Debug.Log("pressed enter");
                    validExpression = true;
                    if (input.Length > 0)
                    {
                        focusable = false;

                        simulatingNow = true;

                        StartCoroutine(Simulate());
                    }

                    GUI.FocusControl(null);

                }
                else if (!simulatingNow)
                {
                    GUI.FocusControl("mathsInput");
                    focusable = true;
                }
            }
            else if(Event.current.keyCode == KeyCode.UpArrow)
            {
                if (!simulatingNow)
                {
                    if (focusable)
                    {
                        Debug.Log("pressed arrow");
                        
                            focusable = false;

                            StartCoroutine(UpArrow());

                        
                        GUI.FocusControl(null);
                    }
                   
                    
                }
                
            }
            else if (Event.current.keyCode == KeyCode.DownArrow)
            {
                if (!simulatingNow)
                {
                    if (focusable)
                    {
                        Debug.Log("pressed arrow");

                        focusable = false;

                        StartCoroutine(DownArrow());


                        GUI.FocusControl(null);
                    }


                }

            }

            if (Event.current.keyCode == KeyCode.Escape)
            {
                if (!simulatingNow)
                {
                    input = "";
                    showHelp = false;
                }

            }
        }


    }

    public void MoveToStart()
    {
        playerController.transform.position = startPos;
    }


    private float timer = 0.0f;

    private Vector2 currentTranslation;
    public bool executingNow = false;

    public void ExecuteEquation()
    {
        playerController.freezeMovement = true;

        timer = 0.0f;

        currentTranslation = new Vector2(transform.position.x, transform.position.y);

        executingNow = true;
    }



    public void ResumeMovement()
    {
        playerController.freezeMovement = false;

        timer = 0.0f;
        executingNow = false;
    }


    void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            MathCommandsBase mathCommandsBase = commandList[i] as MathCommandsBase;

            if (input.Contains(mathCommandsBase.commandId))
            {
                if (commandList[i] as MathCommands != null)
                {
                    (commandList[i] as MathCommands).Invoke();
                }
                else if (commandList[i] as MathCommands<int> != null)
                {
                    (commandList[i] as MathCommands<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }

        //input = new string("");
    }

    IEnumerator Simulate()
    {
        yield return new WaitForSeconds(1f);
        executedCommandsArray.Add(input);
        executedCommandsNavigator = executedCommandsArray.Count - 1;
        HandleInput();
        Debug.Log("done simulating");
        focusable = true;
        simulatingNow = false;
    }

    IEnumerator UpArrow()
    {
        yield return new WaitForSeconds(0.4f);

        if (executedCommandsNavigator > 0)
        {
            Debug.LogError(executedCommandsArray.Count);
            Debug.LogError(executedCommandsNavigator);

            foreach(string s in executedCommandsArray)
            {
                Debug.LogWarning(s);
            }
            executedCommandsNavigator--;
            if (executedCommandsArray.Count > 0)
                input = executedCommandsArray[executedCommandsNavigator];
        }
        
        focusable = true;
        simulatingNow = false;
    }

    IEnumerator DownArrow()
    {
        yield return new WaitForSeconds(0.4f);

        if (executedCommandsNavigator < executedCommandsArray.Count - 1)
        {
            Debug.LogError(executedCommandsArray.Count);
            Debug.LogError(executedCommandsNavigator);

            foreach (string s in executedCommandsArray)
            {
                Debug.LogWarning(s);
            }
            executedCommandsNavigator++;
            if (executedCommandsArray.Count > 0)
                input = executedCommandsArray[executedCommandsNavigator];
        }

        focusable = true;
        simulatingNow = false;
    }

}
