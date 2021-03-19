using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timing_Game_Play_Manager : MonoBehaviour
{

    public float CurrentTimeLeft;
    public float Time_Per_Challange;
    public float SuccessMoveDistance;
    public ParticleSystem Success_Particle;
    public ParticleSystem Fail_Particle;
    public Color[] WarnColors;
    public Image FillPart;
    public bool GameStarted = false;
    public GameObject GameStartPanel;
    public GameObject GameResultPanel;
    public GameObject GamePlayMessagePanel;
    public GameObject RopeObject;
    public GameObject CurrentPlayer;
    public GameObject TimingPrefab;
    public GameObject Game_Finish_Panel;
    private GameObject[] Players;
    public Slider Timer_Slider;
    public Image winImage;
    public Image loseImage;


    private DateTime Challange_Started;
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
        GameStarted = true;
        Timer_Slider.value = Time_Per_Challange;
        CurrentTimeLeft = Time_Per_Challange;
        Challange_Started = DateTime.Now;
    }
    private void ActivateCurrentPlayer(int Index)
    {
        var r = GameObject.FindGameObjectsWithTag("TimingBar").ToArray();
        foreach (var item in r)
        {
            Destroy(item);
        }
        CurrentPlayer = Players[Index];
        GameObject gtimer = Instantiate(TimingPrefab, CurrentPlayer.transform);
        gtimer.transform.localPosition = new Vector3(-0.1F, 1.6F, 0);
    }


    public void ReloadScene()
    {
        string SName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SName);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
    public void SetGameFinishStatus(string Triggered)
    {

        Game_Finish_Panel.GetComponent<Animator>().SetInteger("Game_Finished", 1);
        if (Triggered == "Enemy")
        {
            loseImage.GetComponent<Image>().enabled = false;
            winImage.GetComponent<Image>().enabled = true;


        }
        else
        {
            loseImage.GetComponent<Image>().enabled = true;
            winImage.GetComponent<Image>().enabled = false;

        }
        this.GameStarted = false;
    }


    public void Update()
    {

        if (GameStarted)
        {
            var elapsedSecond = (DateTime.Now - Challange_Started).TotalSeconds;
            CurrentTimeLeft = Mathf.Clamp(Time_Per_Challange - float.Parse(elapsedSecond.ToString()), 0, Time_Per_Challange);
            setColor();
            Timer_Slider.value = CurrentTimeLeft;
            if (Timer_Slider.value <= 0.1F)
            {
                if (CurrentPlayerIndex < Players.Length - 1)
                    CurrentPlayerIndex++;
                else
                    CurrentPlayerIndex = 0;
                ActivateCurrentPlayer(CurrentPlayerIndex);
                ResetTimer();
                Tabbed(true);
            }
        }
        Move_Rope_ToDirection();
    }
    void ResetTimer()
    {
        CurrentTimeLeft = Time_Per_Challange;
        Timer_Slider.value = Time_Per_Challange;
        Challange_Started = DateTime.Now;
    }

    void Move_Rope_ToDirection()
    {
        if (MoveMultiPlayer == 0)
            return;

        float rst = SuccessMoveDistance * MoveMultiPlayer;
        RopeObject.transform.Translate(new Vector3(0F, rst * Time.deltaTime, 0F));
    }

    private void setColor()
    {
        float val = ((100 * CurrentTimeLeft) / Time_Per_Challange) / 100;
        if (val >= 0.75F)
        {
            FillPart.color = WarnColors[0];
        }
        else if (val < 0.75F && val >= 0.5F)
        {
            FillPart.color = WarnColors[1];
        }
        else if (val < 0.5F && val >= 0.25F)
        {
            FillPart.color = WarnColors[2];
        }
        else if (val < 0.25F && val >= 0F)
        {
            FillPart.color = WarnColors[3];
        }

    }


    float MoveMultiPlayer = 0;
    public void Tabbed(bool timerFailed)
    {
        ResetTimer();
        float Result = CurrentPlayer.GetComponentInChildren<Timing_Bar>().Barpressed();
        Debug.Log("Result:" + Result);
        if (timerFailed)
            Result = 4; // kötü
        if (Mathf.Abs(Result) <= 1F) // iyi
        {
            foreach (var item in Players)
            {
                item.GetComponentInChildren<Animator>().SetTrigger("Pull_Back");
            }

            Success_Particle.Simulate(0.0f, true, true);
            Success_Particle.Play();
            MoveMultiPlayer = -1;
        }
        else if (Mathf.Abs(Result) >= 3) // çok kötü 
        {
            foreach (var item in Players)
            {
                item.GetComponentInChildren<Animator>().SetTrigger("Push_Rope");

            }
            MoveMultiPlayer = 1;
            Fail_Particle.Simulate(0.0f, true, true);
            Fail_Particle.Play();
        }
        else
        {
            MoveMultiPlayer = 0;
        }
        StartCoroutine(WaitFornextPlayer());
    }

    IEnumerator WaitFornextPlayer()
    {


        yield return new WaitForSeconds(1F);

        if (CurrentPlayerIndex < Players.Length - 1)
            CurrentPlayerIndex++;
        else
            CurrentPlayerIndex = 0;
        ActivateCurrentPlayer(CurrentPlayerIndex);
        yield return new WaitForSeconds(.5F);
        MoveMultiPlayer = 0;


    }
}
