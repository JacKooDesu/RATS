using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscPanel : MonoBehaviour
{
    public Button continueBtn, mainMenuBtn, leaveBtn;

    private void OnEnable()
    {
        mainMenuBtn.onClick.RemoveAllListeners();
        leaveBtn.onClick.RemoveAllListeners();

        mainMenuBtn.onClick.AddListener(() => SceneUtil.Singleton.ChangeScene("Menu"));
        leaveBtn.onClick.AddListener(() => { print("QUIT"); Application.Quit(); });
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Animator ani = GetComponentInChildren<Animator>();
        ani.SetTrigger("Open");

        StartCoroutine(Pausing());
    }

    IEnumerator Pausing()
    {
        bool breaker = false;
        UnityEngine.UI.Button b = continueBtn;
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
        GetComponentInChildren<Animator>().SetTrigger("Close");
        b.onClick.RemoveAllListeners();
    }
}
