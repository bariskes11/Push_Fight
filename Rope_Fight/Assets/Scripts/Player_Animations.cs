using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    private const string Player_Hit_Trigger = "Player_Hit";
    private const string Player_Pull_Back = "Pull_Back";
    private const string Player_Push_Rope = "Push_Rope";
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pole")
        {
            animator.SetTrigger(Player_Hit_Trigger);
        }
    }

    public void PlayPlayer_Push_Anim()
    {
        animator.SetTrigger(Player_Push_Rope);
    }
    public void PlayPlayer_Pull_Back_Anim()
    {
        animator.SetTrigger(Player_Pull_Back); ;
    }
}
