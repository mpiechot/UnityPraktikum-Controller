using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckStartHandPosition : MonoBehaviour,IState
{
    public GameObject state_disable_glasses;

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
        Debug.Log("Entered State CheckStartHandPosition");
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
        Debug.Log("Exit State CheckStartHandPosition");
    }

    public IEnumerator checkPositionOverTime(float hand_in_area_duration)
    {
        coroutine_running = true;
        yield return new WaitForSeconds(hand_in_area_duration);
        Debug.Log("Ready to enter new State DisableGlasses");
        next_state = state_disable_glasses.GetComponent<IState>();
        finished = true;
    }
}
