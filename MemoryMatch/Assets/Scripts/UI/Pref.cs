using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pref
{
    public static int bestMove{
        set{
            int oldMove = PlayerPrefs.GetInt(PrefKey.BestScore.ToString(), 0);
            if(oldMove > value || oldMove == 0){
                PlayerPrefs.SetInt(PrefKey.BestScore.ToString(), value);
            }
        }
        get => PlayerPrefs.GetInt(PrefKey.BestScore.ToString(), 0);
    }

}
