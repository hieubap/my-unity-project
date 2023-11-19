using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainMenu;
    public GameObject gameplay;
    
    public PauseDialog pauseDialog;
    public TimeoutDialog timeoutDialog;
    public CompletedDialog completedDialog;
    private GameState state;


    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGameplay(bool isShow){
        if(gameplay)
        gameplay.SetActive(isShow);

        if(mainMenu)
        mainMenu.SetActive(!isShow);
    }

}
