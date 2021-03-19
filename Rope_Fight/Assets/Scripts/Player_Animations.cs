using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    private const string Player_Hit_Trigger = "Player_Hit";
    private const string Player_Pull_Back = "Pull_Back";
    private const string Player_Push_Rope = "Push_Rope";
    public float DarbeKuvveti = 3F;
    private Animator animator;
    private Rigidbody rgd_body;
    private PlayerCurrentStatus status;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rgd_body = GetComponent<Rigidbody>();
        status = GetComponent<PlayerCurrentStatus>();
        if (animator.transform.tag == "Player")
        {
            //animator.transform.Rotate(0, -90F, 0);
        }
        else
        {
          //  animator.transform.Rotate(0, -45F, 0);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pole")
        {
            animator.SetTrigger(Player_Hit_Trigger);
            rgd_body.isKinematic = false;
            rgd_body.constraints = RigidbodyConstraints.None;
            rgd_body.AddForce(new Vector3(DarbeKuvveti, 1.7F, 0F), ForceMode.Impulse);
            status.PlayerFallen = true;
        }
    }

    public void PlayPlayer_Push_Anim()
    {
        animator.transform.Rotate(0, 0F, 0);
        animator.SetTrigger(Player_Push_Rope);
    }
    public void PlayPlayer_Pull_Back_Anim()
    {
        animator.transform.Rotate(0, 0F, 0);
        animator.SetTrigger(Player_Pull_Back); ;
    }
}
