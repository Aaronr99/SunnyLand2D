using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField]
    private Slider sliderMaster;
    [SerializeField]
    private Slider sliderSound;
    [SerializeField]
    private Slider sliderMusic;
    [SerializeField]
    private AudioMixer audioMixer;
    private void Start()
    {
        LoadSliders();
    }
    private void LoadSliders()
    {
        // Carga los sliders
        // Si no existian se establecen los valores predeterminados 
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1);
            PlayerPrefs.Save();
        }
        if (sliderMaster != null)
            sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume");

        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
            PlayerPrefs.Save();
        }
        if (sliderSound != null)
            sliderSound.value = PlayerPrefs.GetFloat("SoundVolume");

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.25f);
            PlayerPrefs.Save();
        }
        if (sliderMusic != null)
            sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume");

        // Se setean los volumenes en el mixer
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
        SetSoundVolume(PlayerPrefs.GetFloat("SoundVolume"));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
    }
    // Se encarga de setear el volumen Master del mixer
    // Y lo guarda en player prefs
    public void SetMasterVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    // Se encarga de setear el volumen Sound del mixer
    // Y lo guarda en player prefs
    public void SetSoundVolume(float sliderValue)
    {
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }
    // Se encarga de setear el volumen Music del mixer
    // Y lo guarda en player prefs
    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void ReanudeGame()
    {
        GameplayManager.Instance.PauseMenu();
    }
    public void CheckPoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReloadLvl()
    {
        PlayerPrefs.DeleteKey("Lifes");
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ConfigMenu()
    {
        SceneManager.LoadScene(2);
    }
    public void Play()
    {
        // Se borran los datos del checkpoint 
        PlayerPrefs.DeleteKey("Lifes");
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        SceneManager.LoadScene(1);
    }
    public void CreditsMenu()
    {
        SceneManager.LoadScene(3);
    }
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
