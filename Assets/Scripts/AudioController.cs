using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class AudioController : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgSlider;
    public Slider seSlider;

    // Start is called before the first frame update
    public AudioMixer audioMixer;
    private void Awake()
    {
        SetVolumn("MasterVolumn", masterSlider);
        SetVolumn("BGVolumn", bgSlider);
        SetVolumn("SEVolumn", seSlider);
    }


    private void SetVolumn(string soundName, Slider slider)
    {
        float volumn;
        if (audioMixer.GetFloat(soundName, out volumn))
        {
            slider.value = volumn;
        }
    }
    public void ChangeMasterVolumn(float volumn)
    {
        audioMixer.SetFloat("MasterVolumn", volumn);
        SaveManager.Instance.SaveMaster(volumn);
    }

    public void ChangeBGVolumn(float volumn)
    {
        audioMixer.SetFloat("BGVolumn", volumn);
        SaveManager.Instance.SaveBackground(volumn);
    }

    public void ChangeSEVolumn(float volumn)
    {
        audioMixer.SetFloat("SEVolumn", volumn);
        SaveManager.Instance.SaveEffect(volumn);
    }

    public void OnClose()
    {
        SaveManager.Instance.WriteSettingsData();
    }
}
