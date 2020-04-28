using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGizmosTester : MonoBehaviour
{
    public float arrowTipSize = 0.2f;
    [Range(0f,1f)]
    public float lerpParam = 0.5f;
    
    [Range(0, 100)]
    public int resolution = 20;
    
    public bool resetMemory = false;
    
    public float speed;
    
    [Header("Arrow Heap Params")]
    public float arrowHeadAmplitude = 1f;
    public float arrowHeadOffset = 1f;
    
    [Header("Lines Params")]
    public float linesAmplitude = 0.2f;
    public float linesOffset = 0.6f;
    
    [Header("Rect Param")]
    public float rectSizeAmplitude = 0.4f;
    
    private float currentAngle;
    private float lerpingParam;
    private float lerpingHalfParam;
    private float editorDeltaTime;
    private float lastEditorTime;
    private float sinParam;
    
    private void OnDrawGizmos() {
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
        
        //Draw all shapes        
        ArrowHead();
        Lines();
        Arrow();
        LineArrow();
        HGizmo();
        Bezier();
        BezierArrow();
        Rectangle();
        
        //reset params
        MemoryReset();
        lastEditorTime = currentTime;
    }
    
    /// <summary>
    /// Calculates arrow head position and draws it
    /// </summary>
    private void ArrowHead(){
        Vector3 arrowHeadPos = Vector3.up * (sinParam * arrowHeadAmplitude + arrowHeadOffset) + new Vector3(0.5f, 0, 0.5f);
        Vector3 currentDirection = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward;
        
        CustomGizmos.DrawArrowTip(arrowHeadPos, Vector3.down, arrowTipSize, currentDirection);
    }
    
    /// <summary>
    /// Calculates lines positions and draws them
    /// </summary>
    private void Lines(){
        CustomGizmos.DrawLine(new Vector3(1f,0,0), new Vector3(1f,0,1), 1f - sinParam * linesAmplitude - linesOffset, 0);
        CustomGizmos.DrawLine(new Vector3(1.2f,0,0), new Vector3(1.2f,0,1), sinParam * linesAmplitude + linesOffset);
        CustomGizmos.DrawLine(new Vector3(1.4f,0,0), new Vector3(1.4f,0,1), 0, 1 - sinParam * linesAmplitude - linesOffset);
    }
    
    /// <summary>
    /// Calculates and draws arrow
    /// </summary>
    private void Arrow(){
        Vector3 arrowHeadPos = Vector3.up * (sinParam * arrowHeadAmplitude + arrowHeadOffset) + new Vector3(0.5f, 1f, 1.5f);
        Vector3 currentDirection = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward;
        
        CustomGizmos.DrawArrow(arrowHeadPos, arrowHeadPos + Vector3.down, arrowTipSize, currentDirection);
    }
    
    /// <summary>
    /// Calculates and draws line arrows
    /// </summary>
    private void LineArrow(){
        Vector3 currentDirection = Quaternion.AngleAxis(currentAngle, Vector3.forward) * Vector3.up;
        
        CustomGizmos.DrawLineArrow(new Vector3(2,0,0),  new Vector3(2, 0, 1), arrowTipSize, (sinParam + 1) / 2, currentDirection);
    }
    
    /// <summary>
    /// Calculates and draws an H gizmo
    /// </summary>
    private void HGizmo(){
        Vector3 currentDirection = Quaternion.AngleAxis(currentAngle, Vector3.forward) * Vector3.up;
        CustomGizmos.DrawH(new Vector3(2,0,2), new Vector3(2,0,3), arrowTipSize, currentDirection);
    }
    
    /// <summary>
    /// Calculates and draws bezier curves
    /// </summary>
    private void Bezier(){
        CustomGizmos.Bezier(new Vector3(1,0,2), new Vector3(.5f,0,3), new Vector3(0,0,2), 10);
        CustomGizmos.CubicBezier(new Vector3(1,0,3), new Vector3(.5f,0,4), new Vector3(0,0,3), new Vector3(-1, 0, 4), 10);
    }
    
    /// <summary>
    /// Calculates and draws a bezier curve with arrow
    /// </summary>
    private void BezierArrow(){
        Vector3 currentDirection = Quaternion.AngleAxis(currentAngle, new Vector3(-1,0,1).normalized) * Vector3.up;
        List<Vector3> bezierPoints = new List<Vector3>();
        bezierPoints.Add(new Vector3(3,0,4));
        bezierPoints.Add(new Vector3(1,0,4));
        bezierPoints.Add(new Vector3(2,0,5));
        bezierPoints.Add(new Vector3(0,0,5));
        float bezSinParam = Mathf.Sin(lerpingHalfParam * 2 * Mathf.PI);
        CustomGizmos.BezierWithArrow(bezierPoints, 10, (bezSinParam + 1) / 2, arrowTipSize, currentDirection);
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
        CustomGizmos.DrawCrossedRect(new Vector3(4,0,1.5f), Vector3.forward, Vector3.right, halfSizes, 5);
    }
    
    /// <summary>
    /// Resets memory footprint for bezier if necessary
    /// </summary>
    private void MemoryReset(){
        if(resetMemory){
            resetMemory = false;
            CustomGizmos.ResetBezierMemory();
        }
    }
}
