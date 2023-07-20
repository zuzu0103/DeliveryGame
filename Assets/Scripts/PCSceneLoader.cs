using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PCSceneLoader : MonoBehaviour
{

    public int mainSceneIndex;

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Qutting Game");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneIndex, LoadSceneMode.Additive);
    }
}
