using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class Wave
{
    public string name; //Tên của wave
    public int[] enemies; //Chứa các index loại enemy sẽ xuất hiện trong wave đó
    public int amount; //số lượng enemy sẽ spawn trong mỗi wave
    public float rate; //tốc độ spawn

    public Wave(string Name, int[] Enemies, int Amount, float Rate)
    {
        name = Name;
        enemies = Enemies;
        amount = Amount;
        rate = Rate;
    }
}
public class WaveSpawner : Singelton<WaveSpawner>
{
    //Các state khác nhau giữa các wave
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING,
        FINISH
    }


    [SerializeField]
    private TMP_Text waveTxt;
    [SerializeField]
    private GameObject startBtn;

    [SerializeField]
    private GameObject victoryScene;

    private bool isPressed = false;

    public LevelWaveInfo levelWaveInfo;

    //index của wave tiếp theo
    private int nextWave = 0;
    public int NextWave
    {
        get
        {
            return nextWave;
        }
        set
        {
            nextWave = value;
            if (nextWave <= 9)
            {
                waveTxt.text = "0" + (nextWave + 1).ToString();
            }
            else
            {
                waveTxt.text = (nextWave + 1).ToString() + "0";

            }
        }
    }

    //Thời gian giữa các wave
    public float timeBetweenWaves { get; private set; } = 10f;
    //Đếm ngược đến wave tiếp theo
    public float waveCountdown { get; private set; }

    public SpawnState state { get; set; } = SpawnState.COUNTING;

    private float searchCountdown = 1f;

    public ObjectPool myPool { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        levelWaveInfo = new LevelWaveInfo(1);
        myPool = GetComponent<ObjectPool>();
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            //Xét xem enemy còn sống không
            if (!EnemyIsAlive())
            {
                //Begin a new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }
        if (isPressed)
        {
            if (waveCountdown <= 0)
            {
                //Đang ở state spawn thì bắt đầu spawn
                if (state != SpawnState.SPAWNING && state !=SpawnState.FINISH)
                {
                    StartWave();
                    //Start Spawning 
                }
                //Nếu không thì bắt đầu đếm đến wave tiếp theo 

            }
            else
            {
                waveCountdown -= Time.deltaTime;
                //Debug.Log(waveCountdown);
            }
        }
    }

    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    private void StartWave()
    {
        Debug.Log("start");
        StartCoroutine(SpawnWave(levelWaveInfo.waves[nextWave]));
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);
        //Bắt đầu spawn
        state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.amount; i++)
        {
            //Spawn kẻ thù
            int enemyIndex = wave.enemies[Random.Range(0, wave.enemies.Length)];
            SpawnEnemy(enemyIndex).transform.position = LevelCreator.Instance.portal.transform.position;
            yield return new WaitForSeconds(wave.rate);
        }
        //Sau khi spawn xong thì vô trạng thái đợi
        state = SpawnState.WAITING;


        yield break;
    }


    private GameObject SpawnEnemy(int monsterIndex)
    {
        string type = string.Empty;
        switch (monsterIndex)
        {
            case 0:
                type = "Normal";
                break;
            case 1:
                type = "Assassin";
                break;
            case 2:
                type = "Tank";
                break;
            case 3:
                type = "Fly";
                break;
        }
        GameObject enemy = myPool.GetObject(type);
        return enemy;
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if (nextWave + 1 > levelWaveInfo.waves.Count - 1)
        {
            victoryScene.SetActive(true);
            Debug.Log("Completed all waves");
            state = SpawnState.FINISH;
        }
        else
        {
            CountDown.Instance.StartCountDown(((int)timeBetweenWaves));
            NextWave++;
        }
    }

    public void IsPressStart()
    {
        isPressed = true;
        CountDown.Instance.StartCountDown(((int)timeBetweenWaves));
        startBtn.SetActive(false);
    }


}
