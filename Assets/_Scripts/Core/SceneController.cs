using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController
{
    public static GameManager.kind GameKind;

    public static void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public static void LoadGameplay(GameManager.kind gameplayKind)
    {
        GameKind = gameplayKind;
        SceneManager.LoadScene("gameplay");
    }

}
