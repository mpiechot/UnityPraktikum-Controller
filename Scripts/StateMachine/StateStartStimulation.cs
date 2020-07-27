using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStartStimulation : MonoBehaviour, IState
{
    public GameObject nextStateName; //TODO insert fancy name here!
    public GameObject edgePrefab;

    public GameObject hand;
    public GameObject start;
    public GameObject target;

    public bool finished { get; set; }
    public IState next_state { get; set; }
    public float delay = 1f;
    public float stimulationTime = 2f;

    public SpeechRecognitionClient speech_receiver;

    private const float HALF_WAY_PERCENTAGE = 50f;
    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(nextStateName.transform.position);
        }
    }
    void AddEdge(Vector3 target)
    {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }
    public void Enter()
    {
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;
        return;
    }

    public void Execute()
    {
        Experiment current_experiment = InformationManager.actual_experiment;
        StartCoroutineOnExperimentSetting(current_experiment);
        if (nextStateName != null)
        {
            next_state = nextStateName.GetComponent<IState>();
            finished = true;
        }
        return;
    }

    private void StartCoroutineOnExperimentSetting(Experiment experiment)
    {
        if(experiment.stimulation_start == PossibleStimulationStart.INSTANT)
        {
            StartCoroutine(InstantStimulation(experiment.light_effect_side, experiment.finger_stimulation));
        }
        else
        {
            StartCoroutine(StimulationAfterHalfWay(experiment.light_effect_side, experiment.finger_stimulation));
        }
    }
    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        finished = false;
        return;
    }

    private IEnumerator InstantStimulation(PossibleLightEffectSide effect_side, PossibleFingerStimulations finger)
    {
        yield return new WaitForSeconds(delay);
        ChangeFingerLightState(effect_side,true);
        ChangeStimulationState(finger, true);
        speech_receiver.recognizedWord = "";

        InformationManager.timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log(InformationManager.timestamp);
        speech_receiver.record = true;

        yield return new WaitForSeconds(stimulationTime);
        ChangeStimulationState(finger, false);
        yield return new WaitForSeconds(1f);
        ChangeFingerLightState(effect_side, false);
    }

    private IEnumerator StimulationAfterHalfWay(PossibleLightEffectSide effect_side, PossibleFingerStimulations finger)
    {
        while (!HalfWayDone())
        {
            yield return 0;
        }
        ChangeFingerLightState(effect_side, true);
        ChangeStimulationState(finger, true);
        speech_receiver.recognizedWord = "";
        // TODO: Zeitstempel (nur erste Variable)

        InformationManager.timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log(InformationManager.timestamp);
        speech_receiver.record = true;


        yield return new WaitForSeconds(stimulationTime);
        ChangeStimulationState(finger, false);
        yield return new WaitForSeconds(1f);
        ChangeFingerLightState(effect_side, false);
    }

    private bool HalfWayDone()
    {
        float distance_done = Vector3.Distance(start.transform.position, hand.transform.position);
        float distance_total = Vector3.Distance(start.transform.position, target.transform.position);
        float way_percentage_done = (100 * distance_done) / distance_total;
        return  way_percentage_done > HALF_WAY_PERCENTAGE;
    }

    private void ChangeStimulationState(PossibleFingerStimulations finger, bool state)
    {
        Debug.Log("ChangeStimulationState needs to be implementet!");
        if (state)
        {
            hand.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            hand.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        return;
    }

    private void ChangeFingerLightState(PossibleLightEffectSide effect_side, bool state)
    {
        Debug.Log("ChangeFingerLightState needs to be implementet!");
        if (state)
        {
            start.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            start.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        return;
    }
}
