using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeSceneWithButton : MonoBehaviour
{
    
    [SerializeField] private Transform player; //drag player reference onto here
    private Vector3 targetPosition = new Vector3(-17, -7, 0); //here you store the position you want to teleport your player to


    public void LoadScene(string screneName)
    {
        SceneManager.LoadScene(screneName);
    }
    private void OnEnable()
    {

        SceneManager.sceneLoaded += SceneLoaded; //You add your method to the delegate

    }

    /*private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
    */
    //After adding this method to the delegate, this method will be called every time
    //that a new scene is loaded. You can then compare the scene loaded to your desired
    //scenes and do actions according to the scene loaded.
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("love");
        if (scene.name == "FactoryFix") //use your desired check here to compare your scene
        {

            if (player == null)
                player = FindObjectOfType<JacDev.Fix.PlayerMovement>().transform;

            player.position = targetPosition;
            SceneManager.sceneLoaded -= SceneLoaded;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
     
    }
}
