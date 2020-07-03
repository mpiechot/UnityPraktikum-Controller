using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGlassesState : MonoBehaviour,IState
{
    public GameObject executionState;
    public GameObject edgePrefab;

    public bool finished { get; set; }
    public IState next_state { get; set; }

    private MeshRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<MeshRenderer>();
        state_renderer.material.color = Color.blue;

        if (edgePrefab != null && executionState != null)
        {
            GameObject newEdge = Instantiate(edgePrefab);
            Edge arrow = newEdge.GetComponent<Edge>();
            arrow.start = this.transform.position;
            arrow.target = executionState.transform.position;
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
        EnableGlasses();
        next_state = executionState.GetComponent<IState>();
        finished = true;
        return;
    }

    private void EnableGlasses()
    {
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        return;
    }
}
