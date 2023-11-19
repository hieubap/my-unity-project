using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeoutDialog : Dialog
{
    public void BackToMenu(){
        if(SceneController.Ins)
            SceneController.Ins.LoadCurrentScene();
    }

    public void Replay(){
        SceneManager.sceneLoaded += OnSceneLoadedEvent;
        if(SceneController.Ins)
            SceneController.Ins.LoadCurrentScene();
    }

    public void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode){
        if(GameManagement.Ins)
            GameManagement.Ins.Playgame();
        
        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
    }
}
