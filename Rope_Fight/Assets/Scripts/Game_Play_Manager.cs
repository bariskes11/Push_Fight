using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game_Play_Manager : MonoBehaviour
{
    public bool GameStarted = false;
    public bool Game_Finished = false;
    public float MinimumAmouthForAnswer = 80F;
    public GameObject gameStartPanel;
    private List<OverLapChecker> GenerateTotalList;
    private List<ObjectMovement> MovebleObjects;
    public ParticleSystem Success_Particle;
    public ParticleSystem Fail_Particle;
    public Text CurentFillAmount;
    public GameObject QuestionPanel;
    public float CurrentTimeLeft;
    public QuestionScriptable soruSablonu;
    public List<Vector3> initialPositions;
    public float CurrentOveralAmount;
    public Slider Timer_Slider;
    public float Timer_Per_Question;
    private DateTime questionAskedTime;
    public bool AskingQuestion = false;
    public Color[] WarnColors;
    public Image FillPart;
    // genel duruma göre ileri veya geri gideceğiz
    public float TotalForce;
    public Rigidbody Force_ApplyField;
    public GameObject Rope;
    public float Bonus_Force_Per_Question;
    public float PlayerForce = 30F;
    public float EnemyForce = 30F;
    public GameObject[] EnemyList;
    public GameObject[] PlayerList;
    private Vector3 Rope_Position;
    private void LateUpdate()
    {
        TotalForce = PlayerForce - EnemyForce;
        Rope_Position = Rope.transform.position;
        if(TotalForce!=0)
        {
            Rope_Position.z += TotalForce*Time.deltaTime;
        }

        
        Rope.transform.position = Rope_Position;
        //Force_ApplyField.AddForce(Vector3.forward * TotalForce * Time.deltaTime,ForceMode.VelocityChange);
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        PlayerList = GameObject.FindGameObjectsWithTag("Player");
        GenerateTotalList = GameObject.FindObjectsOfType<OverLapChecker>().ToList();
        MovebleObjects = GameObject.FindObjectsOfType<ObjectMovement>().ToList();
        SetPositions();
        QuestionPanel.SetActive(false);
        gameStartPanel.SetActive(true);
        Timer_Slider.minValue = 0;
        Timer_Slider.maxValue = Timer_Per_Question;

    }
    private void SetPositions()
    {

        if (initialPositions.Count() == 0)
        {
            foreach (var item in MovebleObjects)
            {
                initialPositions.Add(item.GetComponent<RectTransform>().localPosition);
            }
        }
        else
        {
            int i = 0;
            foreach (var item in MovebleObjects)
            {
                RectTransform r = item.GetComponent<RectTransform>();
                r.localPosition = initialPositions.ElementAt(i);
                i++;
            }
        }

        foreach (var item in GenerateTotalList)
        {
            item.ResetFillAmounth();
        }

    }
    public void StartGame()
    {
        GameStarted = true;
        gameStartPanel.SetActive(false);
    }
    public void ShowChallenge(int[] Item_Indexes)
    {


        SetPositions();
        CurrentTimeLeft = Timer_Per_Question;
        AskingQuestion = true;
        CurrentOveralAmount = 0;
        CurentFillAmount.text = "0 %";
        questionAskedTime = DateTime.Now;
        QuestionPanel.SetActive(true);
        start_Timer();
        GenerateTotalList = GameObject.FindObjectsOfType<OverLapChecker>().OrderBy(x => x.name).ToList();
        int i = 0;
        foreach (var item in GenerateTotalList)
        {
            item.GetComponent<Image>().sprite = soruSablonu.shapes_dotted[Item_Indexes[i]];
            item.GetComponent<Image>().tag = soruSablonu.Tags[Item_Indexes[i]];
            i++;
        }
    }

    void start_Timer()
    {
        Timer_Slider.value = Timer_Per_Question;
        CurrentTimeLeft = Timer_Per_Question;
    }
    private void setColor()
    {
        float val = ((100 * CurrentTimeLeft) / Timer_Per_Question) / 100;
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

    // Update is called once per frame
    void Update()
    {
        if (AskingQuestion)
        {
            var elapsedSecond = (DateTime.Now - questionAskedTime).TotalSeconds;
            CurrentTimeLeft = Mathf.Clamp(Timer_Per_Question - float.Parse(elapsedSecond.ToString())
                , 0, Timer_Per_Question);
            Timer_Slider.value = CurrentTimeLeft;
            setColor();
            SetTotalAmounth();
            if (CurrentOveralAmount > MinimumAmouthForAnswer)
            {
                AskingQuestion = false;
                Success_Particle.Simulate(0.0f, true, true);
                Success_Particle.Play();
                PlayerForce += Bonus_Force_Per_Question;

                // Enemy Hizasunda bir Pole Oluşturup Animasyon Yaptırarap Enemy yi düşür.
            }
            else if (Timer_Slider.value == 0)
            {
                AskingQuestion = false;
                Fail_Particle.Simulate(0.0f, true, true);
                Fail_Particle.Play();
                PlayerForce -= Bonus_Force_Per_Question;
                // Player Hizasında bir Pole Oluşturup Animasyon Yaptorarak  Player I

            }
        }

        if (GameStarted && GenerateTotalList != null)
        {

            CurrentOveralAmount = GenerateTotalList.Sum(x => x.overlapAmount) / GenerateTotalList.Count;
        }
    }

    public void SetTotalAmounth()
    {
        CurrentOveralAmount = GenerateTotalList.Sum(x => x.overlapAmount) / GenerateTotalList.Count;
        CurentFillAmount.text = CurrentOveralAmount.ToString("###.#") + " %";
    }
}
