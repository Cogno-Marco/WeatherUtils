using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Profiling;

public static class BezierCurves
{
    //Dictionary used for Dynamic Programming to avoid identical function calls
    //both are used for Generic BezierCurves calls
    private static Dictionary<BezierKeyPoints<int>, Vector3[]> oldBeziersListsMap = new Dictionary<BezierKeyPoints<int>, Vector3[]>();
    private static Dictionary<BezierKeyPoints<float>, Vector3> oldBeziersLerpsMap = new Dictionary<BezierKeyPoints<float>, Vector3>();
    
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
    public static Vector3 Generic(Vector3[] controlPoints, float t){
        int pointsNum = controlPoints.Length;
        if(pointsNum < 2){
            Debug.LogError("Cannot create a Bezier with less than 2 points");
            return Vector3.zero;
        }
        
        Profiler.BeginSample("Case checking");
        //Dynamic Programming: check if already calculated an return it
        if(oldBeziersLerpsMap.ContainsKey((controlPoints,t))){
            Profiler.EndSample();
            return oldBeziersLerpsMap[(controlPoints,t)];
        }
        
        //base case
        if(pointsNum == 2){
            //Dynamic Programming: add calculated point to the list
            Vector3 baseCaseOutput = Vector3.Lerp(controlPoints[0], controlPoints[1], t);
            oldBeziersLerpsMap.Add((controlPoints,t), baseCaseOutput);
            Profiler.EndSample();
            return baseCaseOutput;
        }
        
        Profiler.EndSample();
        Profiler.BeginSample("Recursive Case Single Point");
        //divide: creates lists with points [0..n-1] and [1..n]
        Vector3[] sub1 = new Vector3[pointsNum - 1];
        Vector3[] sub2 = new Vector3[pointsNum - 1];
        sub1[0] = controlPoints[0];
        for(int i = 1; i < pointsNum-1; i++){
            sub1[i] = controlPoints[i];
            sub2[i - 1] = controlPoints[i];
        }
        sub2[pointsNum - 2] = controlPoints[pointsNum-1];
        
        //recurse: a bezier needs the point of the sub curve
        Vector3 p0 = Generic(sub1, t);
        Vector3 p1 = Generic(sub2, t);
        
        //conquer: calcolate new point and return it
        //Dynamic Programming: add calculated point to the list
        Vector3 output = Vector3.Lerp(p0, p1, t);
        oldBeziersLerpsMap.Add((controlPoints,t), output);
        Profiler.EndSample();
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
    public static Vector3[] QuadraticPointsList(Vector3 a, Vector3 b, Vector3 c, int pointsNumb){
        if(pointsNumb <= 1){
            Debug.LogError("Bezier curve must calculate more than 2 points");
            return null;
        }
        return GeneralBezier(new Vector3[]{a,b,c}, pointsNumb);
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
    public static Vector3[] CubicPointsList(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int pointsNumb){
        if(pointsNumb <= 1){
            Debug.LogError("Bezier curve must calculate more than 2 points");
            return null;
        }
        return GeneralBezier(new Vector3[]{a,b,c,d}, pointsNumb);
    }
    
    /// <summary>
    /// Returns a list of points sampled from the bezier curve
    /// </summary>
    /// <param name="points">The list of control points to create a bezier curve from, must not be null and must be >= 2 in length</param>
    /// <param name="pointsNumb">How many points to sample from the curve, must be >= 0 </param>
    /// <returns>Returns a list of points interpolated from the bezier curve</returns>
    public static Vector3[] GeneralBezier(Vector3[] points, int pointsNumb){
        if(points.Length < 2) throw new System.ArgumentException("there must be at least 2 points to work!");
        
        //base cases
        if(oldBeziersListsMap.ContainsKey((points, pointsNumb)))
            return oldBeziersListsMap[(points, pointsNumb)];
        
        if(points.Length == 2){
            oldBeziersListsMap.Add((points, pointsNumb), LinearBezier(points[0], points[1], pointsNumb));
            return oldBeziersListsMap[(points, pointsNumb)];
        }
        
        Profiler.BeginSample("Recursive Case Full List");
        //recursive case
        //1. create 2 lists, the first with the point from 0 to n-1, the second with the points from 1 to n
        Vector3[] arr1 = new Vector3[points.Length - 1];
        Vector3[] arr2 = new Vector3[points.Length - 1];
        
        for(int i = 0; i < points.Length - 1; i++){
            arr1[i] = points[i];
            arr2[i] = points[i+1];
        }
        
        //recursive call itself to get intermediate points
        Vector3[] interm1 = GeneralBezier(arr1, pointsNumb);
        Vector3[] interm2 = GeneralBezier(arr2, pointsNumb);
        
        //final lerp these 2 curves
        float step = 1f / (pointsNumb - 1);
        Vector3[] output = new Vector3[interm1.Length];
        for(int i = 0; i < interm1.Length; i++){
            output[i] = Vector3.Lerp(interm1[i], interm2[i], step * i);
        }
        oldBeziersListsMap.Add((points, pointsNumb), output);
        Profiler.EndSample();
        return output;
    }
    
    /// <summary>
    /// Base case: linear implementation of a bezier curve, basically a lerp
    /// </summary>
    /// <param name="aPos">The starting position of the bezier curve</param>
    /// <param name="bPos">The ending position of the bezier curve</param>
    /// <param name="pointsNumb">how many points to sample from the curve</param>
    /// <returns>Returns a list of points interpolated from the bezier curve</returns>
    private static Vector3[] LinearBezier(Vector3 aPos, Vector3 bPos, int pointsNumb){
        Vector3[] output = new Vector3[pointsNumb];
        for(int i = 0; i < pointsNumb; i++){
            output[i] = Vector3.Lerp(aPos, bPos, (float)(i) / (pointsNumb - 1));
        }
        return output;
    }
    
    /// <summary>
    /// Cleans the memory footprint created by smart implementations of the Bezier Curve
    /// </summary>
    public static void CleanMemory(){
        //clear memory maps
        oldBeziersLerpsMap.Clear();
        oldBeziersListsMap.Clear();
    }
    
    private struct BezierKeyPoints<T>
    {
        public Vector3[] keyPoints;
        public T resolution;

        public BezierKeyPoints(Vector3[] keyPoints, T resolution)
        {
            this.keyPoints = keyPoints;
            this.resolution = resolution;
        }

        public override bool Equals(object obj)
        {
            return obj is BezierKeyPoints<T> other && Comparer<T>.Default.Compare(resolution, other.resolution) == 0 &&
                keyPoints.SequenceEqual(other.keyPoints);
        }

        public override int GetHashCode()
        {
            int hashCode = -1030903623;
            foreach (Vector3 item in keyPoints)
                hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + resolution.GetHashCode();
            return hashCode;
        }

        public void Deconstruct(out Vector3[] item1, out T item2)
        {
            item1 = keyPoints;
            item2 = resolution;
        }

        public static implicit operator (Vector3[], T)(BezierKeyPoints<T> value)
        {
            return (value.keyPoints, value.resolution);
        }

        public static implicit operator BezierKeyPoints<T>((Vector3[], T) value)
        {
            return new BezierKeyPoints<T>(value.Item1, value.Item2);
        }
    }
}
