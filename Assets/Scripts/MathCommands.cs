using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCommandsBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string commandId { get { return _commandId;  } }
    public string commandDescription { get { return _commandDescription;  } }
    public string commandFormat { get { return _commandFormat;  } }

    public MathCommandsBase(string id, string description,string format)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class MathCommands : MathCommandsBase
{
    private Action command;

    public MathCommands(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class MathCommands<T1> : MathCommandsBase
{
    private Action<T1> command;

    public MathCommands(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}
