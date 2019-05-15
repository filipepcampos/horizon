using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script tem como propósito modificar o salto, tornando-o menos realistico aumentando a
//velocidade de queda.

public class JumpModifier : MonoBehaviour
{
    private Rigidbody2D rb;
    [Range(0,5)] public float fallmultiplier = 3.2f;
    [Range(0,5)] public float lowFallMultiplier = 2.0f;

    // Inicializar, obtendo Rigidbody2D da personagem.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Se a personagem estiver a cair utilizar a gravidade correspondente.
        if(rb.velocity.y < 0){
            rb.gravityScale = fallmultiplier;
        }
        // Se a personagem estiver a subir utilizar a gravidade correspondente.
        else if(rb.velocity.y > 0){
            rb.gravityScale = lowFallMultiplier;
        }
        else{
            rb.gravityScale = 1f;
        }
    }
}
