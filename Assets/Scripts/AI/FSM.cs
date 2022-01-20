using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public FSMStateType startState = FSMStateType.Patrol;
    private IFSMState[] states;
    private IFSMState currState;

    private readonly IFSMState emptyAction = new EmptyAction();

    private void Awake()
    {
        states = GetComponents<IFSMState>();
    }

    private void Start()
    {
        currState = emptyAction;
        transitionToState(startState);
    }

    private void Update()
    {
        currState.doAction(); //perform state action
        FSMStateType tranState = currState.shouldTransitionToState();

        //transition to new state if necessary
        if(tranState != currState.stateName)
        {
            transitionToState(tranState);
        }
    }

    private void transitionToState(FSMStateType s)
    {
        currState.onExit();
        currState = getState(s);
        currState.onEnter();

        Debug.Log("Transitioned to State:" + currState.stateName);
    }

    IFSMState getState(FSMStateType s)
    {
        //if state exists
        foreach(var state in states)
        {
            if(state.stateName == s)
            {
                return state;
            }
        }

        //if state does not exist
        return emptyAction;
    }
}
