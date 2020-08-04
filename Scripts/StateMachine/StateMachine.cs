using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;

    public GameObject currentStateGameObject;
    public GameObject stateAfterTrial;

    //Settings
    public static bool trialPhase = true;
    public static int numberOfRuns = 10;
    
    void Start(){
        if(currentStateGameObject != null){
            currentState = currentStateGameObject.GetComponent<IState>();
            currentState.Enter();
        }
    }

    void Update()
    {
        //Statemachine Logic
        if (currentState != null) {
            currentState.Execute();
            if(currentState.finished){
                ChangeState();
            }
        }

        //TrialPhase Logic ( Change to Experiment-Phase, if X is pressed
        if(trialPhase){
            if(Input.GetKeyDown(KeyCode.X)){
                trialPhase = false;
                currentState.Exit();

                //State Visualization (Hide Trial-Statemachine, Show Experiment-Statemachine)
                GameObject[] visuals = GameObject.FindGameObjectsWithTag("NonTrial"); 
                for(int i = 0; i < visuals.Length; i++){
                    visuals[i].GetComponent<SpriteRenderer>().color = new Color(0,0,255,1);
                }
                visuals = GameObject.FindGameObjectsWithTag("Trial"); 
                for(int i = 0; i < visuals.Length; i++){
                    visuals[i].GetComponent<SpriteRenderer>().color = new Color(0,0,255,0);
                }

                currentState = stateAfterTrial.GetComponent<IState>();
                currentState.Enter();
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
