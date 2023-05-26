using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IA : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int index;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(ChangeWaypoint());
    }
    Vector3 ChangeWaypoint()
    {
        if (index >= waypoints.Length)
        {
            index = 0;
        }
        float distance = Vector3.Distance(transform.position, waypoints[index].transform.position);
        if (distance < 1f)
        {
            index++;
        }
        return waypoints[index].transform.position;
    }
}
