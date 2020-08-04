using System.Collections.Generic;
using UnityEngine;

public class Experiment
{
    public PossibleFingerStimulations finger_stimulation { get; }
    public PossibleLightEffectSide light_effect_side { get; }
    public PossibleStimulationStart stimulation_start { get; }
    public PossibleObjectColor object_color { get; }
    public bool SuccessfulFinished { get; set; }
    public string ErrorCode { get; set; }

    public List<Vector3> HandPositions {get;set;}

    public double ReactionTime {get; set;}

    public string SpokenWord {get; set;}

    public Experiment(PossibleFingerStimulations finger, PossibleLightEffectSide effect_side, PossibleStimulationStart start, PossibleObjectColor color)
    {
        this.finger_stimulation = finger;
        this.light_effect_side = effect_side;
        this.stimulation_start = start;
        this.object_color = color;
        this.SuccessfulFinished = false;
        this.ReactionTime = -1;
        this.SpokenWord = "-";
        this.HandPositions = new List<Vector3>();
        this.ErrorCode = "-";
    }

    public override string ToString()
    {
        string experiment = finger_stimulation + ";" + light_effect_side + ";" + stimulation_start + ";" + object_color + ";" + SuccessfulFinished + ";" + ErrorCode + ";" + ReactionTime + ";" + SpokenWord;
        experiment += ";[";
        for(int i = 0; i < HandPositions.Count; i++){
            experiment += "(" + HandPositions[i].x + "," + HandPositions[i].y + "," + HandPositions[i].z + ")";
        }
        experiment += "]";

        return experiment;
        //return "Experiment["+finger_stimulation + ", " + light_effect_side + ", " + stimulation_start + ", " + object_color + ", " + SuccessfulFinished + ", " + ReactionTime + ", " + SpokenWord + "]";
    }

}