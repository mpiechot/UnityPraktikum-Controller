using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;

public static class InformationManager 
{

    public static Stopwatch sw = new Stopwatch();

    public static string filename;

    public static Experiment actual_experiment;
    public static int info1 = 0;
    
    public static List<Vector3> actual_positions;
}
