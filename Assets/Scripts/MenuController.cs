using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : PersistentSingleton<MenuController>
{
    [SerializeField]
    private LevelBlocker levelBlocker;

    [SerializeField]
    private int currentLevel;

    [SerializeField]
    private int _levelIndex;

    private int weaponSelected;

    public int LevelIndex
    {
        get
        {
            return _levelIndex;
        }
        set
        {
            _levelIndex = value;
        }
    }
    [SerializeField]
    private Sprite defaultArrow;

    [SerializeField]
    private int _currentMoney = -1;

    public int CurrentMoney
    {
        get
        {
            return _currentMoney;
        }
        set
        {
            _currentMoney = value;
        }
    }

    private Sprite arrowSprite;

    //Chứa tên đang sử dụng
    public Sprite ArrowSprite
    {
        get
        {
            return arrowSprite;
        }
        set
        {
            arrowSprite = value;
        }
    }

    public bool[] ArrowBought { get => arrowBought; set => arrowBought = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public int WeaponSelected { get => weaponSelected; set => weaponSelected = value; }

    //Chứa xem tên nào đã được mua rồi
    private bool[] arrowBought = new bool[6];

    // Start is called before the first frame update
    public void Setup()
    {
        arrowSprite = defaultArrow;
        weaponSelected = 0;
    }

    public void PlayScene()
    {
        _levelIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        LoadController loadController = FindObjectOfType<LoadController>();
        loadController.StartLoadScene();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void SavePlayerData()
    {
        SaveManager.Instance.SavePlayerData(this.CurrentLevel, this.CurrentMoney,this.ArrowBought, this.WeaponSelected);
    }
}
