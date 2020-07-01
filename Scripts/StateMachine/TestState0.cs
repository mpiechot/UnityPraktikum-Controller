using UnityEngine;
using System.Collections;
//using UnityEngine.

public class TestState0 : MonoBehaviour,IState
{
    
    public GameObject state_prepare;
    public GameObject state_main;

    public bool finished {get;set;}
    public IState next_state{get;set;}


    public void Enter()
    {
        Debug.Log("entering test state0");
        if(!coroutine_started){
            StartCoroutine(this.coroutine());   
        }
    }
 
    private bool coroutine_finished = false;
    private bool coroutine_started = false;
    public void Execute()
    {
        test0();
        if(coroutine_finished){
            finished = true;
        }

        
    }
 
    public void Exit()
    {
        Debug.Log("exiting test state0");
    }


    public void test0(){
        Debug.Log("test0");
    }

    public IEnumerator coroutine(){
        coroutine_started = true;
        yield return new WaitForSeconds(1.0f);
        Debug.Log("finished coroutine");
        coroutine_finished = true;
        yield return null;
    }
    
}