using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameMenu : MonoBehaviour
{

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClickResume()
    {
        animator.SetTrigger("Play");
    }

    public void OnFinishFadeOut()
    {
        GameController.Instance.UnpauseGame();
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
