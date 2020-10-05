using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuEvents : MonoBehaviour
{
    [SerializeField] private GameObject loading;

    public void OnClickStart()
    {
        loading.GetComponent<Animator>().Play("FadeIn");
    }

    public void NextMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
