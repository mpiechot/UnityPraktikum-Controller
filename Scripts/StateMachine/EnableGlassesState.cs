using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGlassesState : MonoBehaviour,IState
{
    public GameObject executionState;
    public GameObject edgePrefab;
    public Renderer glasses_renderer;

    public bool finished { get; set; }
    public IState next_state { get; set; }

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(executionState.transform.position);
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
        return;
    }

    public void Execute()
    {
        EnableGlasses();
        next_state = executionState.GetComponent<IState>();
        finished = true;
        return;
    }

    private void EnableGlasses()
    {
        Color color = Color.red;
        color.a = 0f;
        glasses_renderer.material.color = color;
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        finished = false;
        return;
    }
}
