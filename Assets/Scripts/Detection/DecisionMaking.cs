using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class DecisionMaking
{
    [SerializeField] private bool _avoidCollision;
    public event EventHandler Collision;

    private ObjectDetection _detection;

    public DecisionMaking(ObjectDetection detection)
    {
        _detection = detection;
    }
    public void Decision(DetectionType detectionType)
    {
        switch (detectionType)
        {
            case DetectionType.human:
                RaiseCollision();
                break;
        }
    }
    public void RaiseCollision()
    {
        var h = Collision;
        h?.Invoke(this, EventArgs.Empty);
        Debug.Log("Raising Event");
    }
    public ObjectDetection Detection
    {
        get => _detection;
    }
    public bool AvoidCollision
    {
        get => _avoidCollision;
        set => _avoidCollision = value;
    }

}
