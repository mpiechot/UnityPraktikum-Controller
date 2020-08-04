
/*
To perform one experiment, the hand has to be inside a predefined start area.
Thus, this state requests the user to move his hand inside the start area (and to keep it there for some seconds)
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateCheckStartHandPosition : MonoBehaviour,IState
{
    public TextMeshProUGUI text;

    // for state visualization
    private SpriteRenderer state_renderer;
    
    // for state visualization
    public GameObject edgePrefab;

    // transform of the hand object to retrieve the position
    public Transform hand_transform;

    // renderer of the hand object to set the color
    public Renderer hand_renderer;

    // collider of the start area to check whether the hand is inside
    public Collider startarea_collider;
    
    // duration in seconds for how long the hand has to remain inside the start area
    public int duration;

    // bool which indicates whether the state is finished
    public bool finished {get;set;}

    // reference to the next state's gameobject
    public GameObject state_disable_glasses;

    // IState component of the next state's gameobject
    public IState next_state{get;set;}

    private Coroutine coroutine;

    // indicates whether the coroutine is running or not
    private bool coroutine_running = false;

    // activtes state visualization
    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(state_disable_glasses.transform.position);
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
        // state visualization
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;
        Debug.Log("Entered State CheckStartHandPosition");

        //Set text visible
        text.text = "";
        text.color = new Color(0,0,0,1);
    }
 
    // checks, whether the hand is currently inside the start area - shows an appropriate text
    public void Execute()
    {
        // checks whether the hand is inside the start area
        if(startarea_collider.bounds.Contains(hand_transform.position)){
            // if so, set the hand color to green (just to highlight that the hand is inside)
            hand_renderer.material.color = Color.green;

            // if the hand just entered the start area, start a coroutine which acts as a timer
            if(!coroutine_running){
                coroutine = StartCoroutine(checkPositionOverTime(duration));
            }
        }
        // if the hand is outside the start area, simply request the user to move his hand
        else{
            // set hand color to red (just to highlight that his hand is still not inside the start area)
            hand_renderer.material.color = Color.red;
            text.text = "Bewege die Hand zurück in den Startbereich";
            // if the hand was inside the start area (but not for long enough), stop the running coroutine
            if(coroutine_running){
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
    }
 
    public void Exit()
    {
        // state visualization
        state_renderer.material.color = Color.blue;

        Debug.Log("Exit State CheckStartHandPosition");

        // Reset
        text.text = "";
        text.color = new Color(0,0,0,0);
        coroutine_running = false;
        finished = false;
    }

    // coroutine which acts like a counter
    public IEnumerator checkPositionOverTime(int hand_in_area_duration)
    {
        coroutine_running = true;
        for(int i = hand_in_area_duration; i > 0; i--){
            text.text = "Super! Halte deine Hand noch " + i + " Sekunden in dem Bereich";
            yield return new WaitForSeconds(1);
        }
        // when the hand was "duration" seconds inside the start area, indicate that the current state is finished
        Debug.Log("Ready to enter new State DisableGlasses");
        next_state = state_disable_glasses.GetComponent<IState>();
        finished = true;
    }
}
