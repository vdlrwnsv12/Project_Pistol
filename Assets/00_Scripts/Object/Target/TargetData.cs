using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class TargetData
{
    public int ID;
    public string Type;
    public int Hp;
    public int Score;
}

[Serializable]
public class TargetDataList
{
    public TargetData[] Targets;
}
