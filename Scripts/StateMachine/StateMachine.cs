using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public GameObject currentStateGameObject;
    IState currentState;
    
    void Start(){
        currentState = currentStateGameObject.GetComponent<IState>();
    }

    void Update()
    {
        if (currentState != null) currentState.Execute();
        if(currentState.finished){
            ChangeState();
        }
    }


    public void ChangeState()
    {
        currentState.Exit();
        currentState = currentState.next_state;
        currentState.Enter();
    }
    
}
