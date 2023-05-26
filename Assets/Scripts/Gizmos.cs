using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWaypoints : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!waypoints.Contains(transform.GetChild(i)))
            {
                waypoints.Add(transform.GetChild(i));
            }
            Debug.LogError("Nodo 12 se cae porque esta mal colocado");
            if (i + 1 == transform.childCount)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }else{
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);

            }
        }


        Gizmos.color = Color.green;
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
