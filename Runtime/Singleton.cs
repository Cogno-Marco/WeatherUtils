using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T instance;
    public static T GetInstance() { return instance; }
    private void SingletonContructor(){
        //make this singleton spawn an object if not present in the scene
        if (instance != null && instance != this as T)
            Destroy(this.gameObject);
        else
            instance = this as T;
    }
    
    public virtual void Awake(){
        SingletonContructor();
    }
}