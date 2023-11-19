using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : Singleton<GameManagement>
{
    public int timeLimit;
    public MatchItem[] matchItems;
    public MatchItemUI itemUIPb;
    public Transform gridRoot;
    public GameState state;
    private List<MatchItem> m_matchItemsCopy;
    private List<MatchItemUI> m_matchItemsUIs;
    private List<MatchItemUI> m_answer;

    private float m_timeCounting;
    private int m_totalMatchItem;
    private int m_totalMoving;
    private int m_rightMoving;
    private bool m_isAnswerChecking;

    public int RightMoving { get => m_rightMoving;}
    public int TotalMoving { get => m_totalMoving;}

    public Image filled;

    public override void Awake()
    {
        MakeSingleton(false);
        m_matchItemsCopy = new List<MatchItem>();
        m_matchItemsUIs = new List<MatchItemUI>();
        m_answer = new List<MatchItemUI>();
        m_timeCounting = timeLimit;
        state = GameState.Starting;
    }

    public override void Start()
    {
        Application.targetFrameRate = 30;
        base.Start();
        if(AudioController.Ins)
            AudioController.Ins.PlayBackgroundMusic();
    }

    private void Update() {
        UpdateTime();
    }

    public void Playgame(){
        state = GameState.Playing;
        GenerateMatchItems();
        if(UIManager.Ins)
        UIManager.Ins.ShowGameplay(true);
    }

    public void Exitgame(){
        Application.Quit();
    }

    private void GenerateMatchItems(){
        if(matchItems == null || matchItems.Length <= 0 || itemUIPb == null || gridRoot == null) return;

        int totalItem = matchItems.Length;
        int divItem = totalItem % 2;
        m_totalMatchItem = totalItem - divItem;

        for (int i = 0; i < m_totalMatchItem; i++)
        {
            var matchItem = matchItems[i];
            if(matchItem != null){
                matchItem.Id = i;
            }
        }

        m_matchItemsCopy.AddRange(matchItems);
        m_matchItemsCopy.AddRange(matchItems);
        
        ShuffleMatchItems();
        ClearGrid();

        for (int i = 0; i < m_matchItemsCopy.Count; i++)
        {
            var matchItem = m_matchItemsCopy[i];
            var matchItemUIClone = Instantiate(itemUIPb, Vector3.zero, Quaternion.identity);
            matchItemUIClone.transform.SetParent(gridRoot);
            matchItemUIClone.transform.localPosition = Vector3.zero;
            matchItemUIClone.transform.localScale = Vector3.one;
            matchItemUIClone.UpdateFirstState(matchItem.icon);
            matchItemUIClone.Id = matchItem.Id;
            m_matchItemsUIs.Add(matchItemUIClone);

            if(matchItemUIClone.btnComp){
                matchItemUIClone.btnComp.onClick.RemoveAllListeners();
                matchItemUIClone.btnComp.onClick.AddListener(() => {
                    if(m_isAnswerChecking) return;

                    m_answer.Add(matchItemUIClone);
                    matchItemUIClone.OpenAnimTrigger();
                    if(m_answer.Count == 2){
                        m_totalMoving++;
                        m_isAnswerChecking = true;
                        StartCoroutine(CheckAnswerCo());
                    }

                    matchItemUIClone.btnComp.enabled = false;
                });
            }
        }
    }

    private IEnumerator CheckAnswerCo(){
        bool isRight = m_answer[0] != null && m_answer[1] != null 
                    && m_answer[0].Id == m_answer[1].Id;
                    
        yield return new WaitForSeconds(1f);
        Debug.Log("isRight"+ isRight + ","+(m_answer[0] != null)+","+(m_answer[1] != null));
        if(m_answer != null && m_answer.Count == 2){
            if(isRight){
                m_rightMoving++;
                if(AudioController.Ins)
                AudioController.Ins.PlaySound(AudioController.Ins.right);
            }else{
                if(AudioController.Ins)
                AudioController.Ins.PlaySound(AudioController.Ins.wrong);
            }
            foreach (MatchItemUI answer in m_answer){
                if(answer){
                    if(isRight){
                        answer.ExplodeAnimTrigger();
                    }else{
                        answer.OpenAnimTrigger();
                    }
                }
            }
        }
        m_answer.Clear();
        m_isAnswerChecking = false;

        if(m_rightMoving == m_totalMatchItem && UIManager.Ins){
            Pref.bestMove = m_totalMoving;
            UIManager.Ins.completedDialog.Show(true);
            if(AudioController.Ins)
                AudioController.Ins.PlaySound(AudioController.Ins.gameover);
        }
    }
    private void ShuffleMatchItems(){
        if(m_matchItemsCopy == null || m_matchItemsCopy.Count <= 0) return;
        
        for (int i = 0; i < m_matchItemsCopy.Count; i++)
        {
            var temp = m_matchItemsCopy[i];
            if(temp != null){
                int randomIdx = Random.Range(0, m_matchItemsCopy.Count);
                m_matchItemsCopy[i] = m_matchItemsCopy[randomIdx];
                m_matchItemsCopy[randomIdx] = temp;
            }
        }
    }

    private void ClearGrid(){
        if(gridRoot == null) return;

        for (int i = 0; i < gridRoot.childCount; i++)
        {
            var child = gridRoot.GetChild(i);
            if(child){
                Destroy(child.gameObject);
            }
        }
    }

    private void UpdateTime(){
        if(state != GameState.Playing) return;

        m_timeCounting -= Time.deltaTime;
        if(m_timeCounting <= 0 && state != GameState.Timeout){
            state = GameState.Timeout;
            m_timeCounting = 0;
            if(UIManager.Ins)
            UIManager.Ins.timeoutDialog.Show(true);
            
            if(AudioController.Ins)
                AudioController.Ins.PlaySound(AudioController.Ins.timeOut);
        }else{
            filled.fillAmount = (m_timeCounting / timeLimit);
        }
    }
}
