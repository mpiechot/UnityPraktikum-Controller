using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
public class StateFeedback : MonoBehaviour,IState
{
    public TextMeshProUGUI text;
    
    public bool finished {get;set;}
    public IState next_state{get;set;}

    public GameObject init;
    public GameObject edgePrefab;

    private SpriteRenderer state_renderer;

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

        
        // Set text color
        Color color = Color.black;
        // Set alpha value
        color.a = 1;
        // Set text which will be displayed
        text.text = "Sehr schön!. Drücke Leertaste für den nächsten Durchgang." + (StateMachine.trialPhase ? " Oder drücke X um die Übung zu beenden" : "");
        text.color = color;
        
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

        state_renderer.material.color = Color.blue;
        //store Data of this round
        Experiment current_experiment = InformationManager.actual_experiment;
        List<Vector3> hand_positions = InformationManager.actual_positions;
        finished = false;

        if(!StateMachine.trialPhase){
            File.AppendAllText(InformationManager.filename, InformationManager.actual_experiment.ToString() + Environment.NewLine);
        }
    }


    

    

}
