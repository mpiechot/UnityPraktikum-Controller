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

    public SpeechRecognitionClient speech_receiver;

    private string obj_rotation;

    public void Enter() {
        Debug.Log("Enter: StateActionPerformed");
        text.text = "Bewege Zylinder";
    }

    public void Execute() {
        if (target_position.bounds.Contains(cylinder.position)) {
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

        Debug.Log("You said the word: " + speech_receiver.recognizedWord);
        obj_rotation = cylinder.transform.rotation.eulerAngles.z == 180 ? "upside down" : "richtig rum";
        
        next_state = state_goodbye.GetComponent<IState>();
        finished = true;
    }
}
