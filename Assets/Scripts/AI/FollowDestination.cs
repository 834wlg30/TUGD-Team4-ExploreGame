/*
 * William Gulick
 * Created 10/27/2021
 * Last modified: 10/27/2021
 * Moves agent to destination
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class FollowDestination : MonoBehaviour
{

    public Transform dest;
    private NavMeshAgent agent = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(dest.position);
    }
}
