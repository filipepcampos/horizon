// Este script controla praticamente todas as ações do jogador.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Declaração de variáveis
    public float moveSpeed;
    public float jumpHeight;
    public HealthManager hpManager;    
    public float slowMotionTime;
    float slowMotionCurrentTime;
    public float colorTime;
    float colorCurrentTime;
    public float imunityTime;
    float imunityCurrentTime;
    public Vector2 leftPosition;
    public Vector2 rightPosition; 
    public float characterScale; 
    public float knockUpX;
    public float knockUpY;
    bool isGamePaused;
    Rigidbody2D rigBody;
    SpriteRenderer spriteRenderer;
    public Animator animator;
    bool isJumping;
    bool isFacingRight;
    [SerializeField] bool isKnockedUp;    
    Vector2 fallPosition;
    InGameOptions inGameOptions;   
    public AudioSource jumpAudio;
    public AudioSource hurtAudio;

    void Start()
    {
        //Carregar dados do jogador 
        if(PlayerPrefs.GetString("FirstLoad", "true") == "true")
        {  
            PlayerPrefs.SetString("FirstLoad", "false");           
        }  
        if(PlayerPrefs.GetString("IsNewGame", "true") == "false")
        {
             PlayerPrefs.SetString("IsNewGame", "false"); 
        }   
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        
        // Obter RigidBody2D da personagem.
        rigBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        inGameOptions = GetComponent<InGameOptions>();        

        // Colocar o jogador no lado direito ou esquerdo do mapa baseado na localização anterior
        if(PlayerPrefs.GetInt("LoadDirection") == -1)
        {
           rigBody.position = rightPosition;
           isFacingRight = false;
           transform.localScale = new Vector3(-characterScale, characterScale, characterScale);
        }
        else
        {
            rigBody.position = leftPosition;
            isFacingRight = true;
            transform.localScale = new Vector3(characterScale, characterScale, characterScale);
        }
        rigBody.velocity = new Vector2(moveSpeed * PlayerPrefs.GetInt("LoadDirection"), 0);
    }

    // Movimento lateral da personagem
    private void FixedUpdate()
    {
        isGamePaused = inGameOptions.gamePaused;
        // Mover personagem dependendo do input. 
        if(!isKnockedUp && !isGamePaused)
        {
            rigBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rigBody.velocity.y);
            animator.SetFloat("velocity", Mathf.Abs(rigBody.velocity.x));
        }
        else if(isKnockedUp && !isGamePaused)
        {
            animator.SetFloat("velocity",0);
        }        

        // Rodar personagem conforme a sua velocidade
        if(rigBody.velocity.x < 0 && isFacingRight &&  !isGamePaused && !isKnockedUp)
        {
            transform.localScale = new Vector3(-characterScale, characterScale, characterScale);
            isFacingRight = false;
        }
        else if(rigBody.velocity.x > 0 && !isFacingRight && !isGamePaused && !isKnockedUp)
        {            
            transform.localScale = new Vector3(characterScale, characterScale, characterScale);
            isFacingRight = true;
        }
    }

    void Update()
    {  
        isGamePaused = inGameOptions.gamePaused;
        // Se a personagem não estiver a saltar, se o jogador clicar "Space" ou "UpArrow", saltar.
        if (Input.GetButtonDown("Jump") && !isJumping && !isKnockedUp && !isGamePaused)
        {
            rigBody.velocity = new Vector2(rigBody.velocity.x, jumpHeight);
            isJumping = true;
            animator.SetBool("jumping", true);
            jumpAudio.Play();
        }

        // Atualizar timers de slow motion e mudança de cor.
        slowMotionCurrentTime -= Time.deltaTime;
        colorCurrentTime -= Time.deltaTime;
        imunityCurrentTime -= Time.deltaTime;

        // Desativar slow motion passado um dado tempo.
        if (slowMotionCurrentTime <= 0)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }

        // Desativar mudança de cor passado um dado tempo
        if (colorCurrentTime <= 0)
        {
            spriteRenderer.color = new Color(1,1,1,1);
        }

        if (colorCurrentTime + 0.14f <= 0)
        {
            isKnockedUp = false;
        }
     
    }

    // Quando a personagem levar dano, começar slowmotion, alterar cor do jogador e reduzir vida do jogador.
    void TakeDamage(int dmg, float dir, float y)
    {
        // Se o jogador ainda estiver em período de imunidade não irá receber dano. A menos que seja dano de queda.
        if(imunityCurrentTime * y <= 0)
        {
            // Slowmotion
            Time.timeScale = 0.4f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            slowMotionCurrentTime = slowMotionTime * y; 

            hpManager.TakeDamage(dmg);
            spriteRenderer.color = new Color(0.33f, 0.17f, 0.17f, 1); 
            colorCurrentTime = colorTime; 
            imunityCurrentTime = imunityTime; 

            Knockback(dir, y); 
            hurtAudio.Play();
        }         
    } 

    // Lançar jogador para trás após um ataque.
    void Knockback(float dir, float y)
    {
        if (!isKnockedUp)
        {
            Debug.Log($"Knocking back. x:{dir*moveSpeed*knockUpX}, y: {knockUpY * y}");
            rigBody.velocity = new Vector2(dir* moveSpeed * knockUpX, knockUpY *y);
            isKnockedUp = true;
        }
    }      
  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Registar quando a personagem toca no chão.
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
            isKnockedUp = false;
            animator.SetBool("jumping", false);
        }

        // Levar dano quando entra em contacto com algum inimigo
        if (collision.gameObject.tag == "EnemyWolf" || collision.gameObject.tag == "EnemyBat")
        {            
            TakeDamage(1, Mathf.Sign(transform.position.x - collision.transform.position.x), 1);            
        }
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        // Segunda verificação para o jogador não entrar num estado onde não se pode mover,
        // no caso de ser atacado mas não sair do chão.
        if (other.gameObject.tag == "Platform" && colorCurrentTime < 0 && slowMotionCurrentTime < 0)
        {
            isKnockedUp = false;
        }

        if (other.gameObject.tag == "EnemyWolf" || other.gameObject.tag == "EnemyBat")
        {            
            TakeDamage(1, Mathf.Sign(transform.position.x - other.transform.position.x), 1);            
        }
    }
 
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Carregar nível anterior ou próximo nível dependendo do trigger.
        if (other.gameObject.tag == "LoadDir")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("LoadDirection", 1);
        }
        if (other.gameObject.tag == "LoadEsq")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            PlayerPrefs.SetInt("LoadDirection", -1);
        }

        // Se entrar no trigger com tag "FinalAnimation" ativar a animação final do jogo
        if (other.gameObject.tag == "FinalAnimation")
        {
            Time.timeScale = 0;
            this.rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
            inGameOptions.gamePaused = true;

            animator.Play("finalAnimation");
            GameObject.Find("BlackScreenImage").GetComponent<Animator>().Play("final");
            GameObject.Find("CM vcam1").GetComponent<Animator>().Play("teste");
            GameObject.Find("EmptyHealthBar").transform.position = new Vector3(9999,9999,9999);
            GameObject.Find("FullHealthBar").transform.position = new Vector3(9999,9999,9999);
        }

        // Causar dano ao jogador quando cai fora do mapa e returnar-lo ao checkpoint anterior
        if (other.gameObject.tag == "Fall")
        {
            TakeDamage(1,0, 0);
            gameObject.transform.position = fallPosition;
        }
        if (other.gameObject.tag == "FallCheckpoint")
        {
            fallPosition = gameObject.transform.position;
        }

        // Conjunto de verificações para o jogador não conseguir saltar no meio do ar.
        if(other.gameObject.tag == "JumpCheck")
        {
            isJumping = true;
        }
    }
}
