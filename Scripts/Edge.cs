using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Vector3 start { get; set; }
    public Vector3 target { get; set; }

    // Update is called once per frame
    void Update()
    {
        if(start != null && target != null)
        {
            //start = Camera.main.ScreenToWorldPoint(start);
            //target = Camera.main.ScreenToWorldPoint(target);
            if (target != start)
            {
                var v3 = target - start;
                transform.position = start + (v3) / 2.0f;
                Vector3 ls = transform.localScale;
                transform.localScale = new Vector3(ls.x,v3.magnitude / 2.0f,ls.z);
                transform.rotation = Quaternion.FromToRotation(Vector3.up, v3);
            }
        }
    }
}