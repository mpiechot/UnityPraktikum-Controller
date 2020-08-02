using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StateCheckActionPerformed : MonoBehaviour, IState {
    
    public bool finished { get; set; } // true if current state is finished and ready to call its Exit() method
    public IState next_state { get; set; } // follow-up state -> current_state transitions to next_state in Exit() method

    public GameObject next_state_go; // contains follow-up state

    public Collider target_position; // cylinder will be placed in this area
    public Transform cylinder;
    public Transform hand;

    public MeshRenderer cylinder_renderer; // visual stimulation 

    public TextMeshProUGUI text; // instructions for user

    public float epsilon = 10; // valid range (+/- epsilon) for rotation of cylinder to reduce the noise of the measurement device

    public int duration; // necessary time (in seconds) the cylinder must remain at the target position 
    // coroutine is handling the countdown 
    private Coroutine coroutine;
    private bool coroutine_running = false;

    // speech recognition:
    public SpeechRecognitionClient speech_receiver; // handle speech recognition
    private bool has_responded; // true if user has responded
    private string response; // user response regarding the verbal stimulation
    public bool wordRecordedInTime = false; // true if reaction time was valid

    private PossibleObjectColor obj_rotation; // rotation of object is depending on color of verbal stimulation

    // for the visual representation of the states:
    public GameObject edgePrefab;
    private SpriteRenderer state_renderer;

    void Awake() {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        if (edgePrefab != null) {
            AddEdge(next_state_go.transform.position);
        }
    }

    void AddEdge(Vector3 target)
    {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }

    public void Enter() {
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;

        Debug.Log("Enter: StateActionPerformed");
        
        // instructions:
        text.text = "Bewege den Zylinder in die Zielposition";
        Color color = Color.black;
        color.a = 1; // Set alpha value
        text.color = color;

        has_responded = false; // prevent recording earlier responses
    }

    public void Execute() {
        InformationManager.actual_experiment.HandPositions.Add(hand.position);

        // only the first response will be saved - later responses are discarded
        if (!has_responded) {
            if (speech_receiver.recognizedWord == "") { // no response recorded yet
                response = "";
            } else { // user has responded
                response = speech_receiver.recognizedWord; // store word
                Debug.Log(response);
                has_responded = true;

                //InformationManager.timestamp = DateTime.Now;
                // stop stopwatch and calculate the user's reaction time in milliseconds
                InformationManager.sw.Stop();
                double passed_time = InformationManager.sw.ElapsedMilliseconds;
                InformationManager.actual_experiment.ReactionTime = passed_time;
                InformationManager.actual_experiment.SpokenWord = response;
                Debug.Log("passed time : " + passed_time);

                // if user responded within 3ms the reaction time is valid
                if(passed_time <= 3000){
                    wordRecordedInTime = true;
                    //InformationManager.actual_experiment.SuccessfulFinished = true;
                }

                speech_receiver.record = false; // no more responses are parsed into strings (see SpeechRecognitionClient)
            }
        }

        // if object is at target position and the coroutine has not yet been started, start coroutine
        if (target_position.bounds.Contains(cylinder.position)) { // 
            if (!coroutine_running) {
                coroutine = StartCoroutine(CheckPositionOverTime(duration));
            }
        // if object is not at target position and the coroutine is already running, stop coroutine and give user new instructions    
        } else {
            if(coroutine_running){
                text.text = "Bewege den Zylinder in die Zielposition!";
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
        
    }

    public void Exit() {
        cylinder_renderer.material.color = Color.red; // reset cylinder color
        state_renderer.material.color = Color.blue;
        finished = false;
        coroutine_running = false;
        Debug.Log("Exit: StateActionPerformed");
    }

    // Handling the countdown (duration):
    // When the countdown is over, prepare the transitition to the exit method
    public IEnumerator CheckPositionOverTime(int object_in_position) {
        coroutine_running = true;
        for(int i = object_in_position; i > 0; i--){
            text.text = "Super! Warten..." + i; // inform user about countdown
            yield return new WaitForSeconds(1);
        }
        text.text = "Fertig";

        Debug.Log("You said the word: " + response);
        
        // calculate rotation of cylinder
        obj_rotation = Mathf.Abs(180 - cylinder.transform.rotation.eulerAngles.z) <= epsilon ? PossibleObjectColor.YELLOW : PossibleObjectColor.GREEN;
        
        // parsing response into enum type
        PossibleFingerStimulations pfs = (response == "daumen") ? PossibleFingerStimulations.THUMB : PossibleFingerStimulations.INDEX;        
        
        // trial valid if reaction time valid, object rotation and response correct
        if(wordRecordedInTime && obj_rotation == InformationManager.actual_experiment.object_color && pfs == InformationManager.actual_experiment.finger_stimulation){
            InformationManager.actual_experiment.SuccessfulFinished = true;
        }
        
        // reset:
        has_responded = false;
        wordRecordedInTime = false;
        response = "";
        speech_receiver.Reset();

        next_state = next_state_go.GetComponent<IState>();
        finished = true;
    }

}
