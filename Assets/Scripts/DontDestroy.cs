using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Si ya existe otra musica en background destruye esta
        if (GameObject.FindGameObjectsWithTag("BackGround").Length > 1)
            Destroy(gameObject);
    }

}
