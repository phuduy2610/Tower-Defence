using System.Collections;
using TMPro;
using UnityEngine;
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
public class WaveSpawner : Singleton<WaveSpawner>
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

    public bool isPressed { get; private set; } = false;

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

    //Dùng khi đã hoàn thành hết wave
    public event System.Action OnFinish;

    // Start is called before the first frame update
    void Start()
    {
        levelWaveInfo = new LevelWaveInfo(MenuController.Instance.LevelIndex);
        myPool = GetComponent<ObjectPool>();
        waveCountdown = timeBetweenWaves + CountDown.Instance.TimebeforeWave;
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
                StartCoroutine("WaveCompleted");
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
                if (state != SpawnState.SPAWNING && state != SpawnState.FINISH)
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
            //Lấy random kẻ thù
            int enemyIndex = wave.enemies[Random.Range(0, wave.enemies.Length)];
            //Lấy random cổng 
            int portalIndex = Random.Range(0, LevelCreator.Instance.portal.Count);
            //Spawn kẻ thù
            var result = SpawnEnemy(enemyIndex);
            SetStartPos(result.Item1, result.Item2, portalIndex);
            yield return new WaitForSeconds(wave.rate);
        }
        //Sau khi spawn xong thì vô trạng thái đợi
        state = SpawnState.WAITING;


        yield break;
    }

    // set vị trí của enemy , ifRand là nếu quái có được spawn random hay không
    private void SetStartPos(GameObject enemy, bool ifRand, int portalIndex)
    {
        if (ifRand)
        {
            float leftSide = LevelCreator.Instance.topLeftTile.x;
            float topSide = LevelCreator.Instance.topLeftTile.y;
            float bottomSide = LevelCreator.Instance.bottomRightTile.y;
            float rightSide = LevelCreator.Instance.bottomRightTile.x;

            var enemyPos = Vector2.zero;

            switch (Random.Range(0, 3))
            {
                case 0:
                    //top
                    enemyPos.y = topSide;
                    enemyPos.x = Random.Range(leftSide, rightSide);
                    break;
                case 1:
                    //bottom
                    enemyPos.y = bottomSide - LevelCreator.Instance.TileSize;
                    enemyPos.x = Random.Range(leftSide, rightSide);
                    break;
                case 2:
                    //left
                    enemyPos.x = leftSide;
                    enemyPos.y = Random.Range(topSide, bottomSide);
                    break;
            }
            enemy.transform.position = enemyPos;
        }
        else
        {
            enemy.transform.position = LevelCreator.Instance.portal[portalIndex].transform.position;
        }
    }

    private (GameObject, bool) SpawnEnemy(int monsterIndex)
    {
        string type = string.Empty;
        bool rand = false;
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
                rand = true;
                break;
            case 4:
                type = "Range";
                break;
            case 5:
                type = "Boss";
                break;
            case 6:
                type = "Boss 1";
                break;
        }
        GameObject enemy = myPool.GetObject(type);
        return (enemy, rand);
    }

    private IEnumerator WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves + CountDown.Instance.TimebeforeWave + 3.0f;
        CountDown.Instance.countDownDisplay.gameObject.SetActive(true);
        CountDown.Instance.countDownDisplay.text = "Wave Complete";
        yield return new WaitForSeconds(3.0f);
        if (nextWave + 1 > levelWaveInfo.waves.Count - 1)
        {
            Debug.Log("Completed all waves");
            state = SpawnState.FINISH;
            if (OnFinish != null)
            {
                OnFinish();
            }
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
