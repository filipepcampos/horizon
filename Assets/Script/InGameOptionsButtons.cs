using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

// Botões do menu de pausa

public class InGameOptionsButtons : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI effectsText;
    public TextMeshProUGUI continueText;
    public TextMeshProUGUI menuText;
    public Slider musicSlider;
    public Slider effectsSlider;

    public void Start()
    {
        // Mudar a linguagem dos botões.
        if (PlayerPrefs.GetString("Language", "en") == "pt")
        {
            PlayerPrefs.SetString("Language", "pt");
            musicText.text = "Música";
            effectsText.text = "Efeitos";
            continueText.text = "Continuar";
            menuText.text = "Menu Principal";
        }
        else
        {
            PlayerPrefs.SetString("Language", "en");
            musicText.text = "Music";
            effectsText.text = "Effects";
            continueText.text = "Continue";
            menuText.text = "Main Menu"; 
        }

        // Inicializar o slider de música com o tamanho adequado ao volume atual
        musicMixer.GetFloat("volume",out float vm);
        musicSlider.value = Mathf.Pow(10, vm/20);

        effectsMixer.GetFloat("volume", out float ve);
        effectsSlider.value = Mathf.Pow(10, ve/20);
        effectsSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("effectsVol", 1f) / 20);
    }

    // Mudar volume utilizando o slider de música
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }  

    public void SetEffectsVolume(float volume)
    {
        effectsMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVol", Mathf.Log10(volume) * 20);
    } 

    // Continuar jogo
    public void Continue()
    {
        FindObjectOfType<InGameOptions>().Resume();
    }

    // Carregar menu principal
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
