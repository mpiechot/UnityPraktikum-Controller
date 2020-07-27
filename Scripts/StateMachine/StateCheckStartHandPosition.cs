using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateCheckStartHandPosition : MonoBehaviour,IState
{
    public TextMeshProUGUI text;
    public GameObject state_disable_glasses;

    public Transform hand_transform;
    public Renderer hand_renderer;
    public Collider startarea_collider;
    
    public int duration;

    public bool finished {get;set;}
    public IState next_state{get;set;}

    public GameObject edgePrefab;

    private Coroutine coroutine;
    private bool coroutine_running = false;


    private float time_stamp;
    private SpriteRenderer state_renderer;

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
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;
        Debug.Log("Entered State CheckStartHandPosition");

        //Set text visible
        Color color = Color.black;
        color.a = 1;
        text.text = "";
        text.color = color;

        time_stamp = -1;
    }
 
    public void Execute()
    {
        if(startarea_collider.bounds.Contains(hand_transform.position)){
            hand_renderer.material.color = Color.green;

            // if(time_stamp == -1){
            //     time_stamp = Time.realtimeSinceStartup;
            // }
            // else{
            //     float delta_time = Time.realtimeSinceStartup - time_stamp;
            //     if(delta_time > duration){
                    
            //         finished = true;
            //     }
            // }

            if(!coroutine_running){
                coroutine = StartCoroutine(checkPositionOverTime(duration));
            }
        }
        else{
            hand_renderer.material.color = Color.red;
            text.text = "Bewege die Hand zurück in den Startbereich";
            if(coroutine_running){
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
    }
 
    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        Debug.Log("Exit State CheckStartHandPosition");
        coroutine_running = false;
        //Set text invisible
        Color color = Color.black;
        color.a = 0;
        text.text = "";
        text.color = color;
        finished = false;
    }

    public IEnumerator checkPositionOverTime(int hand_in_area_duration)
    {
        coroutine_running = true;
        for(int i = hand_in_area_duration; i > 0; i--){
            text.text = "Super! Halte deine Hand noch " + i + " Sekunden in dem Bereich";
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Ready to enter new State DisableGlasses");
        next_state = state_disable_glasses.GetComponent<IState>();
        finished = true;
    }
}
