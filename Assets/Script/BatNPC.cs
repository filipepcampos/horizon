using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script controla os morcegos.

public class BatNPC : MonoBehaviour
{
    public int hp;
    public int speed;
    public float range;
    public Rigidbody2D playerRb;
    public float colorTime;
    public float knockback;
    public float stunTime;
    float colorCurrentTime;
    float dir;
    bool isKnockedUp;
    bool isFacingRight;
    float characterScale;
    [SerializeField]
    float currentStunTime;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    { 
        // Obter componentes
        rb = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        isFacingRight = false;
        characterScale = this.transform.localScale.x;

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    
    private void FixedUpdate()
    {
        // Se não estiver morto e o jogador estiver dentro de alcance, mover.
        if(!dead)
        {
            if(Vector2.Distance(transform.position, playerRb.position) < range)
            {
                Move();
            }       
        }
       
    }

    void Update()
    {   
        // Verificar se é necessário mover o sprite dependendo da posição do jogador
        RotateSprite();

        // Quando o hp for igual a 0, ativar a animação de morte, desativar o inimigo e dar vida ao jogador.
        if(hp == 0)
        {
            animator.SetTrigger("dead");
            this.gameObject.tag = "Dead";
            dead = true;
            this.gameObject.layer = 12;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 1.0f;
            hp -= 1;
            FindObjectOfType<HealthManager>().GainHealth();
        }

        // Retornar cor do sprite ao normal após ter levado dano
        if(colorCurrentTime <= 0)
        {
            spriteRenderer.color = new Color(1f,1f,1f,1f);
        }     

        // Diminuir os timers, color -> Cor quando leva dano    stunTime -> tempo em que o inimigo é "empurrado" na direção inversa
        colorCurrentTime -= Time.deltaTime;
        currentStunTime -= Time.deltaTime; 
    }

    // Mover inimigo de acordo com posição do jogador
    void Move()
    {     
        Vector3 direction = (playerRb.position - rb.position).normalized;
        if(currentStunTime <= 0 && !dead)
        {
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
        else if(currentStunTime > 0 && !dead)
        {
            rb.MovePosition(transform.position - direction * speed * Time.deltaTime);
        }        
    }  

    // Rodar sprite para estar virado para o jogador.
    void RotateSprite()
    {           
        if(isFacingRight && hp > 0 && (this.transform.position.x - playerRb.position.x < 0) && Vector2.Distance(transform.position, playerRb.position) < range && currentStunTime < 0 && !dead)
        {
            Debug.Log("a");
            transform.localScale = new Vector3(-characterScale, characterScale, characterScale);
            isFacingRight = false;
        }
        else if(!isFacingRight && hp > 0 && (this.transform.position.x - playerRb.position.x > 0) && Vector2.Distance(transform.position, playerRb.position) < range && currentStunTime < 0 && !dead)
        {            
            Debug.Log("b");
            transform.localScale = new Vector3(characterScale, characterScale, characterScale);
            isFacingRight = true;  
        }
    }

    // Levar dano
    public void TakeDamage()
    {       
        hp -= 1;        
        spriteRenderer.color = new Color(0.33f, 0.17f, 0.17f, 1); 
        colorCurrentTime = colorTime;
        currentStunTime = stunTime;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Quando entrar em contacto com o chão após a morte rodar o sprite e destruir o Rigidbody2D (Desativando as simulações físicas)
        if(other.gameObject.tag == "Platform" && dead)
        {
            transform.rotation = Quaternion.Euler(0,0,-45f);
            Destroy(rb);
        }
    }

    // Representação visual do alcance do inimigo no editor.
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
