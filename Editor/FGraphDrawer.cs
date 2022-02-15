#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
/// <summary>
/// This class contain custom drawer for ReadOnly attribute.
/// </summary>
[CustomPropertyDrawer(typeof(FGraphAttribute))]
public class FGraphDrawer : PropertyDrawer
{
    private AnimationCurve curve = new AnimationCurve();
    
    /// <summary>
    /// Unity method for drawing GUI in Editor
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="property">Property.</param>
    /// <param name="label">Label.</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //graph property only works for floats and ints
        if(property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer){
            EditorGUI.LabelField(position, label.text, "Use FGraph with float or int.");
            return;
        }
        
        //edit curve only while the game is playing
        if(EditorApplication.isPlaying && !EditorApplication.isPaused){
            //find component
            CurveGrapher component = Selection.activeGameObject.GetComponent<CurveGrapher>();
            if(component == null){
                component = Selection.activeGameObject.AddComponent<CurveGrapher>();
            }
            
            if(!component.IsTracking(property))
                component.FTrackProperty(property);
            
            curve = component.GetCurve(property);  
        }
        
        if(EditorApplication.isPlaying || EditorApplication.isPaused){
            //draw curve property
            EditorGUI.CurveField(position, label.text, curve);
            return;
        }
        
        //if the game is not either playing or paused, the curve shouldn't function
        
        // Saving previous GUI enabled value
        var previousGUIState = GUI.enabled;
        // Disabling edit for property
        GUI.enabled = false;
        // Drawing Property
        EditorGUI.CurveField(position, label.text, curve);
        // Setting old GUI enabled value
        GUI.enabled = previousGUIState;
    }
}
#endif