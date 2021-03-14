using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovements : MonoBehaviour
{
    public int Focused_Player_Index = 0;
    private Game_Play_Manager gm = new Game_Play_Manager();
    public Vector3 playerOffset;
    private Vector3 PlayerPosition;
    private GameObject playerObj;
    public int CurrentPlayerIndex;
    private PlayerIndexer[] PlayersList;
    private GameObject currentPlayerToZoom;
    public bool QuestionTime = false;
    public bool QuestionResulted = false;
    public float QuestionTimeOut=5F;
    Camera c;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<Game_Play_Manager>();
        c = Camera.main;
        CurrentPlayerIndex = 0;
        PlayersList = FindObjectsOfType<PlayerIndexer>();
        playerObj = PlayersList.Where(x => x.PlayerIndex == CurrentPlayerIndex).FirstOrDefault().gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (gm.GameStarted)
        {
            CameraPositioning();
        }
    }
    void CameraPositioning()
    {
        if (gm.Game_Finished)
        {
            transform.LookAt(playerObj.transform);
            transform.Translate(Vector3.right * Time.deltaTime);
            return;
        }
        PlayerPosition = Vector3.MoveTowards(c.transform.position, playerObj.transform.position + playerOffset, .1F);
        //PlayerPosition = playerObj.transform.position + playerOffset;
        if (c.transform.position == playerObj.transform.position + playerOffset && !QuestionTime)
        {
            AskQuestion();
        }
        c.transform.position = PlayerPosition;
    }
    void AskQuestion()
    {
        QuestionTime = true;
        StartCoroutine(WaitForAnswerTimeout());
    }

    IEnumerator WaitForAnswerTimeout()
    {
        playerObj = PlayersList.Where(x => x.PlayerIndex == CurrentPlayerIndex).FirstOrDefault().gameObject;
       var rslt= playerObj.GetComponentInChildren<QuestionCreator>().CreateQuestion();
        gm.ShowChallenge(rslt);
        yield return new WaitForSeconds(QuestionTimeOut);
        playerObj.GetComponentInChildren<QuestionCreator>().CloseQuestion();
        gm.SetTotalAmounth();
        CurrentPlayerIndex++;
        if (CurrentPlayerIndex > PlayersList.Count()-1)
        {
            CurrentPlayerIndex =0;
        }
        
        QuestionTime = false;
    }
}






