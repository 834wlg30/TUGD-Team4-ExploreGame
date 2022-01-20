using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Sightline), typeof(Animator))]

public class ChaseState : MonoBehaviour, IFSMState
{


    public float mSpeed = 1.5f;
    public float accel = 3.0f;
    public float aSpeed = 720.0f;
    public float fov = 60.0f;
    public string animRunParam = "Run";

    public FSMStateType stateName { get { return FSMStateType.Chase; } }

    private readonly float minChaseDist = 2.5f;
    private float initFOV = 0.0f;

    private NavMeshAgent agent;
    private Sightline sightline;
    private Animator animator;
    [SerializeField] private GameManager gm;
    [SerializeField] private Health plrHP;
    private AudioSource alertSound;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sightline = GetComponent<Sightline>();
        animator = GetComponent<Animator>();
        alertSound = GetComponent<AudioSource>();

        plrHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    public void onEnter()
    {
        initFOV = sightline.fov;
        sightline.fov = fov;

        agent.isStopped = false;
        agent.speed = mSpeed;
        agent.acceleration = accel;
        agent.angularSpeed = aSpeed;

        gm.playerDetected = true;
        alertSound.Play();

        animator.SetBool(animRunParam, true);
    }

    public void doAction()
    {
        mSpeed = 2.5f;
        agent.SetDestination(sightline.lastKnownPos);
    }

    public void onExit()
    {
        agent.isStopped = true;
        sightline.fov = initFOV;
    }

    public FSMStateType shouldTransitionToState()
    {
        if (plrHP.HP >= 100f)
        {
            Debug.Log("Target Caught");
            GameManager.GM.gameOver();
            return FSMStateType.None;
        }
        else if (!sightline.targetInSight)
        {
            Debug.Log("Lost sight of target");
            gm.playerDetected = false;
            return FSMStateType.Patrol;
        }

        return FSMStateType.Chase;
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
