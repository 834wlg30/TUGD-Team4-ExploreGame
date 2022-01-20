using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : MonoBehaviour
{

    private NavMeshAgent agent;
    private Sightline sightline;
    private Animator animator;
    private Transform target;
    private bool isAttacking;

    public string animAttackParam = "Attack";
    public string targetTag = "Player";
    public float delay = 2.0f;
    public float escapeDist = 2.0f;
    public float maxAttackDist = 2.0f;

    public FSMStateType stateName { get { return FSMStateType.Attack; } }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sightline = GetComponent<Sightline>();
        animator = GetComponent<Animator>();
    }

    public void onEnter()
    {
        Debug.Log("Attacking Player");
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        if (isAttacking)
        {
            Debug.Log("Attacking Player");
            animator.SetTrigger(animAttackParam);
            agent.isStopped = true;
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }
    public void onExit()
    {
        agent.isStopped = true;
        isAttacking = false;
        StopCoroutine(Attack());
    }
    public void doAction()
    {
        isAttacking = Vector3.Distance(target.position, transform.position) < maxAttackDist;

        if (!isAttacking)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
    }
    public FSMStateType shouldTransitionToState()
    {
        if(Vector3.Distance(target.position, transform.position) > escapeDist)
        {
            return FSMStateType.Chase;
        }
        return FSMStateType.Attack;
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
