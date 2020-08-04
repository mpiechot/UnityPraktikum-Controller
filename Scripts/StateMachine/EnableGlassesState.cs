using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGlassesState : MonoBehaviour,IState
{
    //Visualization
    private SpriteRenderer state_renderer;

    public GameObject edgePrefab;
    public Renderer glasses_renderer;

    //StateMachine
    public GameObject executionState;
    public bool finished { get; set; }
    public IState next_state { get; set; }

    void Awake()
    {
        //Visualize StateMachine
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(executionState.transform.position);
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
        EnableGlasses();
        next_state = executionState.GetComponent<IState>();
        finished = true;
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        finished = false;
        return;
    }

    //Change this Method to Send the enable command to an arduino for Example
    private void EnableGlasses()
    {
        Color color = Color.red;
        color.a = 0f;
        glasses_renderer.material.color = color;
        return;
    }
}
