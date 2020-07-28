using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
public class StateTransitionWT : MonoBehaviour,IState
{
    public TextMeshProUGUI text;
    
    public bool finished {get;set;}
    public IState next_state{get;set;}

    public GameObject init;


    public void Enter()
    {
        Debug.Log("Entered State StateTransition");

        
        // Set text which will be displayed
        text.text = "Drücke Leertaste um die Übungsphase zu beginnen";
        text.color = new Color(0,0,0,1);
        
    }
 
    public void Execute()
    {
        
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            next_state = init.GetComponent<IState>();
            finished = true;
        }
        
    }
 
    public void Exit()
    {
        finished = false;
        text.text = "";
    }


    

    

}
