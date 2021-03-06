using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWaveInfo
{
    public int levelIndex { get; private set; }
    public List<Wave> waves { get; private set; } = new List<Wave>();

    public LevelWaveInfo(int LevelIndex)
    {
        levelIndex = LevelIndex;
        switch (levelIndex)
        {
            case 1:
                //Tạo 3 wave cho level đầu
                for (int i = 0; i < 3; i++)
                {
                    int[] enemyIndex = { 0 };
                    Wave temp = new Wave(LevelName(i), enemyIndex, 10 * (i + 1), 1.0f);
                    waves.Add(temp);
                }
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    int[] enemyIndex = { 0, 1, 2 };
                    Wave temp = new Wave(LevelName(i), enemyIndex, 10 * (i + 1), 1.0f);
                    waves.Add(temp);
                }
                break;
            case 3:
                for (int i = 0; i < 1; i++)
                {
                    int[] enemyIndex = new int[1];
                    enemyIndex[0] = 5;
                    Wave temp = new Wave(LevelName(i), enemyIndex, 1, 1.0f);
                    waves.Add(temp);
                }
                break;
            case 4:
                for (int i = 0; i < 3; i++)
                {
                    int[] enemyIndex = { 0, 1, 2, 3};
                    Wave temp = new Wave(LevelName(i), enemyIndex, 10 * (i + 1), 1.0f);
                    waves.Add(temp);
                }
                break;
            case 5:
                for (int i = 0; i < 3; i++)
                {
                    int[] enemyIndex = { 0, 1, 2, 3, 4 };
                    Wave temp = new Wave(LevelName(i), enemyIndex, 10 * (i + 1), 1.0f);
                    waves.Add(temp);
                }
                break;
            case 6:
                for (int i = 0; i < 3; i++)
                {
                    int[] enemyIndex = { 0, 1, 2, 3, 4 };
                    Wave temp = new Wave(LevelName(i), enemyIndex, 10 * (i + 1), 1.0f);
                    waves.Add(temp);
                }
                break;
            case 7:
                for (int i = 0; i < 1; i++)
                {
                    int[] enemyIndex = { 6 };
                    Wave temp = new Wave(LevelName(i), enemyIndex, 1, 1.0f);
                    waves.Add(temp);
                }
                break;
            case 8:
                for (int i = 0; i < 3; i++)
                {
                    int[] enemyIndex = { 0, 1, 2, 3, 4 };
                    Wave temp = new Wave(LevelName(i), enemyIndex, 10 * (i + 1), 1.0f);
                    waves.Add(temp);
                }
                break;
            default:
                break;
        }
    }

    private string LevelName(int waveIndex)
    {
        switch (waveIndex)
        {
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

