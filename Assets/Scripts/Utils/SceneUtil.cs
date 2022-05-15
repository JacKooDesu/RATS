using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil : MonoBehaviour
{
    static SceneUtil singleton = null;
    public static SceneUtil Singleton
    {
        get
        {
            singleton = FindObjectOfType(typeof(SceneUtil)) as SceneUtil;

            if (singleton == null)
            {
                GameObject g = new GameObject("SceneUtil");
                singleton = g.AddComponent<SceneUtil>();
            }

            return singleton;
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
