using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
    public static string SETTINGSPATH = Application.persistentDataPath + "/Settings.txt";
    public static string PLAYERPATH = Application.persistentDataPath + "/PlayerData.bin";
    public static string RESWIDTHKEY = "Screenmanager Resolution Width";
    public static string RESHEIGHTKEY = "Screenmanager Resolution Height";
    public const int MONEYDEFAULT = 10000;
    public const int MAXLEVEL = 8;
}
