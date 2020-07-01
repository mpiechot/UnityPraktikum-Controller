using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    StateMachine stateMachine = new StateMachine();
   
    //declare all states
    public GameObject state0;

    public GameObject state1;


    private IState Istate0;
    

    void Start()
    {
        //instantiate states and set connections (start with the last state, so that it can be passed to the previous one as argument. Afterwards, close the ring)
        //state1 = new TestState0(null);
        //state0 = new TestState0(state1);
        //state1.next_state = state0;

        Istate0 = state0.GetComponent<IState>();

        //set first state
        stateMachine.SetState(Istate0);
    }
 
    void Update()
    {
        stateMachine.Update();
        if(stateMachine.isStateFinished()){
            stateMachine.ChangeState();
        }
    }

}
