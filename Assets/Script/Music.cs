using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    void Awake()
    {
        // Não destruir o objeto que está a reproduzir música.
        DontDestroyOnLoad(this.gameObject);
        
        // Se existir mais que um objeto a reproduzir música eliminar este objeto.
        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
         {             
            Destroy(gameObject);
         }
    }
}
