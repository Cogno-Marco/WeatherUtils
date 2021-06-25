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
    
    /*
    
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
        CustomGizmos.DrawArrowTip(currentTipPos, offset.normalized, tipSize, normal);
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
        List<Vector3> list = new List<Vector3>();
        list.Add(aPos);
        list.Add(bPos);
        list.Add(cPos);
        Bezier(list, resolution);
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
        List<Vector3> list = new List<Vector3>();
        list.Add(aPos);
        list.Add(bPos);
        list.Add(cPos);
        list.Add(dPos);
        Bezier(list, resolution);
    }
    
    /// <summary>
    /// Draws a bezier using a list of n-points
    /// </summary>
    /// <param name="points">The list of points to draw, must not be null and must be >= 2 in length</param>
    /// <param name="resolution">How many lines to draw, used to change resolution, must be >= 0 </param>
    public static void Bezier(List<Vector3> points, int resolution){
        if(points.Count < 2) throw new System.ArgumentException("there must be at least 2 points to work!");
        List<Vector3> toDraw = GeneralBezier(points, resolution);
        for(int i = 0; i < toDraw.Count-1; i++){
            Gizmos.DrawLine(toDraw[i], toDraw[i+1]);
        }
    }
    
    private static Dictionary<string, List<Vector3>> bezierMap = new Dictionary<string, List<Vector3>>();
    
    /// <summary>
    /// Returns a list with the points of the bezier curve
    /// </summary>
    /// <param name="points">The list of points to draw, must not be null and must be >= 2 in length</param>
    /// <param name="resolution">How many lines to draw, used to change resolution, must be >= 0 </param>
    /// <returns>Returns a list of points: the points interpolated in the bezier curve</returns>
    private static List<Vector3> GeneralBezier(List<Vector3> points, int resolution){
        if(points.Count < 2) throw new System.ArgumentException("there must be at least 2 points to work!");
        //base case
        string pointsID = "[";
        for(int i = 0; i < points.Count; i++){
            pointsID += points[i].ToString() + ", ";
        }
        pointsID += resolution + "]";
        
        if(bezierMap.ContainsKey(pointsID))
            return bezierMap[pointsID];
        
        if(points.Count == 2){
            List<Vector3> linearBez = LinearBezier(points[0], points[1], resolution);
            return linearBez;
        }
        //recursive case
        //1. create 2 lists, the first with the point from 0 to n-1, the second with the points from 1 to n
        List<Vector3> list1 = new List<Vector3>();
        List<Vector3> list2 = new List<Vector3>();
        string list1ID = "[";
        string list2ID = "[";
        for(int i = 0; i < points.Count - 1; i++){
            list1.Add(points[i]);
            list2.Add(points[i+1]);
            list1ID += points[i].ToString() + ", ";
            list2ID += points[i+1].ToString() + ", ";
        }
        list1ID += resolution + "]";
        list2ID += resolution + "]";
        //recursive call itself to get intermediate points
        List<Vector3> interm1;
        List<Vector3> interm2;
        
        
        if(bezierMap.ContainsKey(list1ID)){
            interm1 = bezierMap[list1ID];
        }
        else{
            List<Vector3> bez = GeneralBezier(list1, resolution);
            bezierMap.Add(list1ID, bez);
            interm1 = bez;
        }
        
        if(bezierMap.ContainsKey(list2ID)){
            interm2 = bezierMap[list2ID];
        }
        else{
            List<Vector3> bez = GeneralBezier(list2, resolution);
            bezierMap.Add(list2ID, bez);
            interm2 = bez;
        }
        
        //final lerp these 2 curves
        float step = 1f / resolution;
        List<Vector3> output = new List<Vector3>();
        for(int i = 0; i < interm1.Count; i++){
            Vector3 final = Vector3.Lerp(interm1[i], interm2[i], step * i);
            output.Add(final);
        }
        return output;
    }
    
    /// <summary>
    /// Resets the bezier dinamic memory
    /// </summary>
    public static void ResetBezierMemory(){
        bezierMap.Clear();
    }
    
    /// <summary>
    /// Base case: linear implementation of a bezier curve, basically a lerp
    /// </summary>
    /// <param name="aPos">The starting position of the bezier curve</param>
    /// <param name="bPos">The ending position of the bezier curve</param>
    /// <param name="resolution">how many lines to draw</param>
    /// <returns>Returns a list of points: the points interpolated in the bezier curve</returns>
    private static List<Vector3> LinearBezier(Vector3 aPos, Vector3 bPos, int resolution){
        List<Vector3> output = new List<Vector3>();
        for(int i = 0; i <= resolution; i++){
            Vector3 final = Vector3.Lerp(aPos, bPos, (float)(i) / resolution);
            output.Add(final);
        }
        return output;
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
    */
    
}
