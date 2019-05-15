using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Este script controla os botões quando o jogador perde o jogo.

public class GameOverButtons : MonoBehaviour
{
    public TextMeshProUGUI menuText;
    public TextMeshProUGUI lastLevelText;

    public void Start()
    {
        // Mudar a linguagem dos botões
        if(PlayerPrefs.GetString("Language", "en") == "pt")
        {
            menuText.text = "Menu Principal";
            lastLevelText.text = "Carregar último nível";
        }
        else
        {
            menuText.text = "Main Menu";
            lastLevelText.text = "Reload last level";
        }
    }

    // Mudar cena para o menu principal
    public void MainMenu()
    {
        PlayerPrefs.SetInt("Level", 99);
        SceneManager.LoadScene("MenuScene");
    }

    // Repetir o último nível.
    public void ReloadLastLevel()
    {
        PlayerPrefs.SetInt("Health", 5);
        PlayerPrefs.SetInt("LoadDirection", 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
    }
}
