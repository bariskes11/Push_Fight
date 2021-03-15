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
        if (other.gameObject.tag == "Player")
        {
            // show lossse panel
            Debug.Log(" You Lost");
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Debug.Log(" You Win");
            // show win panel
        }
    }
}
