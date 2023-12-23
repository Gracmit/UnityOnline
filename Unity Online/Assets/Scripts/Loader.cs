using System;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scene _targetScene;

    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene
    }
    public static void Load(Scene targetScene)
    {
        _targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadCallback()
    {
        SceneManager.LoadScene(_targetScene.ToString());
    }
}
