using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverLapChecker : MonoBehaviour
{
    public float maxDistance = 50F;
    public float mindistance = 10F;
    public Game_Play_Manager gm = new Game_Play_Manager();
    private Transform obj_pos;
    private void Start()
    {
        obj_pos = transform;
        gm = GameObject.FindObjectOfType<Game_Play_Manager>();
    }
    float overlapDistance;
    public float overlapAmount;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == this.tag)
        {
            overlapAmount = Vector3.Distance(collision.gameObject.transform.position, this.transform.position);
            overlapAmount = 100 - overlapAmount;
            overlapAmount= Mathf.Clamp(overlapAmount, 0, 100);
            gm.SetTotalAmounth();
        }
    }

    public void ResetFillAmounth()
    {
        overlapAmount = 0;
    }
    //void LateUpdate() // otherwise, visual lag can make for inconsistent collision checking.
    //{
    //    Collider2D[] cols = Physics2D.OverlapPointAll(
    //            Vector3.Scale(new Vector3(1, 1, 0), obj_pos.position), // background at z=0 
    //            LayerMask.GetMask("background")); // ignore non background layer objects
    //    Collider2D backgroundCol = null;
    //    for (int i = 0; i < cols.Length; i++) // order of increasing z value
    //    {
    //        if (cols[i].tag == "Background") // may be redundant with layer mask
    //        {
    //            backgroundCol = cols[i];
    //            break;
    //        }
    //    }
    //    if (backgroundCol != null)
    //    {
    //        overlapDistance = Vector3.Distance(this.transform.position, backgroundCol.gameObject.transform.position);

    //        //   float CalcDistance = Mathf.Clamp(overlapDistance, mindistance, maxDistance);
    //        float x = mindistance * 100 / overlapDistance;
    //        Debug.Log(x);

    //    }
    //    else
    //    {
    //        Debug.Log("Does not detect Background");
    //    }

    //}

}
