using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayButton : MonoBehaviour
{
    Button mybtn;
    // Start is called before the first frame update
    void Start()
    {
        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(MenuController.Instance.PlayScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
