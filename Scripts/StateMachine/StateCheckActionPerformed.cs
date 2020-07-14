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
        // wenn nach 2s keine Reaktion erfolgt ist - merken (Zeitstempel)
        if (!has_responded) {
            // checken ob verbale reaktion
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

        Debug.Log("You said the word: " + speech_receiver.recognizedWord);
        obj_rotation = cylinder.transform.rotation.eulerAngles.z == 180 ? "upside down" : "richtig rum";
        
        next_state = state_goodbye.GetComponent<IState>();
        finished = true;
    }
}
