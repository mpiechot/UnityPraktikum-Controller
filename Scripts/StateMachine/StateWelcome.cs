using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateWelcome : MonoBehaviour, IState {

    public bool finished { get; set; }
    public IState next_state { get; set; }

    public TextMeshProUGUI text;
    public GameObject state_check_object_position;

    public void Enter() {
        Debug.Log("Enter: StateWelcome");
        text.text = "Enter Welcome State";
    }

    public void Execute() {
        if (Input.GetKeyDown("space")) {
            text.text = "Du kannst die Leertaste drücken - super!";
        }
        if (Input.GetKeyDown("n")) {
            Debug.Log("Experiment about to start");
            next_state = state_check_object_position.GetComponent<IState>();
            finished = true;
        }
    }

    public void Exit() {
        Debug.Log("Exit: StateWelcome");
    }
}
