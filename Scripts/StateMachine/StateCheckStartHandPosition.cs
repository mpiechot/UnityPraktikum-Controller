using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckStartHandPosition : MonoBehaviour,IState
{
    public Transform hand_transform;
    public Renderer hand_renderer;
    public Collider startarea_collider;
    public float duration;

    public bool finished {get;set;}
    public IState next_state{get;set;}

    private Coroutine coroutine;
    private bool coroutine_running = false;


    public void Enter()
    {

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
            if(coroutine_running){
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
    }
 
    public void Exit()
    {
        
    }

    public IEnumerator checkPositionOverTime(float hand_in_area_duration)
    {
        coroutine_running = true;
        yield return new WaitForSeconds(hand_in_area_duration);
        Debug.Log("Harald: Ready to enter new State");
        //finished = true;
    }
}
