using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Sightline), typeof(Animator))]

public class PatrolState : MonoBehaviour, IFSMState
{

    public Transform dest;
    public float mSpeed = 1.5f;
    public float accel = 2.0f;
    public float aSpeed = 360.0f;
    public string animAlertParam = "Alert";

    public FSMStateType stateName { get { return FSMStateType.Patrol; } }

    private NavMeshAgent agent;
    private Sightline sightline;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sightline = GetComponent<Sightline>();
        animator = GetComponent<Animator>();
    }

    public void onEnter()
    {
        agent.isStopped = false;
        agent.speed = mSpeed;
        agent.acceleration = accel;
        agent.angularSpeed = aSpeed;

        animator.SetBool(animAlertParam, false);
    }

    public void onExit()
    {
        agent.isStopped = true;
    }

    public void doAction()
    {
        agent.SetDestination(dest.position);
    }

    public FSMStateType shouldTransitionToState()
    {
        if (sightline.targetInSight)
        {
            return FSMStateType.Aim;
        }

        return stateName;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
