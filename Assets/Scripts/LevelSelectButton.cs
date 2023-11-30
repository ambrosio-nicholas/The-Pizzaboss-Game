using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private string levelName;
    private Animator anim;

    void Start()
    {
        anim = GameObject.FindWithTag("Transition").GetComponent<Animator>();
    }

    public void changeScene()
    {
        anim.SetTrigger("Start");
        Invoke("newScene", 1);
    }

    private void newScene()
    {
        SceneManager.LoadScene(levelName);
    }
}
