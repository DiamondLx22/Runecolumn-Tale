using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class AudioMixerSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string parameterName;
    public void SetValue(float value)
    {
        mixer.SetFloat(parameterName, value);
    }
}
