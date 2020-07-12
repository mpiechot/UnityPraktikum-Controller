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

    public GameObject init;
    public GameObject edgePrefab;

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        state_renderer.material.color = Color.blue;

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
        state_renderer.material.color = Color.red;

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
                next_state = init.GetComponent<IState>();
                finished = true;
            }
        }
    }
 
    public void Exit()
    {

        state_renderer.material.color = Color.blue;
        //store Data of this round
        Experiment current_experiment = InformationManager.actual_experiment;
        List<Vector3> hand_positions = InformationManager.actual_positions;
        finished = false;
        //reset InformationManager?
        current_experiment.SuccessfulFinished = true;
    }


    public bool isExperimentOver(){
        return false;
    }

    

}
