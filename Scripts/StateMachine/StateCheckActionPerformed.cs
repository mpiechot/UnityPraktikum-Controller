using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateCheckActionPerformed : MonoBehaviour, IState {
    
    public bool finished { get; set; }
    public IState next_state { get; set; }

    public GameObject state_goodbye;
    public Collider target_position;
    public Transform cylinder;

    public TextMeshProUGUI text;

    public int duration;
    private Coroutine coroutine;
    private bool coroutine_running = false;

    public void Enter() {
        Debug.Log("Enter: StateActionPerformed");
        text.text = "Bewege Zylinder";
    }

    public void Execute() {
        // TODO: zusätzlich abfragen, bzgl verbaler Reaktion
        if (target_position.bounds.Contains(cylinder.position)) {
            Debug.Log("Object at target position");
            Debug.Log("TODO: Verbale Reaktion fehlt noch");
            if (!coroutine_running) {
                coroutine = StartCoroutine(CheckPositionOverTime(duration));
            }
            
        } else {
            if(coroutine_running){
                text.text = "Coroutine abgebrochen";
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
        
    }

    public void Exit() {
        Debug.Log("Exit: StateActionPerformed");
    }

    public IEnumerator CheckPositionOverTime(int object_in_position) {
        coroutine_running = true;
        for(int i = object_in_position; i > 0; i--){
            text.text = "Warten..." + i;
            yield return new WaitForSeconds(1);
        }
        text.text = "Fertig";
        next_state = state_goodbye.GetComponent<IState>();
        finished = true;
    }

}

// checken, ob Fußboden zuerst oder richtig rum
