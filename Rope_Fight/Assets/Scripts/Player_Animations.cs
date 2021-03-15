using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    private const string Player_Hit_Trigger = "Player_Hit";
    private const string Player_Pull_Back = "Pull_Back";
    private const string Player_Push_Rope = "Push_Rope";
    public float DarbeKuvveti = 5F;
    private Animator animator;
    private Rigidbody rgd_body;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rgd_body = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pole")
        {
            animator.SetTrigger(Player_Hit_Trigger);
            rgd_body.isKinematic = false;
            rgd_body.constraints = RigidbodyConstraints.None;
            rgd_body.AddForce(new Vector3(1F, .5F, 0F), ForceMode.Impulse);
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
