using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider Volumeslider;
    public AudioSource audioSource;  // Die spezifische AudioSource für die Musik

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            LoadVolume();
        }
        else
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
            LoadVolume();
        }

        // Registriere den Slider-Event-Hörer
        Volumeslider.onValueChanged.AddListener(SetVolume);
    }

    // Setze die Lautstärke der spezifischen AudioSource
    public void SetVolume(float volume)
    {
        Debug.Log("Slider Value: " + volume);  // Ausgabe des aktuellen Slider-Wertes
        audioSource.volume = volume;  // Lautstärke der AudioSource anpassen
        SaveVolume(volume);
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("soundVolume");
        Volumeslider.value = volume;
        audioSource.volume = volume;  // Stelle sicher, dass die AudioSource den gespeicherten Wert verwendet
    }
}