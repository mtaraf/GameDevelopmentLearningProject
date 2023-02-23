using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public void StartNewGame()
    {
        MainManager.instance.isNewGame = true;
        Debug.Log("Start Game Invoked");
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        if (MainManager.instance != null)
        {
            MainManager.instance.isNewGame = false;
            MainManager.instance.LoadPlayerData();
        }
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        if (MainManager.instance != null)
        {
            MainManager.instance.SavePlayerData();
        }
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
    }
}
