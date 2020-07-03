﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateGoodbye : MonoBehaviour, IState {

    public bool finished { get; set; }
    public IState next_state { get; set; }

    public TextMeshProUGUI text;

    public void Enter() {
        Debug.Log("Enter: StateGoodbye");

        text.text = "Danke fürs Mitmachen.";
    }

    public void Execute() {
        if (Input.GetKeyDown("space")) {
            Debug.Log("Experiment finished");
        }
    }

    public void Exit() {
        Debug.Log("Exit: StateGoodbye");
    }

}
