using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public string WindowName
    {
        get
        {
            return gameObject.name;
        }
    }
}
