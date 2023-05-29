using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Canvas canvas;

    public bool colisionandoConCircuito = true;
    

    private void OnTriggerEnter(Collider other)
    {

        // Verificar si colisiona con la carretera
        if (other.gameObject.CompareTag("citcuito"))
        {
            colisionandoConCircuito = true;
            canvas.gameObject.SetActive(false);

        }
        else if (other.gameObject.CompareTag("cesped"))
        {
            colisionandoConCircuito = false;
            canvas.gameObject.SetActive(true);

        }


    }

    // private void OnTriggerExit(Collider collision)
    // {
    //     Debug.Log(collision.gameObject.name);
    //     // Verificar si deja de colisionar con la carretera
    //     if (collision.gameObject.CompareTag("citcuito"))
    //     {
    //         colisionandoConCircuito = false;
    //         // Activar el Canvas
    //         canvas.gameObject.SetActive(true);
    //     }
    // }
}
