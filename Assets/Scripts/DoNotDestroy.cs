using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void Awake ()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if (musicObj.Length > 1 || SceneManager.GetActiveScene().buildIndex > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
