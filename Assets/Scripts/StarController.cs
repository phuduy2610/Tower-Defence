using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarController : MonoBehaviour
{
    [SerializeField]
    private Tool main;

    [SerializeField]
    private List<GameObject> stars;

    private int current = 0;

    private void OnEnable()
    {
        main.onDestroyed += TunrOffStar;
        main.onUpgrade += Upgrade;
    }

    private void OnDisable()
    {
        main.onDestroyed -= TunrOffStar;
        main.onUpgrade -= Upgrade;
    }

    public void TunrOffStar()
    {
        foreach (var obj in stars)
        {
            obj.SetActive(false);
        }
    }

    public void Upgrade()
    {
        if (current < stars.Count - 1)
        {
            stars[++current].GetComponent<Image>().color = Color.white;
        }
    }
}
