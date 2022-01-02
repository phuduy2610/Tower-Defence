using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountDown : Singelton<CountDown>
{
    private int countdownTime;
    [SerializeField]
    private TMP_Text countDownDisplay;
    // Start is called before the first frame update

    public void StartCountDown(int WaitTime){
        countDownDisplay.gameObject.SetActive(true);
        countdownTime = WaitTime;
        StartCoroutine("CountdownToStart");
    }
    IEnumerator CountdownToStart(){
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
