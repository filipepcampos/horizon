using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script controla a ativação / desativação do menu de pausa dentro do jogo.

public class InGameOptions : MonoBehaviour
{
    GameObject panel;
    Rigidbody2D[] rigBodies;
    Animator[] animators;
    public bool gamePaused = false;

    // Inicializar
    void Start()
    {
        gamePaused = false;    
        panel = GameObject.Find("Panel");
        panel.SetActive(false);  
    }
 
    // Se a tecla "Esc" for clicada: Pausar jogo ou Continuar jogo dependendo da situação atual.
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && gamePaused)
        {
            Resume();
        }
        else if (Input.GetButtonDown("Cancel") && !gamePaused)
        {
            Pause();
        }
    }

    // Ativar o menu de pausa e parar todos os Rigidbodies2D
    void Pause()
    {
        Time.timeScale = 0;
        gamePaused = true;
        panel.SetActive(true);
        rigBodies = FindObjectsOfType<Rigidbody2D>();
        animators = FindObjectsOfType<Animator>();    

        for(int i = 0; i < rigBodies.Length; i++)
        {
            rigBodies[i].constraints = RigidbodyConstraints2D.FreezeAll;
        }
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = false;
        }
    }

    // Continuar o jogo
    public void Resume()
    {
        Time.timeScale = 1;
        gamePaused = false;
        panel.SetActive(false);
        rigBodies = FindObjectsOfType<Rigidbody2D>();
        animators = FindObjectsOfType<Animator>();
        for(int i = 0; i < rigBodies.Length; i++)
        {
            if(rigBodies[i].gameObject.tag == "Dead")
            {
                rigBodies[i].constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                rigBodies[i].constraints = RigidbodyConstraints2D.FreezeRotation;
            }            
        }
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = true;
        }
    }
}
