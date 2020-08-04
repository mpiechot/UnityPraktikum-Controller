using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class StateStartStimulation : MonoBehaviour, IState
{
    //Visualization
    private SpriteRenderer state_renderer;

    public GameObject edgePrefab;

    //StateMachine
    public GameObject actionPerformedState;
    public bool finished { get; set; }
    public IState next_state { get; set; }

    //State Reference Objects
    public GameObject hand;
    public GameObject leftLightEffectSide;
    public GameObject rightLightEffectSide;
    public GameObject startArea;
    public GameObject targetArea;
    public SpeechRecognitionClient speech_receiver;

    //State Settings
    public float instantStimulationDelay = 1f;
    public float stimulationTime = 2f;

    private const float HALF_WAY_PERCENTAGE = 50f;

    void Awake()
    {
        //Visualize StateMachine
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(actionPerformedState.transform.position);
        }
    }
    //Draw an Edge using the LineRenderer from this state to its targetState
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

        if (actionPerformedState != null)
        {
            next_state = actionPerformedState.GetComponent<IState>();
            finished = true;
        }
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        finished = false;
        return;
    }

    //Helperfunctions
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

    private IEnumerator InstantStimulation(PossibleLightEffectSide effect_side, PossibleFingerStimulations finger)
    {
        yield return new WaitForSeconds(instantStimulationDelay);
        ChangeFingerLightState(effect_side,true);
        ChangeStimulationState(finger, true);
        speech_receiver.recognizedWord = "";

        InformationManager.sw = new Stopwatch();
        InformationManager.sw.Start();
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

        InformationManager.sw = new Stopwatch();
        InformationManager.sw.Start();
        speech_receiver.record = true;

        yield return new WaitForSeconds(stimulationTime);
        ChangeStimulationState(finger, false);
        yield return new WaitForSeconds(1f);
        ChangeFingerLightState(effect_side, false);
    }

    private bool HalfWayDone()
    {
        float distance_done = Vector3.Distance(startArea.transform.position, hand.transform.position);
        float distance_total = Vector3.Distance(startArea.transform.position, targetArea.transform.position);
        float way_percentage_done = (100 * distance_done) / distance_total;
        return  way_percentage_done > HALF_WAY_PERCENTAGE;
    }


    //Visualize Changes in Unity
    //Change these Methods to send the data to an arduino for example
    private void ChangeStimulationState(PossibleFingerStimulations finger, bool state)
    {
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
        if (state)
        {
            if(effect_side == PossibleLightEffectSide.LEFT)
            {
                leftLightEffectSide.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                rightLightEffectSide.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        else
        {
            if(effect_side == PossibleLightEffectSide.LEFT)
            {
                leftLightEffectSide.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
            else
            {
                rightLightEffectSide.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
        return;
    }
}
