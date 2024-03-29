﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Profiling;

public class CustomGizmos
{
    /// <summary>
    /// Draws a gizmo arrow at startPos pointing at endPos
    /// </summary>
    /// <param name="startPos">The point where the arrow starts from</param>
    /// <param name="endPos">The point where the arrow points to</param>
    /// <param name="tipSize">The size of the arrow head</param>
    /// <param name="normal">The normal vector of the arrow tip, used to orient the arrow tip in a custom direction</oaram>
    public static void DrawArrow(Vector3 startPos, Vector3 endPos, float tipSize, Vector3 normal){
        Vector3 line = endPos - startPos;
        Vector3 arrowDir = line.normalized;
        float arrowLength = line.magnitude - tipSize;
        Vector3 lineEndPos = startPos + arrowDir * arrowLength;
        Gizmos.DrawLine(lineEndPos, startPos);
        DrawArrowTip(lineEndPos, arrowDir, tipSize, normal);
    }
    
    /// <summary>
    /// Draws an arrow tip at position, pointing in a direction.
    /// N.B. due to artistic decision the there's some white space between the position of the tip and the actual tip,
    /// also the angle of the tip is 90° instead of 60°, so this is not an equilateral triangle.
    /// </summary>
    /// <param name="position">The position to place the arrow tip</param>
    /// <param name="direction">The direction of the tip</param>
    /// <param name="size">The size of the arrow tip</param>
    /// <param name="normal">The normal vector of the arrow tip, used to orient the arrow tip</param>
    public static void DrawArrowTip(Vector3 position, Vector3 direction, float size, Vector3 normal){
        Vector3 dir = direction.normalized;
        Vector3 tipPos = position + dir * size;
        Vector3 sideDir1 = Quaternion.AngleAxis(+45, normal) * -dir;
        Vector3 sideDir2 = Quaternion.AngleAxis(-45, normal) * -dir;
        float lengthSize = size * Mathf.Sqrt(5) / 2;    //s^2 + s^2/4 = lengthSize^2
        Gizmos.DrawLine(tipPos, tipPos + sideDir1 * lengthSize);
        Gizmos.DrawLine(tipPos, tipPos + sideDir2 * lengthSize);
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
    public static void DrawLine(Vector3 startPos, Vector3 endPos, float spacing){
        DrawLine(startPos, endPos, spacing, spacing);
    }

    /// <summary>
    /// Draws a line with empty space at the end and at the start, not simmetric.
    /// </summary>
    /// <param name="startPos">The starting pos of line</param>
    /// <param name="endPos">The ending pos of the line</param>
    /// <param name="startSpacing">How much empty space to have at the start of the line</param>
    /// <param name="endSpacing">How much empty space to have at the end of the line</param>    
    public static void DrawLine(Vector3 startPos, Vector3 endPos, float startSpacing, float endSpacing){
        Vector3 lineDir = (endPos - startPos).normalized;
        Gizmos.DrawLine(startPos + lineDir * startSpacing, endPos - lineDir * endSpacing);
    }
    
    /// <summary>
    /// Draws a H shaped gizmo
    /// </summary>
    /// <param name="startPos">Starting position of the H</param>
    /// <param name="endPos">Ending position of the H</param>
    /// <param name="lineSize">How big are the H sides</param>
    /// <param name="normal">The normal vector of the H sides, used to orient the H sides in a custom direction</oaram>
    public static void DrawH(Vector3 startPos, Vector3 endPos, float lineSize, Vector3 normal){
        Vector3 lineDir = (endPos - startPos).normalized;
        
        Vector3 tipStartA = startPos + Quaternion.AngleAxis(-90, normal) * lineDir * -lineSize;
        Vector3 tipEndA = startPos + Quaternion.AngleAxis(+90, normal) * lineDir * -lineSize;
        Vector3 tipStartB = endPos + Quaternion.AngleAxis(-90, normal) * lineDir * -lineSize;
        Vector3 tipEndB = endPos + Quaternion.AngleAxis(+90, normal) * lineDir * -lineSize;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawLine(tipStartA, tipEndA);
        Gizmos.DrawLine(tipStartB, tipEndB);
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
    /// <param name="normal">Normal vector of the arrow tip, used to orient the arrow in a custom direction</param>
    public static void DrawLineArrow(Vector3 startPos, Vector3 endPos, float tipSize, float placementPercentage, Vector3 normal){
        float lineLength = (endPos - startPos).magnitude;
        placementPercentage *= (1 - tipSize / lineLength);
        Vector3 offset = endPos - startPos;
        Vector3 tipMaxPos = startPos + offset.normalized * (offset.magnitude - tipSize);
        Vector3 currentTipPos = Vector3.Lerp(startPos, tipMaxPos, placementPercentage);
        DrawArrowTip(currentTipPos, offset.normalized, tipSize, normal);
        Gizmos.DrawLine(startPos, currentTipPos);
        DrawLine(currentTipPos + offset.normalized * tipSize, endPos, tipSize / 2, 0);
    }
    
    /// <summary>
    /// Draws a rectangle with lines crossing it
    /// </summary>
    /// <param name="center">Center position of the rectangle</param>
    /// <param name="widthDir">Direction of the width of the rectangle</param>
    /// <param name="heightDir">Direction of the height of the rectangle</param>
    /// <param name="halfSides">Half-sizes of the edges</param>
    /// <param name="numbOfLines">How many lines cross the rectangle, 0 simply draws a square</param>
    public static void DrawCrossedRect(Vector3 center, Vector3 widthDir, Vector3 heightDir, Vector2 halfSides, int numbOfLines){
        widthDir = widthDir.normalized;
        heightDir = heightDir.normalized;
        Vector3 topLeftCorner = center - widthDir * halfSides.x + heightDir * halfSides.y;
        Vector3 botRightCorner = center + widthDir * halfSides.x - heightDir * halfSides.y;
        
        //draws the rectangle
        Gizmos.DrawLine(topLeftCorner,  topLeftCorner  + widthDir  * halfSides.x * 2);
        Gizmos.DrawLine(topLeftCorner,  topLeftCorner  - heightDir * halfSides.y * 2);
        Gizmos.DrawLine(botRightCorner, botRightCorner - widthDir  * halfSides.x * 2);
        Gizmos.DrawLine(botRightCorner, botRightCorner + heightDir * halfSides.y * 2);
        
        Vector3 normal = Vector3.Cross(widthDir, heightDir);
        Matrix4x4 transformMatrix = Matrix4x4.identity;
        transformMatrix.SetTRS(center, Quaternion.LookRotation(heightDir, normal), Vector3.one);
        Matrix4x4 inverseMatrix = transformMatrix.inverse;
        
        Vector3 cornerTL = transformMatrix.MultiplyPoint(topLeftCorner);
        Vector3 cornerBR = transformMatrix.MultiplyPoint(botRightCorner);
        Vector3 newHeightDir = transformMatrix.MultiplyVector(heightDir);
        Vector3 newWidthDir = transformMatrix.MultiplyVector(widthDir);
        
        float step = 1f / (numbOfLines+1);
        for(float i = step; i < 1; i += step){
            Vector3 worldLineStart = Vector3.Lerp(topLeftCorner, botRightCorner, i);
            Vector3 lineStart = transformMatrix.MultiplyPoint(worldLineStart);

            //check which side the ray collides with first (between left and bottom)
            Vector3 rayDir1 = transformMatrix.MultiplyVector((Quaternion.AngleAxis(45, normal) * widthDir).normalized);
            
            Vector3 position1 = GetFirst(cornerTL, newWidthDir, lineStart, rayDir1, cornerBR, newHeightDir);
            Vector3 position2 = GetFirst(cornerTL, newHeightDir, lineStart, -rayDir1, cornerBR, newWidthDir);
            
            Gizmos.DrawLine(worldLineStart, inverseMatrix.MultiplyPoint(position1));
            Gizmos.DrawLine(worldLineStart, inverseMatrix.MultiplyPoint(position2));
        }
    }
    
    /// <summary>
    /// Returns the point of intersection between 2 rays
    /// </summary>
    /// <param name="aPos">The start of the first ray</param>
    /// <param name="aDir">The direction of the first ray</param>
    /// <param name="bPos">The start of the second ray</param>
    /// <param name="bDir">The direction of the second ray</param>
    private static Vector3 GetIntersection(Vector3 aPos, Vector3 aDir, Vector3 bPos, Vector3 bDir){
        return bPos + bDir * GetIntersectionParam(aPos, aDir, bPos, bDir);
    }
    
    /// <summary>
    /// Returns the intersection parameter u of the ray of the intersaction of 2 given rays
    /// </summary>
    /// <param name="aPos">The starting position of the first ray</param>
    /// <param name="aDir">The direction of the first ray</param>
    /// <param name="bPos">The starting position of the second ray</param>
    /// <param name="bDir">The direction of the second ray</param>
    private static float GetIntersectionParam(Vector3 aPos, Vector3 aDir, Vector3 bPos, Vector3 bDir){
        float dx = aPos.x - bPos.x;
        float dz = aPos.z - bPos.z;
        float det = aDir.x * bDir.z - aDir.z * bDir.x;
        float u = (dz * aDir.x - dx * aDir.z) / det;
        return u;
    }
    
    /// <summary>
    /// Returns the first point of intersection between 3 rays
    /// </summary>
    /// <param name="aPos">The starting position of the first ray</param>
    /// <param name="aDir">The direction of the first ray</param>
    /// <param name="bPos">The starting position of the second ray</param>
    /// <param name="bDir">The direction of the second ray</param>
    /// <param name="cPos">The starting position of the third ray</param>
    /// <param name="cDir">The direction of the third ray</param>
    private static Vector3 GetFirst(Vector3 aPos, Vector3 aDir, Vector3 bPos, Vector3 bDir, Vector3 cPos, Vector3 cDir){
        return bPos + bDir * Mathf.Min(
            GetIntersectionParam(aPos, aDir, bPos, bDir),
            GetIntersectionParam(cPos, cDir, bPos, bDir)
        );
    }
    
    /// <summary>
    /// Draws a Bézier curve using three points
    /// </summary>
    /// <param name="aPos">The starting position of the curve</param>
    /// <param name="bPos">The control point of the curve</param>
    /// <param name="cPos">The ending position of the curve</param>
    /// <param name="resolution">how many middle lines to draw, used to change resolution, must be >= 0 </param>
    public static void Bezier(Vector3 aPos, Vector3 bPos, Vector3 cPos, int resolution){
        Bezier(new Vector3[]{aPos, bPos, cPos}, resolution);
    }
    
    /// <summary>
    /// Draws a Bézier curve using three points
    /// </summary>
    /// <param name="aPos">The starting position of the curve</param>
    /// <param name="bPos">The first control point of the curve</param>
    /// <param name="cPos">The second control point of the curve</param>
    /// <param name="dPos">The ending position of the curve</param>
    /// <param name="resolution">how many middle lines to draw, used to change resolution, must be >= 0 </param>
    public static void CubicBezier(Vector3 aPos, Vector3 bPos, Vector3 cPos, Vector3 dPos, int resolution){
        Bezier(new Vector3[]{aPos, bPos, cPos, dPos}, resolution + 1);
    }
    
    /// <summary>
    /// Draws a bezier using a list of n-points
    /// </summary>
    /// <param name="points">The list of points to draw, must not be null and must be >= 2 in length</param>
    /// <param name="resolution">How many lines to draw, used to change resolution, must be >= 0 </param>
    public static void Bezier(Vector3[] points, int resolution){
        if(points.Length < 2) throw new System.ArgumentException("there must be at least 2 points to work!");
        Profiler.BeginSample("Bezier Calculation");
        Vector3[] toDraw = BezierCurves.GeneralBezier(points, resolution + 1);
        Profiler.EndSample();
        Profiler.BeginSample("Curve drawing");
        for(int i = 0; i < toDraw.Length - 1; i++){
            Gizmos.DrawLine(toDraw[i], toDraw[i + 1]);
        }
        Profiler.EndSample();
    }
    
    /// <summary>
    /// Resets the bezier dinamic memory
    /// </summary>
    public static void ResetBezierMemory(){
        BezierCurves.CleanMemory();
    }
    
    /// <summary>
    /// Draws a bezier curve with a arrow tip inside the curve
    /// </summary>
    /// <param name="points">The list of control points of the curve</param>
    /// <param name="resolution">how many lines to draw</param>
    /// <param name="arrowTipParam">A value between 0 and 1 (included), lerps the arrow at the given percentage of the curve</param>
    /// <param name="arrowTipSize">The size of the arrow tip</param>
    /// <param name="normal">Normal vector of the arrow tip, used to orient the arrow in a custom direction</param>
    public static void BezierWithArrow(List<Vector3> points, int resolution, float arrowTipParam, float arrowTipSize, Vector3 normal){
        Vector3[] bezier = BezierCurves.GeneralBezier(points.ToArray(), resolution);
        float bezierCurveLength = 0;
        for(int i = 0; i < bezier.Length - 1; i++){
            bezierCurveLength += (bezier[i + 1] - bezier[i]).magnitude;
        }

        arrowTipParam *= (1 - 2f * arrowTipSize / bezierCurveLength);
        int linesCount = bezier.Length - 1;
        int paramLine = (int)(arrowTipParam * linesCount);
        
        float endPosParam = arrowTipParam + 2f * arrowTipSize / bezierCurveLength;
        int endParamLine = (int)(endPosParam * linesCount);
        
        Vector3 arrowStartPos = Vector3.Lerp(bezier[paramLine], bezier[paramLine+1], arrowTipParam * linesCount - paramLine);
        Vector3 arrowEndPos = Vector3.Lerp(bezier[endParamLine], bezier[(endParamLine+1) % bezier.Length], endPosParam * linesCount - endParamLine);
        
        DrawArrowTip(arrowStartPos, (arrowEndPos - arrowStartPos).normalized, arrowTipSize, normal - Vector3.Project(normal, (arrowEndPos - arrowStartPos).normalized));
        
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
        for(int i = endParamLine + 1; i < bezier.Length; i++){
            Vector3 point = bezier[i];
            secondPartList.Add(bezier[i]);
        }
        
        for(int i = 0; i < secondPartList.Count - 1; i++){
            Gizmos.DrawLine(secondPartList[i], secondPartList[i + 1]);
        }
    }

    /// <summary>
    /// Draws a regular poligon (triangle, square, pentagon and so on) given it's center, the direction and length of it's radius,
    /// how many sides to draw and the normal direction to draw the poligon towards.
    /// Can also be used to draw 2D circles by increasing the number of sides to draw
    /// </summary>
    /// <param name="center">Center position of the polygon</param>
    /// <param name="direction">Direction towards the first point of the polygon</param>
    /// <param name="radius">A positive radius of the circle which sircumscribes the polygon</param>
    /// <param name="sidesCount">How many sides to draw. Must be >= 3 (use a line for a 2 sided polygon and a point for a 1 sided poligon)</param>
    /// <param name="normal">Perpendicular of the plane containing the circle</param>
    public static void DrawRegularPoligon(Vector3 center, Vector3 direction, float radius, int sidesCount, Vector3 normal){
        if(sidesCount < 3){
            Debug.LogWarning("Attempted to draw a polygon with less than 3 sides");
            return;
        }
        Vector3 rightDir = direction.normalized;
        Vector3 upDir = Vector3.Cross(rightDir, normal.normalized).normalized;
        
        for(int i = 0; i < sidesCount; i++){
            float angleStep = 2f * Mathf.PI / sidesCount;
            Vector3 p1 = center + radius * rightDir * Mathf.Cos(angleStep * i)       + radius * upDir * Mathf.Sin(angleStep * i);
            Vector3 p2 = center + radius * rightDir * Mathf.Cos(angleStep * (i + 1)) + radius * upDir * Mathf.Sin(angleStep * (i + 1));
            Gizmos.DrawLine(p1, p2);
        }
    }

    /// <summary>
    /// Draws an ellipse in 3d space given enough information
    /// </summary>
    /// <param name="center">The center position of the ellipse</param>
    /// <param name="rightDir">The direction of one of the semiaxis</param>
    /// <param name="upDir">Direction of the other semiaxis</param>
    /// <param name="axisLenghts">Length of both semiaxis, if equal it will draw a circle</param>
    /// <param name="sidesCount">How many segments to draw the ellipse in</param>
    public static void DrawEllipse(Vector3 center, Vector3 rightDir, Vector3 upDir, Vector2 axisLenghts, int sidesCount){
        float a = axisLenghts.x;
        float b = axisLenghts.y;
        
        for(int i = 0; i < sidesCount; i++){
            float angleStep = 2f * Mathf.PI / sidesCount;
            Vector3 p1 = center + a * rightDir * Mathf.Cos(angleStep * i)       + b * upDir * Mathf.Sin(angleStep * i);
            Vector3 p2 = center + a * rightDir * Mathf.Cos(angleStep * (i + 1)) + b * upDir * Mathf.Sin(angleStep * (i + 1));
            Gizmos.DrawLine(p1, p2);
        }
    }

}
