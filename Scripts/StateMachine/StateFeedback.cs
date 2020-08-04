/*
Last state of one experiment. It adds the recorded data of the last run inside an external file
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
public class StateFeedback : MonoBehaviour,IState
{
    // State visualization
    public GameObject edgePrefab;
    private SpriteRenderer state_renderer;

    public TextMeshProUGUI text;
    
    // indicates whether the state is finished
    public bool finished {get;set;}

    // Gameobject of the next state
    public GameObject init;

    // IState of component of the next state's gameobject
    public IState next_state{get;set;}

    // State visualization
    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(init.transform.position);
        }
    }
    void AddEdge(Vector3 target)
    {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }


    public void Enter()
    {
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;

        Debug.Log("Entered State StateFeedback");

        
        // Set text which will be displayed
        text.text = "Sehr schön!. Drücke Leertaste für den nächsten Durchgang." + (StateMachine.trialPhase ? " Oder drücke X um die Übung zu beenden" : "");
        text.color = new Color(0,0,0,1);
        
    }

    public void Execute()
    {
        // sets finished to true as soon as the user presses the spacebar key
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            next_state = init.GetComponent<IState>();
            finished = true;
        }
        
    }

    // At the end of the state, store the collected data (as long as its not a trial round)
    public void Exit()
    {
        // State visualization
        state_renderer.material.color = Color.blue;

        //store Data of this round
        Experiment current_experiment = InformationManager.actual_experiment;
        List<Vector3> hand_positions = InformationManager.actual_positions;

        if(!StateMachine.trialPhase){
            File.AppendAllText(InformationManager.filename, InformationManager.actual_experiment.ToString() + Environment.NewLine);
        }

        // reset
        finished = false;
    }


    

    

}
