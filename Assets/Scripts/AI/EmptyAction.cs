using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAction : IFSMState
{
    public FSMStateType stateName { get { return FSMStateType.None; } }

    public void onEnter() { }
    public void onExit() { }
    public void doAction() { }
    public FSMStateType shouldTransitionToState()
    {
        return FSMStateType.None;
    }
}