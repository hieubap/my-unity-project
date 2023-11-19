using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CompletedDialog : Dialog
{
    public Text totalMoveTxt;
    public Text bestMoveTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);

        if(totalMoveTxt && GameManagement.Ins){
            totalMoveTxt.text = GameManagement.Ins.TotalMoving.ToString();
        }
        
        if(bestMoveTxt)
            bestMoveTxt.text = Pref.bestMove.ToString();
    }
    public void Continue(){
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
