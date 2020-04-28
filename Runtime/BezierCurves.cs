using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierCurves
{
    //Dictionary used for Dynamic Programming to avoid identical function calls
    //both are used for Generic BezierCurves calls
    private static Dictionary<(List<Vector3>,float), Vector3> oldBeziersLerps = new Dictionary<(List<Vector3>,float), Vector3>();
    private static Dictionary<(List<Vector3>,int), List<Vector3>> oldBeziersLists = new Dictionary<(List<Vector3>,int), List<Vector3>>();
    
    /// <summary>
    /// Returns a point on the Bezier Curve given 3 control points of the curve and a percentage (0 to 1) along the curve
    /// </summary>
    /// <param name="a">The starting position of the curve</param>
    /// <param name="b">The middle control point of the curve</param>
    /// <param name="c">The ending position of the curve</param>
    /// <param name="t">
    /// A percentage (0 to 1, both included) of path along the curve,
    /// a value of 0 will return point a,
    /// a value of 1 will return point c,
    /// a value of 0.5 will return the middle point of the curve,
    /// similar to that a value between 0 and 1 will return a point inbetween the curve, t * BezierLength along the path
    /// </param>
    /// <returns>A point t * BezierLength along the path</returns>
    public static Vector3 Quadratic(Vector3 a, Vector3 b, Vector3 c, float t){
        Vector3 p0 = Vector3.Lerp(a,b,t);
        Vector3 p1 = Vector3.Lerp(b,c,t);

        return Vector3.Lerp(p0, p1, t);
    }
    
    /// <summary>
    /// Returns a point on the Bezier Curve given 4 control points of the curve and a percentage (0 to 1) along the curve
    /// </summary>
    /// <param name="a">The starting position of the curve</param>
    /// <param name="b">The first control point of the curve</param>
    /// <param name="c">The second control point of the curve</param>
    /// <param name="d">The ending position of the curve</param>
    /// <param name="t">
    /// A percentage (0 to 1, both included) of path along the curve,
    /// a value of 0 will return point a,
    /// a value of 1 will return point c,
    /// a value of 0.5 will return the middle point of the curve,
    /// similar to that a value between 0 and 1 will return a point inbetween the curve, t * BezierLength along the path
    /// </param>
    /// <returns>A point t * BezierLength along the path</returns>
    public static Vector3 Cubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t){
        Vector3 b0 = Vector3.Lerp(a,b,t);
        Vector3 b1 = Vector3.Lerp(b,c,t);
        Vector3 b2 = Vector3.Lerp(c,d,t);

        Vector3 p0 = Vector3.Lerp(b0, b1, t);
        Vector3 p1 = Vector3.Lerp(b1, b2, t);

        return Vector3.Lerp(p0, p1, t);
    }
    
    /// <summary>
    /// Returns a point on the Bezier Curve given a list of control points of the curve and a percentage (0 to 1) along the curve
    /// </summary>
    /// <param name="controlPoints">A non empty, non null list of control points of the curve, must have > 1 Vector3 inside</param>
    /// <param name="t">
    /// A percentage (0 to 1, both included) of path along the curve,
    /// a value of 0 will return point a,
    /// a value of 1 will return point c,
    /// a value of 0.5 will return the middle point of the curve,
    /// similar to that a value between 0 and 1 will return a point inbetween the curve, t * BezierLength along the path
    /// </param>
    /// <returns>A point t * BezierLength along the path, Vector3.zero if there are less than 2 points</returns>
    public static Vector3 Generic(List<Vector3> controlPoints, float t){
        int pointsNum = controlPoints.Count;
        if(pointsNum < 2){
            Debug.LogError("Cannot create a Bezier with less than 2 points");
            return Vector3.zero;
        }
        
        //Dynamic Programming: check if already calculated an return it
        if(oldBeziersLerps.ContainsKey((controlPoints,t))) return oldBeziersLerps[(controlPoints,t)];
        Vector3 output;
        
        //base case
        if(pointsNum == 2){
            //Dynamic Programming: add calculated point to the list
            output = Vector3.Lerp(controlPoints[0], controlPoints[1], t);
            oldBeziersLerps.Add((controlPoints,t), output);
            return output;
        }
        
        //divide: creates lists with points [0..n-1] and [1..n]
        List<Vector3> sub1 = new List<Vector3>(pointsNum-1);
        List<Vector3> sub2 = new List<Vector3>(controlPoints.Count-1);
        sub1.Add(controlPoints[0]);
        for(int i = 1; i < pointsNum-1; i++){
            sub1.Add(controlPoints[i]);
            sub2.Add(controlPoints[i]);
        }
        sub2.Add(controlPoints[pointsNum-1]);
        
        //recurse: a bezier needs the point of the sub curve
        Vector3 p0 = Generic(sub1, t);
        Vector3 p1 = Generic(sub2, t);
        
        //conquer: calcolate new point and return it
        //Dynamic Programming: add calculated point to the list
        output = Vector3.Lerp(p0, p1, t);
        oldBeziersLerps.Add((controlPoints,t), output);
        return output;
    }
    
    /// <summary>
    /// Returns a list of points on the Bezier curve given 3 control points of the curve and how many points to return
    /// </summary>
    /// <param name="a">The starting position of the curve</param>
    /// <param name="b">The middle control point of the curve</param>
    /// <param name="c">The ending position of the curve</param>
    /// <param name="pointsNumb">How many points to calculate, must be > 1</param>
    /// <returns>A list of pointsNumb points along the curve, null if pointsNumb <= 1</returns>
    public static List<Vector3> QuadraticPointsList(Vector3 a, Vector3 b, Vector3 c, int pointsNumb){
        if(pointsNumb <= 1){
            Debug.LogError("Bezier curve must calculate more than 2 points");
            return null;
        }
        List<Vector3> output = new List<Vector3>();
        for(float t=0; t <=1; t += 1f/(pointsNumb-1)){
            output.Add(Quadratic(a, b, c, t));
        }

        return output;
    }
    
    /// <summary>
    /// Returns a list of points on the Bezier curve given 4 control points of the curve and how many points to return
    /// </summary>
    /// <param name="a">The starting position of the curve</param>
    /// <param name="b">The first control point of the curve</param>
    /// <param name="c">The second control point of the curve</param>
    /// <param name="d">The ending position of the curve</param>
    /// <param name="pointsNumb">How many points to calculate, must be > 1</param>
    /// <returns>A list of pointsNumb points along the curve, null if pointsNumb <= 1</returns>
    public static List<Vector3> CubicPointsList(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int pointsNumb){
        if(pointsNumb <= 1){
            Debug.LogError("Bezier curve must calculate more than 2 points");
            return null;
        }
        List<Vector3> output = new List<Vector3>();
        float step = 1f/(pointsNumb-1);
        for(float t=0; t <=1; t += step){
            output.Add(Cubic(a, b, c, d, t));
        }

        return output;
    }
    
    /// <summary>
    /// Returns a list of points on the Bezier curve given a list of control points of the curve and how many points to return
    /// </summary>
    /// <param name="controlPoints">A non empty, non null list of control points of the curve, must have > 1 Vector3 inside</param>
    /// <param name="pointsNumb">How many points to calculate, must be > 1</param>
    /// <returns>A list of pointsNumb points along the curve, null if pointsNumb <= 1 or pointsNumb < 2</returns>
    public static List<Vector3> GenericPointsList(List<Vector3> controlPoints, int pointsNumb){
        int controlPointsNum = controlPoints.Count;
        if(controlPointsNum < 2){
            Debug.LogError("Cannot create a Bezier with less than 2 points");
            return null;
        }
        if(pointsNumb < 2){
            Debug.LogError("Cannot calculate a Bezier of less than 2 points");
            return null;
        }
        
        //initialize memory used
        List<Vector3> output = new List<Vector3>(controlPointsNum);
        float step = 1f/(pointsNumb-1);
        
        //Dynamic Programming: check if already calculated an return it
        if(oldBeziersLists.ContainsKey((controlPoints, pointsNumb))) return oldBeziersLists[(controlPoints, pointsNumb)];
        
        //base case
        if(controlPointsNum == 2){
            for(int i = 0; i < pointsNumb; i++){
                output.Add(Vector3.Lerp(controlPoints[0], controlPoints[1], i*step));
            }
            //Dynamic Programming: add calculated point to the list
            oldBeziersLists.Add((controlPoints, pointsNumb), output);
            return output;
        }
        
        //divide: creates lists with points [0..n-1] and [1..n]
        List<Vector3> sub1 = new List<Vector3>(controlPointsNum-1);
        List<Vector3> sub2 = new List<Vector3>(controlPoints.Count-1);
        sub1.Add(controlPoints[0]);
        for(int i = 1; i < controlPointsNum-1; i++){
            sub1.Add(controlPoints[i]);
            sub2.Add(controlPoints[i]);
        }
        sub2.Add(controlPoints[controlPointsNum-1]);
        
        //recurse: a bezier needs the point of the sub curve
        List<Vector3> c0 = GenericPointsList(sub1, pointsNumb);
        List<Vector3> c1 = GenericPointsList(sub2, pointsNumb);
        
        //conquer: calcolate new points and return it
        for(int i = 0; i < c0.Count; i++){
            output.Add(Vector3.Lerp(c0[i], c1[i], step * i));
        }
        //Dynamic Programming: add calculated point to the list
        oldBeziersLists.Add((controlPoints, pointsNumb), output);
        return output;
    }
    
    /// <summary>
    /// Cleans the memory footprint created by smart implementations of the Bezier Curve
    /// </summary>
    public static void CleanMemory(){
        //free old memory so it can be removed by the garbage collector
        oldBeziersLerps.Clear();
        oldBeziersLists.Clear();
    }
}
