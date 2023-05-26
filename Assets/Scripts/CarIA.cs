using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarIA : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 45f;
    public WheelCollider wheelFrontLeft;
    public WheelCollider wheelFrontRight;
   
    public List<Transform> nodes;
    public int currentNode = 0;
    public float maxMotorTorque = 80f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    
    private void Start()
    {
        
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    private void FixedUpdate() {
        Drive();
        ApplySteer();
        ChangeWaypoint();
    }

    public void Drive() {
        currentSpeed = 2 * Mathf.PI * wheelFrontLeft.radius * wheelFrontLeft.rpm * 60 /100;
        if (currentSpeed < maxSpeed) {
            wheelFrontLeft.motorTorque = maxMotorTorque;
            wheelFrontRight.motorTorque = maxMotorTorque;
        } else {
            wheelFrontLeft.motorTorque = 0;
            wheelFrontRight.motorTorque = 0;
        }
    }

    void ApplySteer() {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFrontLeft.steerAngle = newSteer;
        wheelFrontRight.steerAngle = newSteer;
    }

    void ChangeWaypoint() {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 3f) {
            currentNode++;
        }
        // if (currentNode >= nodes.Count) {
        //     currentNode = 0;
        // }
        currentNode = (currentNode++)%nodes.Count;
    }
}
