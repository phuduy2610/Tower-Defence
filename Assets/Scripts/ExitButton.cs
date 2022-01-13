using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExitButton : MonoBehaviour
{
    Button mybtn;
    // Start is called before the first frame update
    void Start()
    {
        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(MenuController.Instance.QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
