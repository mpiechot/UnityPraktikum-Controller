using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDisableGlasses : MonoBehaviour,IState
{
    public Renderer glasses_renderer;
    public float delay;

    public GameObject debug_next_state0;

    public bool finished {get;set;}
    public IState next_state{get;set;}

    public GameObject edgePrefab;

    private bool coroutine_running = false;

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(debug_next_state0.transform.position);
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
        Debug.Log("Entered State DisableGlasses");
    }
 
    public void Execute()
    {
        if(!coroutine_running){
            //debug
            Color color = Color.red;
            color.a = 0.25f;
            glasses_renderer.material.color = color;

            //send message the same way we did in the exercise (function will be given from the other group)
            debugSendData(0);

            //wait for some seconds just to be sure that the message was received -> glasses disabled
            StartCoroutine(continueAfterSeconds(delay));
        }
    }
 
    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        coroutine_running = false;
        finished = false;
    }

    public IEnumerator continueAfterSeconds(float delay)
    {
        coroutine_running = true;
        yield return new WaitForSeconds(delay);
        Debug.Log("Ready to enter new State from StateDisable glasses");
        //comment the following in to process the next state
        next_state = debug_next_state0.GetComponent<IState>();
        finished = true;  
    }
    
    public void debugSendData(int state){
        //uses senderPort of the tcpip connection etc...
    }

}
