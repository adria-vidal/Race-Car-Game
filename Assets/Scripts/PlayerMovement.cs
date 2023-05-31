using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    public float acceleration = 500f;
    public float breackingForce = 300f;
    public float maxTurnAngle = 15f;

    public float currentAcceleration = 0f;
    private float currentBreackForce = 0f;
    private float currentTurnAngle = 0f;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
 // Encuentra el componente AudioSource del objeto "SoundManager"
        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.W))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Reproduce el sonido
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); // Detiene el sonido si se suelta la tecla "W"
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            currentBreackForce = breackingForce;
        }
        else
        {
            currentBreackForce = 0f;
        }

        //Apply acceleration wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreackForce;
        frontLeft.brakeTorque = currentBreackForce;
        backLeft.brakeTorque = currentBreackForce;
        backRight.brakeTorque = currentBreackForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
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