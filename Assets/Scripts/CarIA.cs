using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;
    public bool avoid;
    public float avoidMultiplier;
    public Rigidbody rb;
    float movementThreshold = 0.01f; // Umbral de movimiento en el eje Z para considerar que el objeto se ha movido
    float timeThreshold = 3f; // Tiempo en segundos para determinar si el objeto no se ha movido

    float lastZPosition;// Almacena la última posición en el eje Z
    float timer = 0f; // Temporizador para realizar el seguimiento del tiempo transcurrido





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
    private void Update()
    {
        // Verificar si el objeto se ha movido en el eje Z
        if (Mathf.Abs(gameObject.transform.position.z - lastZPosition) > movementThreshold)
        {
            // Reiniciar el temporizador si el objeto se ha movido
            timer = 0f;
        }
        else
        {
            // Incrementar el temporizador si el objeto no se ha movido
            timer += Time.deltaTime;

            // Verificar si el temporizador ha alcanzado el umbral de tiempo
            if (timer >= timeThreshold)
            {
                // Llamar a la función IAcar
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, 0f);

            }
        }

        // Actualizar la última posición en el eje Z
        lastZPosition = gameObject.transform.position.z;
    }

    private void FixedUpdate()
    {
        Sensors();
        Drive();
        ApplySteer();
        ChangeWaypoint();
    }

    public void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFrontLeft.radius * wheelFrontLeft.rpm * 60 / 100;
        if (currentSpeed < maxSpeed)
        {
            wheelFrontLeft.motorTorque = maxMotorTorque;
            wheelFrontRight.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFrontLeft.motorTorque = 0;
            wheelFrontRight.motorTorque = 0;
        }
    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFrontLeft.steerAngle = newSteer;
        wheelFrontRight.steerAngle = newSteer;
    }

    void ChangeWaypoint()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 3f)
        {
            currentNode++;
        }
        // if (currentNode >= nodes.Count) {
        //     currentNode = 0;
        // }
        currentNode = (currentNode++) % nodes.Count;
    }
    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStarPose = transform.position;
        sensorStarPose += transform.forward * frontSensorPosition.z;
        sensorStarPose += transform.up * frontSensorPosition.y;
        avoid = false;

        //front right sensor//
        sensorStarPose += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStarPose, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("car"))
            {
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier -= 1f;
            }
        }
        //front right angle sensor//

        else if (Physics.Raycast(sensorStarPose, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("car"))
            {
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier -= 0.5f;
            }
        }
        //front left sensor//
        sensorStarPose -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStarPose, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("car"))
            {
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier += 1f;
            }
        }
        //front left angle sensor//
        else if (Physics.Raycast(sensorStarPose, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("car"))
            {
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier += 0.5f;
            }
        }
        //front center sensor//
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStarPose, transform.forward, out hit, sensorLength))
            {
                if (hit.collider.gameObject.CompareTag("car"))
                {
                    Debug.DrawLine(sensorStarPose, hit.point);
                    avoid = true;
                    if (hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }
            }
        }
        if (avoid)
        {
            wheelFrontLeft.steerAngle = maxSteerAngle * avoidMultiplier;
            wheelFrontRight.steerAngle = maxSteerAngle * avoidMultiplier;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("verificador"))
        {
            LapsCount.instance.verificador = true;
        }
        if (other.gameObject.CompareTag("Meta") && LapsCount.instance.verificador == true && LapsCount.instance.vueltas < 3)
        {
            LapsCount.instance.vueltas++;
            LapsCount.instance.verificador = false;
        }
        if (other.gameObject.CompareTag("Meta") && LapsCount.instance.verificador == true && LapsCount.instance.vueltas == 3)
        {
            Debug.Log("entra");
            SceneManager.LoadScene("FinalRace");
        }

    }
}

