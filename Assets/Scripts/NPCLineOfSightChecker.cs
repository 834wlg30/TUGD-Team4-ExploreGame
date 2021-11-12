using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NPCLineOfSightChecker : MonoBehaviour
{
    //----------------------------------Tutorial for below: https://www.youtube.com/watch?v=t9e2XBQY4Og

    public SphereCollider Collider; //Agro range
    public float FieldOfView;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(Transform Target);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform Target);
    public LoseSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) //If something enters the agro range
    {
        if (!CheckLineOfSight(other.transform)) //If there is clear line of sight between NPC and other thing
        {
            CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.transform)); //Continuously checks for line of sight with that object
        }
    }

    private void OnTriggerExit(Collider other) //If something exits the agro range
    {
        OnLoseSight?.Invoke(other.transform); 
        if (CheckForLineOfSightCoroutine != null) //If there was something with clear line of sight...
        {
            StopCoroutine(CheckForLineOfSightCoroutine); //...There is no longer clear line of sight with that thing
        }
    }

    private bool CheckLineOfSight(Transform Target)
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized; //get direction to them
        float dotProduct = Vector3.Dot(transform.forward, direction); //Get ratio of how in front or behind them you are
        if (dotProduct >= Mathf.Cos(FieldOfView)) //Checks if they're in FOV
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Collider.radius, LineOfSightLayers)) //Checks if there is clear line between the object
            {
                if (hit.transform.tag == "Player") //Checks if the object is hostile to the NPC       <-----------(NOTE! THIS SHOULD BE ADJUSTED LATER TO INCLUDE ENEMY NPCS IN OTHER FACTIONS!!!!!!!)---------
                {
                    OnGainSight?.Invoke(Target);
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(Transform Target) //Repeats check for line of sight
    {
        WaitForSeconds Wait = new WaitForSeconds(0.5f); //Determines how often checks are

        while (!CheckLineOfSight(Target))
        {
            yield return Wait;
        }
    }
}