using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game_Play_Manager : MonoBehaviour
{
    public bool GameStarted = false;
    public bool Game_Finished = false;
    public GameObject gameStartPanel;
    private List<OverLapChecker> GenerateTotalList;
    
    public Text CurentFillAmount;
    public GameObject QuestionPanel;
    public float CurrentTimeLeft;

    public float CurrentOveralAmount;
    // Start is called before the first frame update
    void Start()
    {
        QuestionPanel.SetActive(false);
        gameStartPanel.SetActive(true);

    }
    public void StartGame()
    {
        GameStarted = true;
        gameStartPanel.SetActive(false);
    }
    public void ShowChallenge()
    {
        GenerateTotalList = GameObject.FindObjectsOfType<OverLapChecker>().ToList();
        QuestionPanel.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if(GameStarted)
        CurrentOveralAmount = GenerateTotalList.Sum(x => x.overlapAmount) / GenerateTotalList.Count;
    }

    public void SetTotalAmounth()
    {
        CurrentOveralAmount = GenerateTotalList.Sum(x => x.overlapAmount) / GenerateTotalList.Count;
        CurentFillAmount.text = CurrentOveralAmount.ToString("###.#")+ " %";
    }
}
