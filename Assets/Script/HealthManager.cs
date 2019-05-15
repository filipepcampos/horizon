using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Este script controla a barra de vida do jogador

public class HealthManager : MonoBehaviour
{
    float initialScaleX;
    public int maxHP;
    public float lerpSmooth;
    public int currentHP;
    Player player;
    Transform hpBar;
    Vector3 desiredScale;

    // Inicializar
    void Start()
    {           
        // Obter o componente "Player" do jogador
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        initialScaleX = transform.localScale.x;

        // Carregar vida 
        currentHP = PlayerPrefs.GetInt("Health", 5);
        
        // Inicializar barra de vida com o tamanho correto.
        desiredScale = new Vector3(currentHP * (initialScaleX / maxHP), transform.localScale.y, 1);      
        this.transform.localScale =  new Vector3(currentHP * (initialScaleX / maxHP), transform.localScale.y, 1);   
    }

    void Update()
    {        
        // Se o tamanho da barra de vida não for o desejado, alterar usando Lerp. (Interpolação Linear)
        if(Mathf.Abs(this.transform.localScale.x - desiredScale.x) > 0.005)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, desiredScale, lerpSmooth);
        }    
    }

    public void TakeDamage(int dmg)
    {   
        // Reduzir hp atual, se hp for maior que 0 calcular o tamanho necessário para a barra de vida, caso contrário GameOver().  
        currentHP -= dmg;
        PlayerPrefs.SetInt("Health", currentHP);
        if(currentHP > 0)
        {
            desiredScale = new Vector3(currentHP * (initialScaleX / maxHP), transform.localScale.y, 1);               
        }
        else if(currentHP < 0)
        {
            GameOver();
        }
        else
        {
            desiredScale = new Vector3(currentHP * (initialScaleX / maxHP), transform.localScale.y, 1);            
            GameOver();
        }
    }

    // Ganhar 1 ponto de vida.
    public void GainHealth()
    {        
        if(currentHP < maxHP)
        {           
            currentHP +=1;
            PlayerPrefs.SetInt("Health", currentHP);
            desiredScale = new Vector3(currentHP * (initialScaleX / maxHP), transform.localScale.y, 1); 
        }
    }

    // Carregar a cena "GameOver"
    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
