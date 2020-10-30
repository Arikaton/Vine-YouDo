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

    private bool _isActive;

    public void ChangeState()
    {
        if (!_isActive)
        {
            scaleIn.BeginTransitions();
            _isActive = true;
        }
        else
        {
            scaleOut.BeginTransitions();
            _isActive = false;
        }
    }
}
