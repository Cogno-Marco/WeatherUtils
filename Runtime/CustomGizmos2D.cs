using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for using 2d version of gizmos.
/// 2D gizmos will be placed in the xy plane (unity's default 2d plane)
/// TODO: enable usage in xz and yz plane using an enum or something
/// </summary>
public class CustomGizmos2D
{
    /// <summary>
    /// Draws a gizmo2d arrow at startPos pointing at endPos
    /// </summary>
    /// <param name="startPos">The point where the arrow starts from</param>
    /// <param name="endPos">The point where the arrow points to</param>
    /// <param name="tipSize">The size of the arrow head</param>
    public static void DrawArrow(Vector2 startPos, Vector2 endPos, float tipSize){
        CustomGizmos.DrawArrow(startPos, endPos, tipSize, Vector3.forward);
    }
    
    /// <summary>
    /// Draws a gizmo2d arrow at startPos and with a direction and a length, using polar coordinates
    /// </summary>
    /// <param name="startPos">The point where the arrow starts from</param>
    /// <param name="direction">The direction the arrow points towards</param>
    /// <param name="length">The length of the arrow</param>
    /// <param name="tipSize">The size of the arrow head</param>
    public static void DrawArrow(Vector2 startPos, Vector2 direction, float length, float tipSize){
        DrawArrow(startPos, startPos + direction.normalized * length, tipSize);
    }
    
