using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateFeedback : MonoBehaviour,IState
{
    public TextMeshProUGUI text;
    
    public bool finished {get;set;}
    public IState next_state{get;set;}

    public void Enter()
    {
        Debug.Log("Entered State StateFeedback");

        //check, if experiment is finish
        if(isExperimentOver()){
            //next_state = ...;
            finished = true;
        }
        else{
            // Set text color
            Color color = Color.black;
            // Set alpha value
            color.a = 1;
            // Set text which will be displayed
            text.text = "Das hast du toll gemacht. Drücke Leertaste für den nächsten Durchgang.";
            text.color = color;
        }
    }
 
    public void Execute()
    {
        if(!isExperimentOver()){
            if (Input.GetKeyDown("space"))
            {
                print("space key was pressed");
                //next_state = ...;
                //finished = true;
            }
        }
    }
 
    public void Exit()
    {
        //store Data of this round
        Experiment current_experiment = InformationManager.actual_experiment;
        List<Vector3> hand_positions = InformationManager.actual_positions;

        //reset InformationManager?
    }


    public bool isExperimentOver(){
        return false;
    }

    

}
