using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

// Menu de opções

public class Options : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;
    public Slider musicSlider;
    public Slider effectsSlider;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI effectsText;
    public TextMeshProUGUI returnText;
    public TextMeshProUGUI portugues;
    public TextMeshProUGUI english;

    public void Start()
    {
        // Alterar a linguagem.
        if(PlayerPrefs.GetString("Language", "en") == "pt")
        {
            PlayerPrefs.SetString("Language","pt");
            musicText.text = "Música";
            effectsText.text = "Efeitos";
            returnText.text = "Voltar";
            portugues.color = new Color(1,1,1,1);
            english.color = new Color(0.5f, 0.5f, 0.5f,1);
        }
        else
        {
            PlayerPrefs.SetString("Language","en");
            musicText.text = "Music";
            effectsText.text = "Effects";
            returnText.text = "Return";
            english.color = new Color(1,1,1,1);
            portugues.color = new Color(0.5f, 0.5f, 0.5f,1);
        }        

        // Inicializar o valor do slider de acordo com o volume da música atualmente.
        musicMixer.GetFloat("volume",out float vm);
        musicSlider.value = Mathf.Pow(10, vm/20);
        

        effectsMixer.GetFloat("volume",out float ve);
        effectsSlider.value = Mathf.Pow(10, ve/20);
        effectsSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("effectsVol", 1f) / 20);
    }

    // Mudar volume da música utilizando slider
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVol", Mathf.Log10(volume) * 20);
    }

    // Alterar linguagem para português.
    public void Portugues()
    {
        PlayerPrefs.SetString("Language","pt");
        musicText.text = "Música";
        effectsText.text = "Efeitos";
        returnText.text = "Voltar";
        portugues.color = new Color(1,1,1,1);
        english.color = new Color(0.5f, 0.5f, 0.5f,1);

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    // Alterar linguagem para inglês.
    public void English()
    {
        PlayerPrefs.SetString("Language","en");
        musicText.text = "Music";
        effectsText.text = "Effects";
        returnText.text = "Return";
        english.color = new Color(1,1,1,1);
        portugues.color = new Color(0.5f, 0.5f, 0.5f,1);

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    // Voltar para o menu principal.
    public void Return()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
