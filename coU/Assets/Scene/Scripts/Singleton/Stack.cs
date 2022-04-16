using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack
{
    private static Stack _stack = null;
    private static Stack<SceneInfo> stack = new Stack<SceneInfo>();

    private Stack() {}

    public static Stack Instance {
        get {
            if (_stack == null)
                _stack = new Stack();
            return _stack;
        }
    }

    public void Push(SceneInfo element)
    {
        stack.Push(element);
    }

    public SceneInfo Pop()
    {
        return (stack.Pop());
    }

    public void Clear()
    {
        stack.Clear();
    }

    public int Count()
    {
        return (stack.Count);
    }
}
