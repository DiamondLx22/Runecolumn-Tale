using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour{
    public bool option_menu = false;
    public GameObject option_screen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckOptionScreen();
    }
    void CheckOptionScreen(){
        if (option_menu==true){
            option_screen.SetActive(true);
        }else{
            option_screen.SetActive(false);
        }



    }








    public void StartGame()
    {
        SceneManager.LoadScene("MainWorld");
     

    }
    public void QuitGame(){
        Application.Quit();


    }
    public void Option(){
        option_menu = true;
    
    }
    public void CloseOptionscreen(){
        option_menu = false;
    
    }
}

