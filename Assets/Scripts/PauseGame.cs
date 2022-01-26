using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseGame : MonoBehaviour
{
    [SerializeField]
    private List<PlayerInput> inputs;

    [SerializeField]
    GameObject pauseMenuUI;
    public static bool GameisPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        inputs.Add(LevelCreator.Instance.character.GetComponent<PlayerInput>());
    }

    // Update is called once per frame
    void Update()
    {   
        if(Keyboard.current[Key.Escape].wasPressedThisFrame){
            if(GameisPaused){
                Resume();
            }    
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        foreach (var obj in inputs)
        {
            obj.actions.Enable();
        }
    }
    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true; 
        foreach (var obj in inputs)
        {
            obj.actions.Disable();
        }
    }

    public void Exit(){
        Resume();
        SceneManager.LoadScene("Menu");
    }
}
