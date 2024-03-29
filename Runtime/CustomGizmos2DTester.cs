using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class CustomGizmos2DTester : MonoBehaviour
{
    public float arrowTipSize = 0.2f;
    [Range(0f, 1f)]
    public float lerpParam = 0.5f;
    
    [Range(0, 100)]
    public int resolution = 20;
    
    public bool resetMemory = false;
    
    public float speed;
    
    [Header("Arrow Heap Params")]
    public float arrowHeadAmplitude = 1f;
    public float arrowHeadOffset = 1f;
    public Vector2 arrowOffset;
    
    [Header("Lines Params")]
    public float linesAmplitude = 0.2f;
    public float linesOffset = 0.6f;
    
    [Header("Rect Param")]
    public float rectSizeAmplitude = 0.4f;
    
    [Header("Poligons Params")]
    public float sidesCountAmplitude = 5f;
    [Range(0f, 60f)]
    public float normalAmplitude = 1f;
    [Range(0f, 2f)]
    public float polygonsRadius = 2f;
    public float positionAmplitude = 0.5f;
    
    [Header("Circle Params")]
    [Range(0f, 2f)]
    public float circleRadius = 2f;
    
    public List<Vector2> bezPoints;
    
    [ReadOnly] public float currentAngle;
    [FGraph] public float lerpingParam;
    private float lerpingHalfParam;
    private float editorDeltaTime;
    private float lastEditorTime;
    [Graph] public float sinParam;
    private float doubleSinParam;
    
    private void OnDrawGizmos()
    {
        //calculate editor time
        Gizmos.color = Color.green;
        float currentTime = Time.realtimeSinceStartup;
        editorDeltaTime = currentTime - lastEditorTime;
        
        // Common lerp values used throught the code
        lerpingParam += speed * editorDeltaTime;
        lerpingParam %= 1f;
        lerpingHalfParam += speed * editorDeltaTime / 2;
        lerpingHalfParam %= 1f;
        currentAngle += speed * 360 * editorDeltaTime;
        currentAngle %= 360;

        sinParam = Mathf.Sin(lerpingParam * 2 * Mathf.PI);
        doubleSinParam = Mathf.Sin(lerpingParam * 4 * Mathf.PI);
        
        //Draw all shapes        
        Arrow();
        ArrowHead();
        Lines();
        HGizmo();
        LineArrow();
        Rectangle();
        Polygons();
        Circle();
        Ellipse();
        Bezier();
        BezierArrow();
        Profiler.BeginSample("2D Bezier");
        CustomGizmos2D.Bezier(bezPoints, 40);
        Profiler.EndSample();
        //reset params
        MemoryReset();
        lastEditorTime = currentTime;
    }
    
    /// <summary>
    /// Calculates and draws arrow
    /// </summary>
    private void Arrow()
    {
        Vector3 arrowHeadPos = Vector3.up * (sinParam * arrowHeadAmplitude + arrowHeadOffset);
        
        CustomGizmos2D.DrawArrow(arrowHeadPos, Vector3.down, 1f, arrowTipSize);
        
        CustomGizmos2D.DrawArrow(arrowOffset, lerpingParam * 2 * Mathf.PI, 1f, arrowTipSize);
    }
    
    /// <summary>
    /// Calculates arrow head position and draws it
    /// </summary>
    private void ArrowHead()
    {
        Vector3 arrowHeadPos = Vector3.up * (sinParam * arrowHeadAmplitude + arrowHeadOffset) + new Vector3(0.5f, 0, 0.5f);
        
        CustomGizmos2D.DrawArrowTip(arrowHeadPos, Vector3.down, arrowTipSize);
    }
    
    /// <summary>
    /// Calculates lines positions and draws them
    /// </summary>
    private void Lines()
    {
        CustomGizmos2D.DrawLine(new Vector2(1f,   0), new Vector2(1f,   1), 1f - sinParam * linesAmplitude - linesOffset, 0);
        CustomGizmos2D.DrawLine(new Vector2(1.2f, 0), new Vector2(1.2f, 1), sinParam * linesAmplitude + linesOffset);
        CustomGizmos2D.DrawLine(new Vector2(1.4f, 0), new Vector2(1.4f, 1), 0, 1 - sinParam * linesAmplitude - linesOffset);
    }
    
    /// <summary>
    /// Calculates and draws an H gizmo
    /// </summary>
    private void HGizmo(){
        CustomGizmos2D.DrawH(new Vector2(2, 2), new Vector2(3, 2 + sinParam), arrowTipSize);
    }
    
    /// <summary>
    /// Calculates and draws line arrows
    /// </summary>
    private void LineArrow(){
        CustomGizmos2D.DrawLineArrow(new Vector2(2, 0),  new Vector2(2, 1), arrowTipSize, (sinParam + 1) / 2);
    }
    
    /// <summary>
    /// Calculates and draws a rectangle
    /// </summary>
    private void Rectangle(){
        float rectSinParam = Mathf.Sin(lerpingHalfParam * 2 * Mathf.PI);
        float rectCosParam = Mathf.Cos(lerpingHalfParam * 2 * Mathf.PI);
        Vector2 halfSizes = new Vector2(
            1.5f * (rectSinParam * rectSizeAmplitude + (1 - rectSizeAmplitude)),
            1    * (rectCosParam * rectSizeAmplitude + (1 - rectSizeAmplitude))
        );
        CustomGizmos2D.DrawCrossedRect(new Vector3(4,0,1.5f), sinParam, halfSizes, 5);
    }
    
    /// <summary>
    /// Draws regular polygons animation
    /// </summary>
    private void Polygons(){
        int sidesCount = 3 + (int)(sidesCountAmplitude * (sinParam + 1) / 2);
        Vector3 currentDirection = Quaternion.Euler(0, 0, doubleSinParam * normalAmplitude) * Vector3.right;
        CustomGizmos2D.DrawRegularPoligon(new Vector2(4, 2.3f), currentDirection * polygonsRadius, sidesCount);
    }
    
    /// <summary>
    /// Draws a rotating circle
    /// </summary>
    private void Circle(){
        CustomGizmos2D.DrawRegularPoligon(new Vector2(6, 2), Vector3.right * circleRadius * sinParam, 30);
    }
    
    private void Ellipse(){
        CustomGizmos2D.DrawEllipse(new Vector2(7, 4), sinParam, new Vector2(1.5f, 0.5f), 20);
    }
    
    
    /// <summary>
    /// Calculates and draws bezier curves
    /// </summary>
    private void Bezier(){
        CustomGizmos2D.Bezier(new Vector3(7, 0), new Vector3(6.5f, 1), new Vector3(6, 0), 10);
        CustomGizmos2D.CubicBezier(new Vector2(1, 3), new Vector2(.5f, 4), new Vector2(0, 3), new Vector2(-1, 4), 10);
    }
    
    /// <summary>
    /// Calculates and draws a bezier curve with arrow
    /// </summary>
    private void BezierArrow(){
        List<Vector2> bezierPoints = new List<Vector2>();
        bezierPoints.Add(new Vector2(3, 4));
        bezierPoints.Add(new Vector2(1, 4));
        bezierPoints.Add(new Vector2(2, 5));
        bezierPoints.Add(new Vector2(0, 5));
        float bezSinParam = Mathf.Sin(lerpingHalfParam * 2 * Mathf.PI);
        CustomGizmos2D.BezierWithArrow(bezierPoints, 10, (bezSinParam + 1) / 2, arrowTipSize);
        
    }
    
    /// <summary>
    /// Resets memory footprint for bezier if necessary
    /// </summary>
    private void MemoryReset(){
        if(resetMemory){
            resetMemory = false;
            CustomGizmos2D.ResetBezierMemory();
        }
    }
}
