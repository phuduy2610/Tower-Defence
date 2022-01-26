using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayController : MonoBehaviour
{
    public Toggle fullScreenToggle;
    int currentWidth;
    int currentHeight;
    public TMP_Dropdown resolutionDropdown;
    // Start is called before the first frame update
    private void Awake()
    {
        if (SaveManager.Instance.Settings.screenWidth != 0)
        {
            fullScreenToggle.SetIsOnWithoutNotify(SaveManager.Instance.Settings.fullScreenMode);
            currentWidth = SaveManager.Instance.Settings.screenWidth;
            currentHeight = SaveManager.Instance.Settings.screenHeight;
        } else
        {
            fullScreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
            currentWidth = Screen.width;
            currentHeight = Screen.height;
            SaveManager.Instance.SaveScreen(currentWidth, currentHeight);
            SaveManager.Instance.SaveScreenMode(Screen.fullScreen);
        }

        switch (currentWidth)
        {
            case 1368:
                resolutionDropdown.SetValueWithoutNotify(0);
                break;
            case 1600:
                resolutionDropdown.SetValueWithoutNotify(1);
                break;
            case 1920:
                resolutionDropdown.SetValueWithoutNotify(2);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    public void ChangeFullScene()
    {
        bool value = !Screen.fullScreen;
        Screen.fullScreen = value;
        SaveManager.Instance.SaveScreenMode(value);
    }

    public void ChangeResolution()
    {
        switch (resolutionDropdown.value)
        {
            case 0:
                currentWidth = 1368;
                currentHeight = 768;
                break;
            case 1:
                currentWidth = 1600;
                currentHeight = 900;
                break;
            case 2:
                currentWidth = 1920;
                currentHeight = 1080;
                break;
            default:
                break;
        }
        Screen.SetResolution(currentWidth, currentHeight, Screen.fullScreen);
        SaveManager.Instance.SaveScreen(currentWidth, currentHeight);
    }

    public void OnClose()
    {
        SaveManager.Instance.WriteSettingsData();
    }
}
