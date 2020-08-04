/*
This state turns the glasses black
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDisableGlasses : MonoBehaviour,IState
{
    // State visualization
    public GameObject edgePrefab;
    private SpriteRenderer state_renderer;

    // Renderer of the glasses
    public Renderer glasses_renderer;

    // delay just to make sure that it enters the next state after the glasses were turned black
    public float delay;

    // indicates whether the state is finished
    public bool finished {get;set;}

    // gameobject of the next state
    public GameObject next_state_object;

    // IState component of the next state's gameobject
    public IState next_state{get;set;}

    private bool coroutine_running = false;

    
    // Visualization
    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(next_state_object.transform.position);
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

    // Sends a message to the glasses
    public void Execute()
    {
        if(!coroutine_running){
            //debug - visualize the glasses beeing turned off
            glasses_renderer.material.color = new Color(255,0,0,0.25f);

            //send message the same way we did in the exercise (function will be given from the other group)
            debugSendData(0);

            //wait for some seconds just to be sure that the message was received -> glasses black
            StartCoroutine(continueAfterSeconds(delay));
        }
    }

    public void Exit()
    {
        // State visualization
        state_renderer.material.color = Color.blue;

        // Reset
        coroutine_running = false;
        finished = false;
    }

    // coroutine which sets finished to true after a short delay
    public IEnumerator continueAfterSeconds(float delay)
    {
        coroutine_running = true;
        yield return new WaitForSeconds(delay);
        Debug.Log("Ready to enter new State from StateDisable glasses");
        //comment the following in to process the next state
        next_state = next_state_object.GetComponent<IState>();
        finished = true;  
    }
    

    // Method which will be replaced with a method from the other group
    public void debugSendData(int state){
        //uses senderPort of the tcpip connection etc...
    }

}