    /// <summary>
    /// Draws a gizmo2d arrow at startPos and with a direction and a length, using polar coordinates
    /// Asks for an angle instead of a direction
    /// </summary>
    /// <param name="startPos">The point where the arrow starts from</param>
    /// <param name="angle">The angle the arrow points towards (in radians)</param>
    /// <param name="length">The length of the arrow</param>
    /// <param name="tipSize">The size of the arrow head</param>
    public static void DrawArrow(Vector2 startPos, float angle, float length, float tipSize){
        DrawArrow(startPos, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), length, tipSize);
    }
    
    /// <summary>
    /// Draws an arrow tip at position, pointing in a direction.
    /// N.B. due to artistic decision the there's some white space between the position of the tip and the actual tip,
    /// also the angle of the tip is 90° instead of 60°, so this is not an equilateral triangle.
    /// </summary>
    /// <param name="position">The position to place the arrow tip</param>
    /// <param name="direction">The direction of the tip</param>
    /// <param name="size">The size of the arrow tip</param>
    public static void DrawArrowTip(Vector2 position, Vector2 direction, float size){
        CustomGizmos.DrawArrowTip(position, direction, size, Vector3.forward);
    }
    
    /// <summary>
    /// Draws a line with empty space at the end and at the start
    /// </summary>
    /// <param name="startPos">The starting position of the line</param>
    /// <param name="endPos">The ending position of the line</param>
    /// <param name="spacing">
    /// The spacing to have both at the start and end of the line, symmetric.
    /// A value of 0 will draw a line equal to Gizmos.DrawLine, meaning with no empty space
    /// </param>
    public static void DrawLine(Vector2 startPos, Vector2 endPos, float spacing){
        CustomGizmos.DrawLine(startPos, endPos, spacing);
    }
    
    /// <summary>
    /// Draws a line with empty space at the end and at the start, not simmetric.
    /// </summary>
    /// <param name="startPos">The starting pos of line</param>
    /// <param name="endPos">The ending pos of the line</param>
    /// <param name="startSpacing">How much empty space to have at the start of the line</param>
    /// <param name="endSpacing">How much empty space to have at the end of the line</param>    
    public static void DrawLine(Vector2 startPos, Vector2 endPos, float startSpacing, float endSpacing){
        CustomGizmos.DrawLine(startPos, endPos, startSpacing, endSpacing);
    }
    
    /// <summary>
    /// Draws a H shaped gizmo
    /// </summary>
    /// <param name="startPos">Starting position of the H</param>
    /// <param name="endPos">Ending position of the H</param>
    /// <param name="lineSize">How big are the H sides</param>
    public static void DrawH(Vector3 startPos, Vector3 endPos, float lineSize){
        CustomGizmos.DrawH(startPos, endPos, lineSize, Vector3.forward);
    }
    
    /// <summary>
    /// Draws a line with an embedded arrow in the middle
    /// </summary>
    /// <param name="startPos">The starting position of the line</param>
    /// <param name="endPos">The ending position of the line</param>
    /// <param name="tipSize">The size of the arrow tip</param>
    /// <param name="placementPercentage">
    /// A float from 0 to 1.
    /// 0 means the arrow tip is at the start of the line (the triangle of the line starts at startPos)
    /// 1 means the arrow tip is at the end of the line (equal to calling DrawArrow)
    /// a value between 0 and 1 means the arror tip is inbetween startPos and endPos
    /// </param>
    public static void DrawLineArrow(Vector3 startPos, Vector3 endPos, float tipSize, float placementPercentage){
        CustomGizmos.DrawLineArrow(startPos, endPos, tipSize, placementPercentage, Vector3.forward);
    }
    
    /// <summary>
    /// Draws a rectangle with lines crossing it
    /// </summary>
    /// <param name="center">Center position of the rectangle</param>
    /// <param name="halfSides">Half-sizes of the edges</param>
    /// <param name="numbOfLines">How many lines cross the rectangle, 0 simply draws a square</param>
    public static void DrawCrossedRect(Vector2 center, Vector2 halfSides, int numbOfLines){
        CustomGizmos.DrawCrossedRect(center, Vector3.right, Vector3.up, halfSides, numbOfLines);
    }
    
    /// <summary>
    /// Draws a rectangle with lines crossing it
    /// </summary>
    /// <param name="center">Center position of the rectangle</param>
    /// <param name="rotation">Counter-clockise rotation of the rectangle (in radians)</param>
    /// <param name="halfSides">Half-sizes of the edges</param>
    /// <param name="numbOfLines">How many lines cross the rectangle, 0 simply draws a square</param>
    public static void DrawCrossedRect(Vector2 center, float rotation, Vector2 halfSides, int numbOfLines){
        CustomGizmos.DrawCrossedRect(
            center, 
            Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg) * Vector3.right,
            Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg) * Vector3.up,
            halfSides, numbOfLines
        );
    }
    
    /// <summary>
    /// Draws a regular n-sided poligon (triangle, square, pentagon and so on) given it's center, the direction and length of it's radius,
    /// how many sides to draw and the normal direction to draw the poligon towards.
    /// Can also be used to draw 2D circles by increasing the number of sides to draw to a really high number
    /// </summary>
    /// <param name="center">Center position of the polygon</param>
    /// <param name="angle">Angle to draw the poligon towards</param>
    /// <param name="radius">A positive radius of the circle which sircumscribes the polygon</param>
    /// <param name="sidesCount">How many sides to draw. Must be >= 3 (use a line for a 2 sided polygon and a point for a 1 sided poligon)</param>
    /// <param name="normal">Perpendicular of the plane containing the circle</param>
    public static void DrawRegularPoligon(Vector2 center, float angle, float radius, int sidesCount){
        CustomGizmos.DrawRegularPoligon(center, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), radius, sidesCount, Vector3.forward);
    }

    /// <summary>
    /// Draws a regular n-sided poligon (triangle, square, pentagon and so on) given it's center, the direction and length of it's radius,
    /// how many sides to draw and the normal direction to draw the poligon towards.
    /// Can also be used to draw 2D circles by increasing the number of sides to draw to a really high number
    /// </summary>
    /// <param name="center">Center position of the polygon</param>
    /// <param name="endPos">Position of one of the poligon vertices</param>
    /// <param name="sidesCount">How many sides to draw. Must be >= 3 (use a line for a 2 sided polygon and a point for a 1 sided poligon)</param>
    /// <param name="normal">Perpendicular of the plane containing the circle</param>
    public static void DrawRegularPoligon(Vector2 center, Vector2 endPos, int sidesCount){
        CustomGizmos.DrawRegularPoligon(center, endPos.normalized, endPos.magnitude, sidesCount, Vector3.forward);
    }
    
    /// <summary>
    /// Draws a Bézier curve using three points
    /// </summary>
    /// <param name="aPos">The starting position of the curve</param>
    /// <param name="bPos">The control point of the curve</param>
    /// <param name="cPos">The ending position of the curve</param>
    /// <param name="resolution">how many middle lines to draw, used to change resolution, must be >= 0 </param>
    public static void Bezier(Vector2 aPos, Vector2 bPos, Vector2 cPos, int resolution){
        List<Vector3> list = new List<Vector3>();
        list.Add(aPos);
        list.Add(bPos);
        list.Add(cPos);
        CustomGizmos.Bezier(list, resolution);
    }
    
    /// <summary>
    /// Draws a bezier using a list of n-points
    /// </summary>
    /// <param name="points">The list of points to draw, must not be null and must be >= 2 in length</param>
    /// <param name="resolution">How many lines to draw, used to change resolution, must be >= 0 </param>
    public static void Bezier(List<Vector2> points, int resolution){
        List<Vector3> points3D = new List<Vector3>();
        foreach(Vector2 p in points) points3D.Add((Vector3)p);
        CustomGizmos.Bezier(points3D, resolution);
    }
    
    /// <summary>
    /// Draws a Bézier curve using three points
    /// </summary>
    /// <param name="aPos">The starting position of the curve</param>
    /// <param name="bPos">The first control point of the curve</param>
    /// <param name="cPos">The second control point of the curve</param>
    /// <param name="dPos">The ending position of the curve</param>
    /// <param name="resolution">how many middle lines to draw, used to change resolution, must be >= 0 </param>
    public static void CubicBezier(Vector2 aPos, Vector2 bPos, Vector2 cPos, Vector2 dPos, int resolution){
        List<Vector3> list = new List<Vector3>();
        list.Add(aPos);
        list.Add(bPos);
        list.Add(cPos);
        list.Add(dPos);
        CustomGizmos.Bezier(list, resolution);
    }
    
    /// <summary>
    /// Resets the bezier dinamic memory
    /// </summary>
    public static void ResetBezierMemory(){
        CustomGizmos.ResetBezierMemory();
    }
    
    /*
    
    /// <summary>
    /// Draws a bezier curve with a arrow tip inside the curve
    /// </summary>
    /// <param name="points">The list of control points of the curve</param>
    /// <param name="resolution">how many lines to draw</param>
    /// <param name="arrowTipParam">A value between 0 and 1 (included), lerps the arrow at the given percentage of the curve</param>
    /// <param name="arrowTipSize">The size of the arrow tip</param>
    /// <param name="normal">Normal vector of the arrow tip, used to orient the arrow in a custom direction</param>
    public static void BezierWithArrow(List<Vector3> points, int resolution, float arrowTipParam, float arrowTipSize, Vector3 normal){
        List<Vector3> bezier = GeneralBezier(points, resolution);
        float bezierCurveLength = 0;
        for(int i = 0; i < bezier.Count-1; i++){
            bezierCurveLength += (bezier[i+1] - bezier[i]).magnitude;
        }

        arrowTipParam *= (1 - 2f * arrowTipSize / bezierCurveLength);
        int linesCount = bezier.Count - 1;
        int paramLine = (int)(arrowTipParam * linesCount);
        
        float endPosParam = arrowTipParam + 2f * arrowTipSize / bezierCurveLength;
        int endParamLine = (int)(endPosParam * linesCount);
        
        Vector3 arrowStartPos = Vector3.Lerp(bezier[paramLine], bezier[paramLine+1], arrowTipParam * linesCount - paramLine);
        Vector3 arrowEndPos = Vector3.Lerp(bezier[endParamLine], bezier[(endParamLine+1)%bezier.Count], endPosParam * linesCount - endParamLine);
        
        CustomGizmos.DrawArrowTip(arrowStartPos, (arrowEndPos - arrowStartPos).normalized, arrowTipSize, normal - Vector3.Project(normal, (arrowEndPos - arrowStartPos).normalized));
        
        List<Vector3> firstPartList = new List<Vector3>();
        
        for(int i = 0; i <= paramLine; i++){
            Vector3 point = bezier[i];
            firstPartList.Add(point);
        }
        
        firstPartList.Add(arrowStartPos);
        
        for(int i = 0; i < firstPartList.Count - 1; i++){
            Gizmos.DrawLine(firstPartList[i], firstPartList[i+1]);
        }
        
        List<Vector3> secondPartList = new List<Vector3>();
        secondPartList.Add(arrowEndPos);
        for(int i = endParamLine+1; i < bezier.Count; i++){
            Vector3 point = bezier[i];
            secondPartList.Add(bezier[i]);
        }
        
        for(int i = 0; i < secondPartList.Count - 1; i++){
            Gizmos.DrawLine(secondPartList[i], secondPartList[i+1]);
        }
    }
    
    */
    
}
