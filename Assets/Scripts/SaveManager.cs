using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : PersistentSingleton<SaveManager>
{
    private SettingsData _settings = new SettingsData();

    public SettingsData Settings { get => _settings;}

    public void GetLoadData(SettingsData settingsData)
    {
        if(settingsData != null)
            _settings = settingsData;
    }

    public void SaveScreen(int width, int height)
    {
        _settings.screenWidth = width;
        _settings.screenHeight = height;
    }

    public void SaveScreenMode(bool mode)
    {
        _settings.fullScreenMode = mode;
    }

    public void SaveEffect(float se)
    {
        _settings.effectVolume = se;
    }

    public void SaveBackground(float bg)
    {
        _settings.backgroundVolume = bg;
    }

    public void SaveMaster(float master)
    {
        _settings.masterVolume = master;
    }

    public void WriteSettingsData()
    {
        if(_settings.screenWidth == 0 || _settings.screenHeight == 0)
        {
            _settings.screenWidth = Screen.width;
            _settings.screenHeight = Screen.height;
        }
        var jsonString = JsonUtility.ToJson(_settings);
        File.WriteAllText(Constant.SETTINGSPATH, jsonString);
    }

    public void SavePlayerData(int level, int money, bool[] weapons, int select)
    {
        PlayerData playerData = new PlayerData();
        playerData.currentLevel = level;
        playerData.money = money;
        playerData.weapons = weapons;
        playerData.currentSelect = select;
        FileStream file = new FileStream(Constant.PLAYERPATH, FileMode.Create, FileAccess.Write);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, playerData);
        file.Close();
    }

    [Serializable]
    public class SettingsData
    {
        public int screenWidth = 0;
        public int screenHeight = 0;
        public bool fullScreenMode;
        public float masterVolume;
        public float effectVolume;
        public float backgroundVolume;
    }

    [Serializable]
    public class PlayerData
    {
        public int currentLevel = 0;
        public int money = 0;
        public bool[] weapons;
        public int currentSelect;
    }
}
