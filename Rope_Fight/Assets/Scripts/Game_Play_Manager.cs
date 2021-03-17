using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float TotalForce;
    public Rigidbody Force_ApplyField;
    public GameObject Rope;
    public float Bonus_Force_Per_Question;
    public float PlayerForce = 30F;
    public float EnemyForce = 30F;
    public GameObject[] EnemyList;
    public GameObject[] PlayerList;
    private Vector3 Rope_Position;
    public GameObject Red_Pole;
    public GameObject Blue_Pole;
    public GameObject Game_Finish_Panel;
    public Image winImage;
    public Image loseImage;
    public Button NexLevel;
    private enum Win_Lost
    {
        InGame,
        Win,
        Lost,
    }


    private void LateUpdate()
    {
        if (GameStarted)
        {
            TotalForce = PlayerForce - EnemyForce;
            Rope_Position = Rope.transform.position;
            if (TotalForce != 0)
            {
                Rope_Position.z += TotalForce * Time.deltaTime;
            }


            Rope.transform.position = Rope_Position;
            //Force_ApplyField.AddForce(Vector3.forward * TotalForce * Time.deltaTime,ForceMode.VelocityChange);
        }
    
    }
    private void Update()
    {
        if (Game_Finished)
            return;
        if (AskingQuestion)
        {
            QuestionAlgorithms();
        }
        CheckGameStatus();
        if (GameStarted && GenerateTotalList != null)
        {

            CurrentOveralAmount = GenerateTotalList.Sum(x => x.overlapAmount) / GenerateTotalList.Count;
        }
    }

    public void SetGameFinishStatus(string Triggered)
    {
        if (String.IsNullOrEmpty(Triggered))
        {
            EnemyCount = EnemyList.ToList().Where(x => x.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen == false).Count();
            PlayerCount = PlayerList.ToList().Where(x => x.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen == false).Count();
        }
        else if (Triggered == "Player")
        {
            PlayerCount = 0;
        }
        else if (Triggered == "Enemy")
        {
            EnemyCount = 0;
        }


        Game_Finish_Panel.GetComponent<Animator>().SetInteger("Game_Finished", 1);
        if (PlayerCount>0)
        {
            loseImage.GetComponent<Image>().enabled = false;
            winImage.GetComponent<Image>().enabled = true;
            NexLevel.gameObject.SetActive(true);
            currentState = Win_Lost.Win;
        }
        else if (EnemyCount >0)
        {
            loseImage.GetComponent<Image>().enabled = true;
            winImage.GetComponent<Image>().enabled = false;
            NexLevel.gameObject.SetActive(false);
            currentState = Win_Lost.Lost;
        }
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Game_Play_Timing_Scene");
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

    private void InstantiatePoleAndKickPlayer(bool isRed)
    {
        if (isRed) // Bize Çarpan bir yastık.
        {
            GameObject ps = PlayerList.ToList().Where(x => x.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen == false).FirstOrDefault();
            ps.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen = true;
            Vector3 playerPos = ps.transform.position;
            GameObject pol = Instantiate(Red_Pole, playerPos, Quaternion.identity);
            playerPos.y += 2.5F;
            pol.transform.position = playerPos;
            pol.GetComponent<Animator>().SetInteger("PlayKick_Anim", 1);
            Destroy(pol, 3F);
        }
        else // düşlmana çarpan bir yastık
        {
            GameObject es = EnemyList.ToList().Where(x => x.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen == false).FirstOrDefault();
            Vector3 playerPos = es.transform.position;
            es.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen = true;
            GameObject pol = Instantiate(Blue_Pole, playerPos, Quaternion.identity);
            playerPos.y += 2.5F;
            pol.transform.position = playerPos;
            pol.GetComponent<Animator>().SetInteger("PlayKick_Anim", 1);
            Destroy(pol, 3F);



        }
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
        currentState = Win_Lost.InGame;
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
        List<ObjectMovement> moveable_objs = GameObject.FindObjectsOfType<ObjectMovement>().OrderBy(x => x.name).ToList();
        int i = 0;
        foreach (var item in GenerateTotalList)
        {
            item.GetComponent<Image>().sprite = soruSablonu.shapes_dotted[Item_Indexes[i]];
            item.GetComponent<Image>().tag = soruSablonu.Tags[Item_Indexes[i]];
            i++;
        }
        i = 0;
        foreach (var item in moveable_objs)
        {
            item.GetComponent<Image>().color = soruSablonu.shape_Colors[Item_Indexes[i]];
            i++;
        }
    }

    void start_Timer()
    {
        if (this.Game_Finished)
            return;
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
   
    public void FinishlineTouchDown(string Tag)
    {
        if (Tag == "Player")
        {
            currentState = Win_Lost.Lost;
        }
        else if (Tag == "Enemy")
        {
            currentState = Win_Lost.Win;
        }
        Game_Finished = true;
    }

    int EnemyCount;
    int PlayerCount;
    private Win_Lost currentState;
    bool finishShown = false;
    void CheckGameStatus()
    {
        if (currentState != Win_Lost.InGame)
        {
            return;
        }

        EnemyCount = EnemyList.ToList().Where(x => x.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen == false).Count();
        PlayerCount = PlayerList.ToList().Where(x => x.GetComponentInParent<PlayerCurrentStatus>().PlayerFallen == false).Count();
        if (EnemyCount == 0 || PlayerCount == 0)
        {
            Game_Finished = true;
            if (!finishShown)
            {
                finishShown = true;
                SetGameFinishStatus(null);

            }
        }
        if (EnemyCount == 0)
        {
            currentState = Win_Lost.Win;
        }
        else if (PlayerCount == 0)
        {
            currentState = Win_Lost.Lost;
        }

    }

    private void QuestionAlgorithms()
    {
        var elapsedSecond = (DateTime.Now - questionAskedTime).TotalSeconds;
        CurrentTimeLeft = Mathf.Clamp(Timer_Per_Question - float.Parse(elapsedSecond.ToString())
            , 0, Timer_Per_Question);
        Timer_Slider.value = CurrentTimeLeft;
        setColor();
        SetTotalAmounth();
        if (CurrentOveralAmount > MinimumAmouthForAnswer)
        {

            Success_Particle.Simulate(0.0f, true, true);
            Success_Particle.Play();
            PlayerForce -= Bonus_Force_Per_Question;
            EnemyForce += Bonus_Force_Per_Question;
            InstantiatePoleAndKickPlayer(false);
            AskingQuestion = false;
            // Enemy Hizasunda bir Pole Oluşturup Animasyon Yaptırarap Enemy yi düşür.
        }
        else if (Timer_Slider.value <= 0.1F)
        {

            Fail_Particle.Simulate(0.0f, true, true);
            Fail_Particle.Play();
            EnemyForce -= Bonus_Force_Per_Question;
            PlayerForce += Bonus_Force_Per_Question;
            InstantiatePoleAndKickPlayer(true);
            AskingQuestion = false;
            // Player Hizasında bir Pole Oluşturup Animasyon Yaptorarak  Player I

        }
    }

    public void SetTotalAmounth()
    {
        CurrentOveralAmount = GenerateTotalList.Sum(x => x.overlapAmount) / GenerateTotalList.Count;
        CurentFillAmount.text = CurrentOveralAmount.ToString("###.#") + " %";
    }
}
