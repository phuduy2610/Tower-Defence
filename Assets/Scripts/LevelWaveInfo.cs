using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWaveInfo 
{
    public int levelIndex{get; private set;}
    public List<Wave> waves{get;private set;} = new List<Wave>();

    public LevelWaveInfo(int LevelIndex){
        levelIndex = LevelIndex;
        switch(levelIndex){
            case 1:
            //Tạo 3 wave cho level đầu
                for(int i=0;i<3;i++){
                    int[] enemyIndex = {0,1,2};
                    Wave temp = new Wave(LevelName(i),enemyIndex,4,3.0f);
                    waves.Add(temp);
                }
            break;
            case 2:
            break;
            case 3:
            break;
            default:
            break;
        }
    }

    private string LevelName(int waveIndex){
        switch(waveIndex){
            case 0:
            return "They're Coming...";
            case 1:
            return "Second Wave...";
            case 2:
            return "Final Wave!";
            default:
            return "Something is here";
        }

    }
}
    
