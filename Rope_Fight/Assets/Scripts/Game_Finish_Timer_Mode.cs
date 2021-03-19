using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Finish_Timer_Mode : MonoBehaviour
{
    private Timing_Game_Play_Manager gm;
    void Start()
    {
        gm = FindObjectOfType<Timing_Game_Play_Manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        string rslt = other.gameObject.tag;
        Debug.Log("Finish_line Triggered" + other.gameObject.tag + " " + rslt);
        if (rslt == "Player")
        {
            // show lossse panel
            //  gm.Game_Finished = true;
            gm.SetGameFinishStatus(rslt);
            //Debug.Log(" You Lost");
        }
        else if (rslt == "Enemy")
        {
            //    gm.Game_Finished = true;
            gm.SetGameFinishStatus(rslt);
            //Debug.Log(" You Win");

            // show win panel
        }
    }




}
