using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoRace : MonoBehaviour
{
    public TMP_Text Laps;
    public TMP_Text velocityText;
    public TMP_Text LapsText;
    public PlayerMovement playerMovementScript;
    public LapsCount lapsScript;

    public float speed;
    public int vueltas;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Acceder a currentAcceleration desde el script PlayerMovement
    speed = playerMovementScript.GetComponent<Rigidbody>().velocity.magnitude;
    float currentSpeed = Mathf.Floor(speed * 10);
    velocityText.GetComponent<TMPro.TextMeshProUGUI>().text = currentSpeed.ToString();
    vueltas = lapsScript.vueltas;
    LapsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Laps " + vueltas.ToString() + "/3";

    }
}
