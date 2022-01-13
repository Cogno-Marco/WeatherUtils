#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;


public class CurveGrapher : MonoBehaviour
{
    
    private List<CurveData> trackedProperties = new List<CurveData>();
    
    void LateUpdate()
    {
        foreach(CurveData data in trackedProperties){
            if(data.isFGraph) continue;
            
            UpdateGraph(data);
        }
    }
    
    private void FixedUpdate() {
        foreach(CurveData data in trackedProperties){
            if(!data.isFGraph) continue;
            
            UpdateGraph(data);
        }
    }
    
    void UpdateGraph(CurveData data){
        var targetObject = data.unityObject;
        System.Reflection.FieldInfo field = targetObject.GetType().GetField(data.propertyName);

        if (field != null)
        {
            var value = field.GetValue(targetObject);
            data.curve.AddKey(Time.time, (float)value);
            
            //set last 3 keys tangents to make them linear
            if(data.curve.keys.Length > 3){
                //get keys
                Keyframe[] keys = data.curve.keys;
                Keyframe finalKey = keys[keys.Length - 1];
                Keyframe lastKey = keys[keys.Length - 2];
                Keyframe previousKey = keys[keys.Length - 3];
                
                //edit tangents from third to second
                float tangent = (lastKey.value - previousKey.value) / (lastKey.time - previousKey.time);
                previousKey.outTangent = tangent;
                lastKey.inTangent = tangent;
                
                //edit tangents from second to first
                tangent = (finalKey.value - lastKey.value) / (finalKey.time - lastKey.time);
                lastKey.outTangent = tangent;
                finalKey.inTangent = tangent;
                
                //set keys
                keys[keys.Length - 1] = finalKey;
                keys[keys.Length - 2] = lastKey;
                keys[keys.Length - 3] = previousKey;
                data.curve.keys = keys;
            }
        }
    }
    
    public AnimationCurve GetCurve(SerializedProperty property){
        foreach(CurveData data in trackedProperties){
            if(data.unityObject.Equals(property.serializedObject.targetObject) && data.propertyName.Equals(property.name)){
                return data.curve;
            }
        }
        return new AnimationCurve();
    }
    
    public bool IsTracking(SerializedProperty property){
        foreach(CurveData data in trackedProperties){
            if(data.unityObject.Equals(property.serializedObject.targetObject) && data.propertyName.Equals(property.name)){
                return true;
            }
        }
        return false;
    }
    
    public void TrackProperty(SerializedProperty property){
        CurveData data = new CurveData();
        data.property = property;
        data.unityObject = data.property.serializedObject.targetObject;
        data.propertyName = property.name;
        data.curve = new AnimationCurve();
        data.isFGraph = false;
        trackedProperties.Add(data);
    }
    
    public void FTrackProperty(SerializedProperty property){
        CurveData data = new CurveData();
        data.property = property;
        data.unityObject = data.property.serializedObject.targetObject;
        data.propertyName = property.name;
        data.curve = new AnimationCurve();
        data.isFGraph = true;
        trackedProperties.Add(data);
    }
}

[SerializeField]
public struct CurveData
{
    public SerializedProperty property;
    public UnityEngine.Object unityObject;
    public string propertyName;
    public AnimationCurve curve;
    public bool isFGraph;
}

#endif
