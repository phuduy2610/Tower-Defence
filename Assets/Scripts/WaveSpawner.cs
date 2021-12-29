using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wave
{
    public string name; //Tên của wave
    public GameObject enemy; //Lấy prefab enemy ra
    public int amount; //số lượng enemy sẽ spawn
    public float rate; //tốc độ spawn

}
public class WaveSpawner : Singelton<WaveSpawner>
{
    //Các state khác nhau giữa các wave
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    }



    public Wave[] waves;

    //index của wave tiếp theo
    private int nextWave = 0;

    //Thời gian giữa các wave
    public float timeBetweenWaves { get; private set; } = 3f;
    //Đếm ngược đến wave tiếp theo
    public float waveCountdown { get; private set; }

    public SpawnState state { get; set; } = SpawnState.COUNTING;

    private float searchCountdown = 1f;

    public ObjectPool myPool { get; set; }


    // Start is called before the first frame update
    void Start()
    {
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
        if (waveCountdown <= 0)
        {
            //Đang ở state spawn thì bắt đầu spawn
            if (state != SpawnState.SPAWNING)
            {
                StartWave();
                //Start Spawning 
            }
            //Nếu không thì bắt đầu đếm đến wave tiếp theo 

        }
        else
        {
            waveCountdown -= Time.deltaTime;
            Debug.Log(waveCountdown);
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
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);
        //Bắt đầu spawn
        state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.amount; i++)
        {
            //Spawn kẻ thù
            wave.enemy = SpawnEnemy(2);
            wave.enemy.transform.position = LevelCreator.Instance.portal.transform.position;
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
        }
        GameObject enemy = myPool.GetObject(type);
        return enemy;
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed all waves");
        }
        else
        {
            nextWave++;

        }
    }

}
