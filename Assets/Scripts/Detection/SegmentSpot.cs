using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SegmentSpot
{
    [field: SerializeField] public Vector3 Position {get;set;}
    [field: SerializeField] public bool HasSpotBeenClaimed{ get; set; }

}
