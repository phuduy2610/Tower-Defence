using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class LoadManager : PersistentSingleton<LoadManager>
{
    public AudioMixer audioMixer;

    [SerializeField]
    private MenuController controller;

    [SerializeField]
    private Shopping shopping;

    protected override void Awake()
    {
        SaveManager.PlayerData playerData = ReadPlayerData();
        if (playerData != null)
        {
            controller.ArrowBought = playerData.weapons;
            controller.CurrentMoney = playerData.money;
            controller.CurrentLevel = playerData.currentLevel;
            controller.WeaponSelected = playerData.currentSelect;
            controller.ArrowSprite = shopping.ArrowSprites[playerData.currentSelect];
            controller.CharDamage = shopping.ArrowDamage[playerData.currentSelect];
            shopping.CurrentArrowImage.GetComponent<Image>().sprite = controller.ArrowSprite;
        } else
        {
            controller.CurrentMoney = Constant.MONEYDEFAULT;
            controller.CharDamage = shopping.ArrowDamage[0];
            controller.Setup();
        }
        shopping.Setup();

        SaveManager.SettingsData settingsData = ReadSettingsData();
        SaveManager.Instance.GetLoadData(settingsData);
        if (settingsData != null)
        {
            Screen.SetResolution(settingsData.screenWidth, settingsData.screenHeight, settingsData.fullScreenMode);
            audioMixer.SetFloat("MasterVolumn", settingsData.masterVolume);
            audioMixer.SetFloat("BGVolumn", settingsData.backgroundVolume);
            audioMixer.SetFloat("SEVolumn", settingsData.effectVolume);
        } else
        {
            float value;
            audioMixer.GetFloat("MasterVolumn", out value);
            SaveManager.Instance.SaveMaster(value);
            audioMixer.GetFloat("BGVolumn", out value);
            SaveManager.Instance.SaveBackground(value);
            audioMixer.GetFloat("SEVolumn", out value);
            SaveManager.Instance.SaveEffect(value);
        }
    }

    private SaveManager.PlayerData ReadPlayerData()
    {
        SaveManager.PlayerData result;
        try
        {
            SaveManager.PlayerData playerData = new SaveManager.PlayerData();
            FileStream file = new FileStream(Constant.PLAYERPATH, FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            result = formatter.Deserialize(file) as SaveManager.PlayerData;
            file.Close();
        }
        catch
        {
            return null;
        }
        return result;
    }

    private SaveManager.SettingsData ReadSettingsData()
    {
        if (File.Exists(Constant.SETTINGSPATH))
        {
            var fileString = File.ReadAllText(Constant.SETTINGSPATH);
            return JsonUtility.FromJson<SaveManager.SettingsData>(fileString);
        }
        return null;
    }
}
