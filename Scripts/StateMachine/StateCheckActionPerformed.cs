using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateCheckActionPerformed : MonoBehaviour, IState {
    
    public bool finished { get; set; }
    public IState next_state { get; set; }

    public GameObject edgePrefab;

    public GameObject state_goodbye;
    public Collider target_position;
    public Transform cylinder;

    public MeshRenderer cylinder_renderer;

    public TextMeshProUGUI text;

    public int duration;
    private Coroutine coroutine;
    private bool coroutine_running = false;

    public SpeechRecognitionClient speech_receiver;
    private bool has_responded;
    private string response;

    private string obj_rotation;
    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        state_renderer.material.color = Color.blue;

        if (edgePrefab != null)
        {
            AddEdge(state_goodbye.transform.position);
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
        state_renderer.material.color = Color.red;
        Debug.Log("Enter: StateActionPerformed");
        text.text = "Bewege den Zylinder in die Zielposition";
        Color color = Color.black;
            // Set alpha value
        color.a = 1;

        text.color = color;

        has_responded = false;
    }

    public void Execute() {
        // Mithilfe eines Zeitstempels überprüfen, ob Reaktion innerhalb 2 s erfolgt ist. Wenn nicht -> notieren. 
        // if (!has_responded && response_time < 2s) -> response_time mit Zeitstempel berechnen
        /* damit kann man aktuelle Zeit herausfinden
            DateTime dt = DateTime.Now;
            Console.WriteLine(dt.ToString());
        */
        if (!has_responded) {
            // checken ob verbale reaktion
            if (speech_receiver.recognizedWord == "") {
                // user has not responded
                // trial is invalid
                response = "";
            } else {
                // user has responded
                // store the word (only the first response is important)
                response = speech_receiver.recognizedWord;
                has_responded = true;
            }
        }
        if (target_position.bounds.Contains(cylinder.position)) {
            if (!coroutine_running) {
                coroutine = StartCoroutine(CheckPositionOverTime(duration));
            }
            
        } else {
            if(coroutine_running){
                text.text = "Bewege den Zylinder in die Zielposition!";
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
        
    }

    public void Exit() {
        cylinder_renderer.material.color = Color.red;
        state_renderer.material.color = Color.blue;
        finished = false;
        coroutine_running = false;
        Debug.Log("Exit: StateActionPerformed");
    }

    public IEnumerator CheckPositionOverTime(int object_in_position) {
        coroutine_running = true;
        for(int i = object_in_position; i > 0; i--){
            text.text = "Super! Warten..." + i;
            yield return new WaitForSeconds(1);
        }
        text.text = "Fertig";

        Debug.Log("You said the word: " + response);
        obj_rotation = cylinder.transform.rotation.eulerAngles.z == 180 ? "upside down" : "normal";
        has_responded = false; // reset

        next_state = state_goodbye.GetComponent<IState>();
        finished = true;
    }
}
