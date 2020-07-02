using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectColorState : MonoBehaviour,IState
{
    public GameObject enableGlasses;
    public GameObject edgePrefab;

    public bool finished { get; set; }
    public IState next_state { get; set; }

    private MeshRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<MeshRenderer>();
        state_renderer.material.color = Color.blue;

        if (edgePrefab != null && enableGlasses != null)
        {
            GameObject newEdge = Instantiate(edgePrefab);
            Edge arrow = newEdge.GetComponent<Edge>();
            arrow.start = this.transform.position;
            arrow.target = enableGlasses.transform.position;
        }
    }

    public void Enter()
    {
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

    private void ChangeObjectColor(PossibleObjectColor object_color)
    {
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        return;
    }
}
