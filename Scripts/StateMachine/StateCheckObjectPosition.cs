using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateCheckObjectPosition : MonoBehaviour, IState {
    
    public bool finished { get; set; }
    public IState next_state { get; set; }

    public GameObject state_check_action_peformed;
    public Collider starting_position;
    public Transform cylinder;

    public TextMeshProUGUI text;

    public int duration;
    private Coroutine coroutine;
    private bool coroutine_running = false;

    public void Enter() {
        Debug.Log("Enter: StateCheckObjectPosition");
        text.text = "State Check Object Position";
    }

    public void Execute() {
        if (starting_position.bounds.Contains(cylinder.position)) {
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
        Debug.Log("Exit: StateCheckObjectPosition");
    }

    public IEnumerator CheckPositionOverTime(int object_in_position) {
        coroutine_running = true;
        for(int i = object_in_position; i > 0; i--){
            text.text = "Warten..." + i;
            yield return new WaitForSeconds(1);
        }
        text.text = "Fertig";
        next_state = state_check_action_peformed.GetComponent<IState>();
        finished = true;
    }

}
