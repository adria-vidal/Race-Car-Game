using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapsCount: MonoBehaviour
{
    public bool verificador = false;
    public bool meta = false;
    public int vueltas;

    public static LapsCount instance;

    private void Start() {
        if (instance == null){
            instance = this;
        }
        vueltas = 1;
    }



    
}
