using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Este script muda a linguagem da cena final, e termina o jogo levando o jogador até o menu principal.

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI endText;

    // Mudar linguagem
    public void Start()
    {
        if(PlayerPrefs.GetString("Language", "pt") == "en")
        {
            endText.text = "The End";
        }
        else
        {
            endText.text = "O Fim";
        }

    }
    // Mudar cena para "MenuScene"
    public void MainMenu()
    {
        PlayerPrefs.SetInt("Level", 99);
        SceneManager.LoadScene("MenuScene");
    }
}
