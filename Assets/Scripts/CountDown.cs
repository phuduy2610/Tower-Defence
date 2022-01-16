using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountDown : Singleton<CountDown>
{
    [SerializeField]
    public float TimebeforeWave{get;private set;} = 2.0f;
    private int countdownTime;
    public TMP_Text countDownDisplay;
    // Start is called before the first frame update

    public void StartCountDown(int WaitTime){
        countDownDisplay.gameObject.SetActive(true);
        countDownDisplay.text ="Next wave will start in";
        countdownTime = WaitTime;
        StartCoroutine("CountdownToStart");
    }
    IEnumerator CountdownToStart(){
        yield return new WaitForSeconds(TimebeforeWave);
        while(countdownTime>0){
            countDownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1.0f);
            countdownTime--;
        }
        countDownDisplay.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        countDownDisplay.gameObject.SetActive(false);
        yield break;
    }
}
