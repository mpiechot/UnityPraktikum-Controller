using Array = System.Array;
using Enum = System.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : MonoBehaviour,IState
{
    public int numberOfRuns = 10;
    public GameObject checkTargetPositionState;

    public bool finished {get;set;}
    public IState next_state{get;set;}

    private Experiment[] experiments;
    private int currentExperiment = 0;

    // Start is called before the first frame update
    void Start()
    {
        Array stimulations = Enum.GetValues(typeof(PossibleFingerStimulations));
        Array light_effects = Enum.GetValues(typeof(PossibleLightEffectSide));
        Array stimulation_starts = Enum.GetValues(typeof(PossibleStimulationStart));
        Array object_colors = Enum.GetValues(typeof(PossibleObjectColor));

        experiments = new Experiment[stimulations.Length * light_effects.Length * stimulation_starts.Length * object_colors.Length * numberOfRuns];
        int i = 0;
        foreach (PossibleFingerStimulations stimulation in stimulations)
        {
            foreach (PossibleLightEffectSide light_effect in light_effects)
            {
                foreach (PossibleStimulationStart stimulation_start in stimulation_starts)
                {
                    foreach (PossibleObjectColor object_color in object_colors)
                    {
                        for(int run = 0; run < numberOfRuns; run++)
                        {
                            experiments[i++] = new Experiment(stimulation, light_effect, stimulation_start, object_color);
                        }
                    }
                }
            }
        }
        Shuffle();
        foreach(Experiment exp in experiments)
        {
            Debug.Log((i--) + ": " + exp);
        }
    }

    //TODO Maybe another shuffle algorihm? This one seems to be not that great :/
    private void Shuffle()
    {
        for (int i = 0; i < experiments.Length - 1; i++)
        {
            int rnd = Random.Range(i, experiments.Length);
            var tempGO = experiments[rnd];
            experiments[rnd] = experiments[i];
            experiments[i] = tempGO;
        }
    }

    public void Enter()
    {
        currentExperiment++;
        InformationManager.actual_experiment = experiments[currentExperiment];
        finished = true;
    }

    public void Execute()
    {
        return;
    }

    public void Exit()
    {
        return;
    }
}
