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
    // Start is called before the first frame update
    void Start()
    {

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



    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
