using UnityEngine;
using System.Collections;
//using UnityEngine.

public class TestState1 : MonoBehaviour,IState
{
    
    public GameObject state0;
    public GameObject state1;

    public bool finished {get;set;}
    public IState next_state{get;set;}


    public void Enter()
    {
        
    }
 
    public void Execute()
    {
        test1();
        
    }
 
    public void Exit()
    {
        Debug.Log("exiting test state0");
    }


    public void test1(){
        //Debug.Log("test1");
    }

    
}