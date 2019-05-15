using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwnAttack;
    public float startTimeBtwAttack;
    Animator anim;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    InGameOptions inGameOptions;
    public AudioSource attackAudio;
    
    void Start()
    {
        // Obter componentes da personagem
        anim = GetComponent<Animator>(); 
        inGameOptions = GetComponent<InGameOptions>();       
    }

    void Update()
    {
        if(timeBtwnAttack <= 0)
        {
            // Se o jogo não se encontrar em pausa, iniciar a animação de ataque e dar dano aos inimigos que se encontram dentro do alcance da personagem.
            if(Input.GetButtonDown("Attack") && !inGameOptions.gamePaused)
            {     
                anim.SetTrigger("attack");   
                attackAudio.Play(); 
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length ; i++)
                {
                  
                    if(enemiesToDamage[i].tag == "EnemyWolf")
                    {
                        enemiesToDamage[i].GetComponent<WolfNPC>().TakeDamage();
                    }
                    else if(enemiesToDamage[i].tag == "EnemyBat")
                    {
                        enemiesToDamage[i].GetComponent<BatNPC>().TakeDamage();
                    }                                        
                }
                timeBtwnAttack = startTimeBtwAttack;
            }            
        }
        else
        {
            timeBtwnAttack -= Time.deltaTime;
        }     
    }
    
    // Visualização do alcance do ataque
    void OnDrawGizmosSelected() 
    {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
