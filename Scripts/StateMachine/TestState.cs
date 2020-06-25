using UnityEngine;

public class TestState : IState
{
    //Controller owner;

    public bool finished {get;set;}
 
    //public TestState(Controller owner) { this.owner = owner; }


 
    public void Enter()
    {
        Debug.Log("entering test state");

    }
 
    public void Execute()
    {
        Debug.Log("updating test state");

    }
 
    public void Exit()
    {
        Debug.Log("exiting test state");
    }


    
}