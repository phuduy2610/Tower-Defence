using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class VictoryLoseManager : Singelton<VictoryLoseManager>
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

    // Update is called once per frame
    void Update()
    {

    }

    void OnGameLose()
    {
        loseScene.SetActive(true);
        Time.timeScale = 0f;
        if(player.death){
            loseTxt.text = "YOU DIED";
        }
    }

    void OnGameWin()
    {
        victoryScene.SetActive(true);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Restart(){
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void NextScene(){
        MenuController.Instance.LevelIndex++;
        Restart();
    }
}


