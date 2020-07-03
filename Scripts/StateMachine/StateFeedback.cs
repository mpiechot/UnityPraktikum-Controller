using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateFeedback : MonoBehaviour,IState
{
    public Text text;
    
    public bool finished {get;set;}
    public IState next_state{get;set;}

    public void Enter()
    {
        Debug.Log("Entered State StateFeedback");

        //check, if experiment is finish
        if(false){
            finished = true;
        }
        else{
            // Set text color
            Color color = Color.black;
            // Set alpha value
            color.a = 1;
            // Set text which will be displayed
            text.text = "Du hast " + 10 + " Runden gemeistert. Es fehlen nur noch " + 10 + ". Drücke Leertaste um fortzufahren.";
            text.color = color;
        }
    }
 
    public void Execute()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            //finished = true;
        }
    }
 
    public void Exit()
    {
        //store Data of this round
        Experiment current_experiment = InformationManager.actual_experiment;
        List<Vector3> hand_positions = InformationManager.actual_positions;
    }

    

}
