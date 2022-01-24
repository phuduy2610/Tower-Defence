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
    private GameObject loseScene;
    private WaveSpawner waveSpawner;
    private Gate gate;
    private Player player;
    [SerializeField]
    private GameObject musicPlayerObj;
    private AudioSource musicPlayer;
    [SerializeField]
    private AudioClip winSound;
    [SerializeField]
    private AudioClip loseSound;
    [SerializeField]
    private TMP_Text moneyEarn;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        gate = FindObjectOfType<Gate>();
        player = FindObjectOfType<Player>();
        waveSpawner.OnFinish += OnGameWin;
        gate.OnGateDestroy += OnGameLose;
        player.OnPlayerDeath += OnGameLose;
        musicPlayer = musicPlayerObj.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGameLose()
    {
        loseScene.SetActive(true);
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
        victoryScene.SetActive(true);
        musicPlayer.clip = winSound;
        musicPlayer.Play();
        moneyEarn.text = "+" + MenuController.Instance.LevelIndex * 100;
        MenuController.Instance.CurrentMoney += MenuController.Instance.LevelIndex * 100;
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


