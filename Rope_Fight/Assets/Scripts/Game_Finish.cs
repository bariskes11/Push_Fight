using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Finish : MonoBehaviour
{

    public Game_Play_Manager gm;
    void Start()
    {
        gm = FindObjectOfType<Game_Play_Manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            var r = other.gameObject.GetComponent<PlayerCurrentStatus>();
            if (r != null && r.PlayerFallen)
            {
                return;
            }
        }
        if (other.gameObject.GetComponentInChildren<Animator>() != null)
        {

            string rslt = other.gameObject.GetComponentInChildren<Animator>().tag;
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
}
