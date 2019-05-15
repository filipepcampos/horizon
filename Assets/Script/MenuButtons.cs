using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Botões do menu principal

public class MenuButtons : MonoBehaviour
{
    public Button loadButton;
    public Button quitButton;
    public TextMeshProUGUI newGameText;
    public TextMeshProUGUI loadGameText;
    public TextMeshProUGUI optionsText;
    public TextMeshProUGUI quitText;
    public Slider musicSlider;

    void Start()
    {
        // Se o nível atual for igual a 99 impedir Load
        if(PlayerPrefs.GetInt("Level", 99) == 99)
        {
            loadButton.interactable = false;
        }

        // Mudar a linguagem
        if(PlayerPrefs.GetString("Language", "en") == "pt")
        {
            newGameText.text = "Novo Jogo";
            loadGameText.text = "Continuar Jogo";
            optionsText.text = "Opções";
            quitText.text = "Sair";
        }
        else
        {
            newGameText.text = "New Game";
            loadGameText.text = "Load Game";
            optionsText.text = "Options";
            quitText.text = "Quit";
        } 

        // Se o jogo estiver a ser jogado num browser retirar o botão de Sair do jogo
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Destroy(quitButton.gameObject);
        }      
    }

    // Carregar um novo jogo
    public void NewGame()
    {
        PlayerPrefs.SetString("IsNewGame", "true");
        PlayerPrefs.SetString("FirstLoad", "true");
        PlayerPrefs.SetInt("Health", 5);
        PlayerPrefs.SetInt("Level", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);      
    }

    // Continuar um jogo anterior
    public void LoadGame()
    {
        PlayerPrefs.SetString("IsNewGame", "false");
        PlayerPrefs.SetString("FirstLoad", "true");
        PlayerPrefs.SetInt("LoadDirection", 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1));        
    }

    // Abrir a cena de Options
    public void Options()
    {
        SceneManager.LoadScene("OptionsScene");      
    }

    // Sair do jogo
    public void Quit()
    {
        Application.Quit();
    }
}
