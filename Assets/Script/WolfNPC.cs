// Este script controla os inimigos (Non Playable Characters)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfNPC : MonoBehaviour
{
    public int hp;
    public int moveSpeed;
    public float range;
    public Rigidbody2D playerRb;
    public float colorTime;

    float colorCurrentTime;
    float dir;
    bool isKnockedUp;
    bool isFacingRight;
    float characterScale;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    // Obter o Rigidbody2D do NPC
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        isFacingRight = true;
        characterScale = this.transform.localScale.x;
    }

    void Update()
    {
        // Se hp for menor ou igual a 0, ativar animação de morte, desativar este inimigo e dar vida ao jogador.
        if(hp == 0)
        {
            animator.SetTrigger("dead");
            this.gameObject.tag = "Dead";
            this.gameObject.layer = 8;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            hp -= 1;
            FindObjectOfType<HealthManager>().GainHealth();
        }

        // Rodar sprite para estar sempre virado para o jogador
        RotateSprite();
        
        // Mover NPC baseado na posição do jogador
        if(!isKnockedUp)
        {
            Move(playerRb.position.x);            
        }

        // Se o timer chegar a 0 retornar cor do inimigo ao normal
        if(colorCurrentTime <= 0)
        {
            spriteRenderer.color = new Color(1f,1f,1f,1f);
        }

        // Diminuir timer
        colorCurrentTime -= Time.deltaTime;    

        dir = - Mathf.Sign(rb.velocity.x);  

        animator.SetFloat("vel", Mathf.Abs(rb.velocity.x));
    }

    void Move(float x)
    {   
        // Se o NPC estiver à direita do jogador, mover para a esquerda
        if(rb.position.x > x && Mathf.Abs(rb.position.x - x) <= range)
        {            
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        // Caso contrário, mover para a direita
        else if(rb.position.x < x && Mathf.Abs(rb.position.x - x) <= range)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void RotateSprite()
    {    
        if(rb.velocity.x < 0 && isFacingRight && hp > 0 && !isKnockedUp && Mathf.Abs(transform.position.x - playerRb.transform.position.x) <= range 
            && Mathf.Abs(transform.position.x - playerRb.transform.position.x) > 0.05 && -Mathf.Sign(transform.position.x - playerRb.transform.position.x) == -1)
        {   
            transform.localScale = new Vector3(-characterScale, characterScale, characterScale);
            isFacingRight = false;
        }

        else if(rb.velocity.x > 0 && !isFacingRight && hp > 0 && !isKnockedUp && Mathf.Abs(transform.position.x - playerRb.transform.position.x) <= range 
            && Mathf.Abs(transform.position.x - playerRb.transform.position.x) > 0.05 && -Mathf.Sign(transform.position.x - playerRb.transform.position.x) == 1)
        {   
            transform.localScale = new Vector3(characterScale, characterScale, characterScale);
            isFacingRight = true;
        }
    }

   

    // Receber Dano
    public void TakeDamage()
    {       
        hp -= 1;
        animator.SetTrigger("hurt");
        spriteRenderer.color = new Color(0.33f, 0.17f, 0.17f, 1); 
        Knockback(dir);
        colorCurrentTime = colorTime;
    }

    // Impulso para trás após receber dano
    void Knockback(float dir)
    {      
        dir = rb.position.x - playerRb.position.x;
        rb.velocity = new Vector2(dir*2, 2);
        isKnockedUp = true;        
    }      
  
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {            
            isKnockedUp = false;
            if(isFacingRight)
            {
                rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);
            }   
            else
            {
                rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y);
            }        
        } 

        if (other.gameObject.tag == "Player")
        {
            rb.velocity = new Vector2(-Mathf.Sign(transform.position.x - playerRb.transform.position.x) * 0.1f, 0);
        }     
    }    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Se entrar num trigger do jogador chamado "PlayerAttack" ativar a animação de ataque.
        if (other.gameObject.tag == "PlayerAttack")
        {
            animator.SetTrigger("attack");
        }  
    }

    // Visualização no editor do alcance do inimigo
    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(gameObject.transform.position.x - range, gameObject.transform.position.y, gameObject.transform.position.z)
        , new Vector3(gameObject.transform.position.x + range, gameObject.transform.position.y, gameObject.transform.position.z) );
    }
}
