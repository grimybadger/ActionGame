using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectDetection))]
public class ObjectDetectionEditor : Editor
{
    ObjectDetection d;
    private bool _hasDetected;
    Collider _col;

    private bool _mCollider;

    private void OnEnable()
    {
        d = target as ObjectDetection;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (d.GetComponent<Collider>())
        {
            _col = d.GetComponent<Collider>();
            d.Collider = _col;
            _mCollider = true;
        }
        else
        {
            EditorGUILayout.HelpBox("Missing Collider", MessageType.Warning);
            _mCollider = false;
            return;
        }

        var detectedObjects = serializedObject.FindProperty("_detectedItems");
        var makeDecision = serializedObject.FindProperty("_decisionMaking");
        var detectionType = serializedObject.FindProperty("_detectionType");
        var maxDistance = serializedObject.FindProperty("_maxDistance");
        var centerOffset = serializedObject.FindProperty("_centerOffset");
        var heightOffset = serializedObject.FindProperty("_heightOffset");
        var angle = serializedObject.FindProperty("_angle");
        var isVisualized = serializedObject.FindProperty("_isVisualized");
        var isWithinArc = serializedObject.FindProperty("_isWithinArc");
        var rotationSpeed = serializedObject.FindProperty("_rotationSpeed");
        var segmentNumbers = serializedObject.FindProperty("_segmentNumbers");
        var isDetecting = serializedObject.FindProperty("_isDetecting");
        var useRadar = serializedObject.FindProperty("_useRadar");
        var mdetectionDistance = serializedObject.FindProperty("_mDistanceDetection");
        var segmentPositions = serializedObject.FindProperty("_segmentPositions");


        // EditorGUILayout.PropertyField(makeDecision);

        EditorGUILayout.PropertyField(detectedObjects);

        EditorGUILayout.PropertyField(detectionType);
        EditorGUILayout.PropertyField(maxDistance);
        EditorGUILayout.PropertyField(centerOffset);
        EditorGUILayout.PropertyField(heightOffset);
        EditorGUILayout.PropertyField(angle);
        EditorGUILayout.PropertyField(isVisualized);
        EditorGUILayout.PropertyField(isWithinArc);
        EditorGUILayout.PropertyField(rotationSpeed);
        EditorGUILayout.PropertyField(segmentNumbers);
        EditorGUILayout.PropertyField(isDetecting);
        EditorGUILayout.PropertyField(useRadar);
        EditorGUILayout.PropertyField(mdetectionDistance);
        EditorGUILayout.PropertyField(segmentPositions);

        if (GUILayout.Button("Toggle Detection", GUILayout.Width(250)))
        {
            d.ToggleDetection();
        }
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// TODO Have the arc direction point in the appropiate direction according to gameObject rotation
    /// </summary>
    /// 
    private void OnSceneGUI()
    {
        if (!_mCollider)
        {
            return;
        }

        // DivideIntoSegments(d.Angle);
        Vector3 forwardPointMinusHalfAngle = Quaternion.Euler(0, -d.Angle / 2, 0) * d.transform.forward;
        Vector3 arcStart = forwardPointMinusHalfAngle * d.MaxDistance;
        //Vector3 arcCenter = new Vector3(d.transform.position.x, d.transform.position.y + d.HeightOffset, d.transform.position.z + d.CenterOffset);
        Vector3 arcCenter = new Vector3(_col.bounds.center.x, d.transform.position.y + d.HeightOffset, _col.bounds.center.z + d.CenterOffset);
        Vector3 arcDirecton = d.ArcDirection();

        Handles.color = new Color(1, 1, 1, 0.1f);

        if (d.DetectedObject) Handles.color = new Color(1, 0.92f, 0.016f, 0.08f);
        Handles.DrawSolidArc(
            arcCenter, // Center of the arc 
            Vector3.up, // Up direction of the arc
            arcStart, // Point where it begins
            d.Angle, // Angle of the arc
            d.MaxDistance // Radius of the arc 

        );

    }
    private float GetSineVectorAngle(float angle) => Mathf.Sin(angle * Mathf.Deg2Rad);
    private float GetCosineVectorAngle(float angle) => Mathf.Cos(angle * Mathf.Deg2Rad);

    private float GetVectorAngle(bool isCos = false, float angle = 0)
    {
        return angle = isCos ? Mathf.Cos(angle) : Mathf.Sin(angle);
    }

    private void DrawRay(float maxDistance, float angle)
    {
        //angle = 90 + angle;
        //Vector3 direction = new Vector3(GetVectorAngle(true,angle), 0, GetVectorAngle(false, angle));
        //Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        //float ang = Mathf.Atan2(direction.x, direction.z);
        //Vector3 newDirection = new Vector3(0, 0, ang);

        Vector3 leftHandle = Quaternion.Euler(0, angle, 0) * d.transform.forward;
        Vector3 rightHandle = Quaternion.Euler(0, -angle, 0) * d.transform.forward;
        Vector3 centerLine = Quaternion.Euler(0, angle / 5f, 0) * d.transform.forward;
        Vector3 otherCenterLine = Quaternion.Euler(0, -angle / 5f, 0) * d.transform.forward;

        //Vector3 cornerOne = Quaternion.Euler(0, 190, 0) * d.transform.forward;
        // Vector3 newDirection = direction * d.transform.forward;
        //Vector3 newDirection = d.transform.TransformDirection(direction) * 10;

        // Debug.DrawRay(d.transform.position, cornerOne * maxDistance, Color.green);
        Debug.DrawRay(d.transform.position, leftHandle * maxDistance, Color.yellow);
        Debug.DrawRay(d.transform.position, rightHandle * maxDistance, Color.yellow);
        Debug.DrawRay(d.transform.position, centerLine * maxDistance, Color.green);
        Debug.DrawRay(d.transform.position, otherCenterLine * maxDistance, Color.green);
        //Debug.DrawRay(d.transform.position,(direction * maxDistance) * d.transform.forward, Color.yellow);
    }

    private Vector3 ArcVector(float angle, float maxDistance)
    {
        float dividedAngle = angle / 2;
        // float dividedAngle = d.transform.forward * angle / 2;
        // Vector3 arcStart = forwardPointMinusHalfAngle * d.MaxDistance;
        Vector3 forwardPointMinusHalfAngle = Quaternion.Euler(0, -d.Angle, 0) * d.transform.forward;
        Vector3 pointMinusHalfAngle = Quaternion.Euler(0, d.Angle, 0) * d.transform.forward;
        Vector3 centerLine = Quaternion.Euler(0, d.Angle / 2, 0) * d.transform.forward;
        //Vector3 quaterLine = Quaternion.Euler(0, d.Angle / 4, 0) * d.transform.forward;
        //Vector3 halfLine = Quaternion.Euler(0, -d.Angle / 4, 0) * d.transform.forward;
        Vector3 arcStart = forwardPointMinusHalfAngle * d.MaxDistance;

        Vector3 smallLine = Quaternion.Euler(0, -d.Angle / 16, 0) * d.transform.forward;
        Vector3 negativeLine = Quaternion.Euler(0, d.Angle / 16, 0) * d.transform.forward;

        Vector3 direction = new Vector3(Mathf.Cos(dividedAngle * Mathf.Deg2Rad), 0, Mathf.Sin(dividedAngle * Mathf.Deg2Rad));

        Debug.DrawRay(d.transform.position, centerLine * maxDistance, Color.green);
        //Debug.DrawRay(d.transform.position, quaterLine * maxDistance, Color.yellow);
        //Debug.DrawRay(d.transform.position, halfLine * maxDistance, Color.yellow);
        // Debug.DrawRay(d.transform.position, smallLine * maxDistance, Color.yellow);
        // Debug.DrawRay(d.transform.position, negativeLine * maxDistance, Color.yellow);

        Debug.DrawRay(d.transform.position, arcStart, Color.yellow);

        //Debug.DrawRay(d.transform.position, , Color.yellow);

        Vector3 otherDirection = new Vector3(Mathf.Cos(-dividedAngle * Mathf.Deg2Rad), 0, Mathf.Sin(-dividedAngle * Mathf.Deg2Rad));
        Debug.DrawRay(d.transform.position, pointMinusHalfAngle * maxDistance, Color.yellow);

        return arcStart;
    }

    private void DivideIntoSegments(float angle)
    {
        Vector3 leftHandle = Quaternion.Euler(0, angle / 2, 0) * d.transform.forward;
        Vector3 rightHandle = Quaternion.Euler(0, -angle / 2, 0) * d.transform.forward;
        Vector3 centerOffset = new Vector3(d.transform.position.x, d.transform.position.y + d.HeightOffset, d.transform.position.z + d.CenterOffset);
        Debug.DrawRay(centerOffset, leftHandle * d.MaxDistance, Color.yellow);
        Debug.DrawRay(centerOffset, rightHandle * d.MaxDistance, Color.yellow);
        Debug.DrawRay(centerOffset, d.transform.forward * d.MaxDistance, Color.magenta);//Center Line

        float segmentSizes = (angle / 2);
        Handles.color = new Color(1, 1, 1, 0.1f);

        for (int i = 0; i < d.SegmentNumber; i++)
        {
            Vector3 direction = Quaternion.Euler(0, GetAngle(segmentSizes, d.SegmentNumber, i), 0) * d.transform.forward;
            Vector3 mirrorDirection = Quaternion.Euler(0, GetAngle(-segmentSizes, d.SegmentNumber, i), 0) * d.transform.forward;
            //Debug.Log(segmentSizes);
            //Debug.Log(GetAngle(segmentSizes, d.SpokeNumber, i));
            Debug.DrawRay(centerOffset, direction * d.MaxDistance, Color.green);
            Debug.DrawRay(centerOffset, mirrorDirection * d.MaxDistance, Color.cyan);

            RaycastHit hit;
            if (Physics.Raycast(centerOffset, mirrorDirection, out hit, d.MaxDistance) || Physics.Raycast(centerOffset, direction, out hit, d.MaxDistance))
            {
                Debug.Log(hit.collider.gameObject.name);
                Handles.color = new Color(1, 0.92f, 0.016f, 0.08f);
            }

        }
    }
    private float GetAngle(float angle, float segmentDivisions, float currentSegment)
    {
        if (segmentDivisions == 1)
        {
            return angle = angle / 2;
        }
        float segments = angle / segmentDivisions;
        float angleDegree = (angle - (segments * (currentSegment + 1)));
        return angleDegree;
    }
}
