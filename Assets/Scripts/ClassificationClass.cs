using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassificationClass : MonoBehaviour
{
    // Start is called before the first frame update
   public int clasificacion = 0;

    public List<ClassificationClass> coches;

    private void Start()
    {
        // Obtener una referencia a todos los coches en la carrera
        coches = new List<ClassificationClass>(FindObjectsOfType<ClassificationClass>());
    }

    private void Update()
    {
        // Actualizar la clasificación del coche
        coches.Sort((a, b) => b.transform.position.z.CompareTo(a.transform.position.z));

        for (int i = 0; i < coches.Count; i++)
        {
            if (coches[i] == this)
            {
                clasificacion = i + 1;
                break;
            }
        }
    }
}
