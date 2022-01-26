using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class VictoryLoseManager : Singleton<VictoryLoseManager>
{
    [SerializeField]
    private TMP_Text loseTxt;
    [SerializeField]
    private GameObject victoryScene;
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject loseScene;
    private WaveSpawner waveSpawner;
    private Gate gate;
    private Player player;
    [SerializeField]
    private AudioSource musicPlayer;
    [SerializeField]
    private AudioClip winSound;
    [SerializeField]
    private AudioClip loseSound;
    [SerializeField]
    private TMP_Text moneyEarn;
    [SerializeField]
    private GameObject nextButton;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        gate = FindObjectOfType<Gate>();
        player = FindObjectOfType<Player>();
        waveSpawner.OnFinish += OnGameWin;
        gate.OnGateDestroy += OnGameLose;
        player.OnPlayerDeath += OnGameLose;
    }

    void OnGameLose()
    {
        MenuController.Instance.SavePlayerData();
        loseScene.SetActive(true); 
        inventory.SetActive(false);
        Time.timeScale = 0f;
        if (player.death)
        {
            loseTxt.text = "YOU DIED";
        }
        musicPlayer.clip = loseSound;
        musicPlayer.Play();
    }

    void OnGameWin()
    {
        if (MenuController.Instance.LevelIndex < Constant.MAXLEVEL)
        {
            if (MenuController.Instance.CurrentLevel == MenuController.Instance.LevelIndex)
            {
                MenuController.Instance.CurrentLevel++;
            }
        } else
        {
            nextButton.SetActive(false);
        }
        MenuController.Instance.SavePlayerData();
        victoryScene.SetActive(true);
        musicPlayer.clip = winSound;
        musicPlayer.Play();
        moneyEarn.text = "+" + MenuController.Instance.LevelIndex * 100;
        MenuController.Instance.CurrentMoney += MenuController.Instance.LevelIndex * 100;
        inventory.SetActive(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void NextScene()
    {   
        MenuController.Instance.LevelIndex++;
        Restart();
    }
}


