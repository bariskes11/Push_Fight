using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Timing_Game_Play_Manager : MonoBehaviour
{
    public GameObject CurrentPlayer;
    public GameObject TimingPrefab;
    public float CurrentTimeLeft;
    public bool GameStarted = false;
    public GameObject GameStartPanel;
    public GameObject GameResultPanel;
    public GameObject GamePlayMessagePanel;
    private GameObject[] Players;
    public Slider Timer_Slider;
    public float SuccessMoveDistance;
    private DateTime Challange_Started;
    public float Time_Per_Challange;
    private int CurrentPlayerIndex;
    private void Start()
    {
        CurrentPlayerIndex = 0;
        GameStartPanel.SetActive(true);
        GamePlayMessagePanel.SetActive(false);
        Players = GameObject.FindGameObjectsWithTag("Player").OrderBy(x => x.name).ToArray();
        if (Players.Length == 0)
        {
            Debug.Log("Player Bulunamadı");
        }
    }
    public void StartGame()
    {
        GameStartPanel.SetActive(false);
        GamePlayMessagePanel.SetActive(true);
        ActivateCurrentPlayer(0);
    }
    private void ActivateCurrentPlayer(int Index)
    {
       var r=  GameObject.FindGameObjectsWithTag("TimingBar").ToArray();
        foreach (var item in r)
        {
            Destroy(item);
        }
        CurrentPlayer = Players[Index];
        GameObject gtimer=Instantiate(TimingPrefab, CurrentPlayer.transform);
        gtimer.transform.localPosition = new Vector3(-0.1F, 1.6F, 0);
        
    }
    public void Update()
    {
        if (GameStarted)
        {
            var elapsedSecond = (DateTime.Now - Challange_Started).TotalSeconds;
            CurrentTimeLeft = Mathf.Clamp(Time_Per_Challange - float.Parse(elapsedSecond.ToString()), 0, Time_Per_Challange);
            Timer_Slider.value = CurrentTimeLeft;
            if (Timer_Slider.value <= 0.1F)
            {
                if (CurrentPlayerIndex < Players.Length)
                    CurrentPlayerIndex++;
                else
                    CurrentPlayerIndex = 0;
                ActivateCurrentPlayer(CurrentPlayerIndex);
            }

        }
    }
    public void Tabbed()
    {
       float Result=  CurrentPlayer.GetComponentInChildren<Timing_Bar>().Barpressed();
        if (Mathf.Abs(Result) <= 1F) // iyi
        {
            //playerAnimator.SetBool
        }
        else if (Mathf.Abs(Result) >= 3) // çok kötü 
        {

        }
        StartCoroutine(WaitFornextPlayer());
    }

    IEnumerator WaitFornextPlayer()
    {
        yield return new WaitForSeconds(1F);
        
        if (CurrentPlayerIndex < Players.Length)
            CurrentPlayerIndex++;
        else
            CurrentPlayerIndex = 0;
        ActivateCurrentPlayer(CurrentPlayerIndex);


    }
}
