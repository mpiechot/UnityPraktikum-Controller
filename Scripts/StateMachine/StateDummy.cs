using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDummy : MonoBehaviour,IState
{
    public GameObject edgePrefab;

    public bool finished { get; set; }
    public IState next_state { get; set; }

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        state_renderer.material.color = Color.blue;
    }
    public void Enter()
    {
        state_renderer.material.color = Color.red;
        return;
    }

    public void Execute()
    {
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        return;
    }
}
