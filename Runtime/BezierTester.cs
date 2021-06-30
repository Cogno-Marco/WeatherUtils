using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class BezierTester : MonoBehaviour
{
    [Range(0f,1f)]
    public float t = 0f;
    [Range(0,100)]
    public int pointsNumber = 10;
    public bool shouldClean = false;
    
    public float sphereSize = 0.1f;
    
    private List<Vector3> controlPoints = new List<Vector3>();
    private Vector3 lerpBezierPos;
    private List<Vector3> bezierPoints = new List<Vector3>();
    
    void Update() {
        //clean memory and skip this frame
        if(shouldClean){
            shouldClean = false;
            BezierCurves.CleanMemory();
            return;
        }
        
        //if there are 3 childrens, calculate quadratic curves
        if(transform.childCount == 3){
            lerpBezierPos = BezierCurves.Quadratic(
                transform.GetChild(0).position,
                transform.GetChild(1).position,
                transform.GetChild(2).position,
                t
            );
            bezierPoints = new List<Vector3>(BezierCurves.QuadraticPointsList(
                transform.GetChild(0).position,
                transform.GetChild(1).position,
                transform.GetChild(2).position,
                pointsNumber
            ));
        }
        //if there are 4 childrens, calculate cubic curves
        else if(transform.childCount == 4){
            lerpBezierPos = BezierCurves.Cubic(
                transform.GetChild(0).position,
                transform.GetChild(1).position,
                transform.GetChild(2).position,
                transform.GetChild(3).position,
                t
            );
            bezierPoints = new List<Vector3>(BezierCurves.CubicPointsList(
                transform.GetChild(0).position,
                transform.GetChild(1).position,
                transform.GetChild(2).position,
                transform.GetChild(3).position,
                pointsNumber
            ));
        }
        //else if the are more than 4, calculate generic curves
        else if (transform.childCount > 4){
            controlPoints = new List<Vector3>();
            for(int i = 0; i < transform.childCount; i++){
                controlPoints.Add(transform.GetChild(i).position);
            }
            Profiler.BeginSample("Bezier Point Calculation");
            lerpBezierPos = BezierCurves.Generic(controlPoints.ToArray(), t);
            Profiler.EndSample();
            Profiler.BeginSample("Bezier List Calculation");
            bezierPoints = new List<Vector3>(BezierCurves.GeneralBezier(controlPoints.ToArray(), pointsNumber));
            Profiler.EndSample();
        }
    }
    
    
    private void OnDrawGizmos() {
        //draw single point curve
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(lerpBezierPos, sphereSize);
        
        //draw control points
        Gizmos.color = Color.gray;
        for(int i = 0; i < transform.childCount; i++){
            Gizmos.DrawSphere(transform.GetChild(i).position, sphereSize);
        }
        
        //if available draw the whole curve
        if(bezierPoints == null) return;
        
        Gizmos.color = Color.yellow;
        for(int i = 0; i < bezierPoints.Count-1; i++){
            Vector3 pointA = bezierPoints[i];
            Vector3 pointB = bezierPoints[i+1];
            Gizmos.DrawLine(pointA, pointB);
        }
    }
}
