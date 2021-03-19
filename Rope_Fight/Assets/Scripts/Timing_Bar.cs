using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timing_Bar : MonoBehaviour
{
    public float MoveDistance = 6F;
    public float Turn_Speed = 6F;
    public GameObject ok;
    public bool Pressed = false;
    private Timing_Game_Play_Manager gm;
    private Animator playerAnimator;
    private void Start()
    {
        gm = GameObject.FindObjectOfType<Timing_Game_Play_Manager>();
    }
    private void Update()
    {
        if (!gm.GameStarted)
            return;
        if (Pressed == false)
        {
            Vector3 mov = new Vector3(Mathf.Sin(Time.time * Turn_Speed) * MoveDistance, ok.transform.localPosition.y, ok.transform.localPosition.z);
            ok.transform.localPosition = mov;
        }
    }
    public float Barpressed()
    {
      playerAnimator=  transform.GetComponentInParent<Animator>();
        
        Pressed = true;
        float PressedPosition = ok.transform.localPosition.x;

        return PressedPosition;
 
    }



}
