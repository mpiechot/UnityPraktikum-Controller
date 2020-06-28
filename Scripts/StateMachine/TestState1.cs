using UnityEngine;

public class TestState1 : IState
{
    
    public bool finished {get;set;}
    public IState next_state{get;set;}
 
    public TestState1(IState ns){
        next_state = ns;
    }

    public void Enter()
    {
        Debug.Log("entering test state");
    }
 
    public void Execute()
    {
        test1();
    }
 
    public void Exit()
    {
        Debug.Log("exiting test state");
    }


    public void test1(){
        Debug.Log("test1");
    }
    
}