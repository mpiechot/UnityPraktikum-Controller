using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectColorState : MonoBehaviour,IState
{
    //Visualization
    private SpriteRenderer state_renderer;

    public GameObject edgePrefab;
    public MeshRenderer cylinder_renderer;

    //StateMachine
    public GameObject enableGlasses;
    public bool finished { get; set; }
    public IState next_state { get; set; }

    void Awake()
    {
        //Visualize StateMachine
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(enableGlasses.transform.position);
        }
    }
    //Draw an Edge using the LineRenderer from this state to its targetState
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
        return;
    }

    public void Execute()
    {
        Experiment current_experiment = InformationManager.actual_experiment;
        ChangeObjectColor(current_experiment.object_color);
        if(enableGlasses != null)
        {
            next_state = enableGlasses.GetComponent<IState>();           
            finished = true;
        }
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        finished = false;
        return;
    }

    //Change this Method to Send the object Color to an arduino for Example
    private void ChangeObjectColor(PossibleObjectColor object_color)
    {
        if(object_color == PossibleObjectColor.YELLOW){
            cylinder_renderer.material.color = Color.yellow;
        }
        else{
            cylinder_renderer.material.color = Color.green;
        }
        return;
    }
}
