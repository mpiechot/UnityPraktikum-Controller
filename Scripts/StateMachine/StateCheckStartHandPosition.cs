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

    private Coroutine coroutine;
    private bool coroutine_running = false;


    public void Enter()
    {
        Debug.Log("Entered State CheckStartHandPosition");

        //Set text visible
        Color color = Color.black;
        color.a = 1;
        text.text = "";
        text.color = color;
    }
 
    public void Execute()
    {
        if(startarea_collider.bounds.Contains(hand_transform.position)){
            hand_renderer.material.color = Color.green;
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
        Debug.Log("Exit State CheckStartHandPosition");

        //Set text invisible
        Color color = Color.black;
        color.a = 0;
        text.text = "";
        text.color = color;
    }

    public IEnumerator checkPositionOverTime(int hand_in_area_duration)
    {
        coroutine_running = true;
        for(int i = hand_in_area_duration; i > 0; i--){
            text.text = "Sehr gut! Es geht los in " + i + " Sekunden";
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Ready to enter new State DisableGlasses");
        next_state = state_disable_glasses.GetComponent<IState>();
        finished = true;
    }
}
