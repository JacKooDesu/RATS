using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject pausePanel;

    public void Pause()
    {
        Time.timeScale = 0;
        Animator ani = pausePanel.GetComponentInChildren<Animator>();
        ani.SetTrigger("Open");

        StartCoroutine(Pausing());
    }

    IEnumerator Pausing()
    {
        bool breaker = false;
        UnityEngine.UI.Button b = pausePanel.transform.GetChild(0).Find("Continue").GetComponent<UnityEngine.UI.Button>();
        UnityEngine.Events.UnityAction action = null;
        action = () =>
        {
            breaker = true;
        };
        b.onClick.AddListener(action);

        while (!breaker)
        {
            yield return null;
        }

        Time.timeScale = 1;
        pausePanel.GetComponentInChildren<Animator>().SetTrigger("Close");
        b.onClick.RemoveAllListeners();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0)
            Pause();
    }
}
