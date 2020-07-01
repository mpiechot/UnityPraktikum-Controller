using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public GameObject currentStateGameObject;
    IState currentState;
    
    void Start(){
        if(currentStateGameObject != null){
            currentState = currentStateGameObject.GetComponent<IState>();
            currentState.Enter();
        }
    }

    void Update()
    {
        
        if (currentState != null) {
            currentState.Execute();
            if(currentState.finished){
                ChangeState();
            }
        }
    }


    public void ChangeState()
    {
        if(currentState != null){
            currentState.Exit();
            currentState = currentState.next_state;
            currentState.Enter();
        }
    }
    
}
