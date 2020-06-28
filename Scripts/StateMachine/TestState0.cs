using UnityEngine;

public class TestState0 : IState
{
    
    public bool finished {get;set;}
    public IState next_state{get;set;}

    public TestState0(IState ns){
        next_state = ns;
    }

    public void Enter()
    {
        Debug.Log("entering test state");
    }
 
    public void Execute()
    {
        test0();
    }
 
    public void Exit()
    {
        Debug.Log("exiting test state");
    }


    public void test0(){
        Debug.Log("test0");
    }
    
}