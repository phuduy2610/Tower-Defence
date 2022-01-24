using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : PersistentSingleton<MenuController>
{

    [SerializeField]
    private int _levelIndex;

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
    private int _currentMoney;

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

    //Chứa xem tên nào đã được mua rồi
    public bool[] arrowBought = new bool[6];

    // Start is called before the first frame update
    void Start()
    {
        _levelIndex = 0;
        arrowSprite = defaultArrow;
    }

    // Update is called once per frame
    void Update()
    {

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

}
