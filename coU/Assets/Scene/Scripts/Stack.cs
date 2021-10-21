using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private static List<SceneInfo> stack = null;
    private static Stack _stack = null;

    public static Stack Instance {
        get {
            if (_stack == null)
                _stack = new Stack();
            return _stack;
        }
    }

    public void Push(SceneInfo element)
    {
        if (stack == null)
            stack = new List<SceneInfo>();
        stack.Add(element);
    }

    public SceneInfo Pop()
    {
        int lastIdx = stack.ToArray().Length;
        SceneInfo ret = stack[lastIdx];

        stack.RemoveAt(lastIdx);
        return (ret);
    }

    public void Clear()
    {
        stack = null;
    }
}
