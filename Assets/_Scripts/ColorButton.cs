using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using Lean.Transition;
using Lean.Transition.Extras;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    [SerializeField] private LeanAnimation scaleIn;
    [SerializeField] private LeanAnimation scaleOut;

    private bool isActive = false;

    public void ChangeState()
    {
        if (!isActive)
        {
            scaleIn.BeginTransitions();
            isActive = true;
        }
        else
        {
            scaleOut.BeginTransitions();
            isActive = false;
        }
    }
}
