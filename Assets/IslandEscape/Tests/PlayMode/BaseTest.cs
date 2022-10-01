using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public abstract class BaseTest
{
    public bool sceneLoaded = false;

    [OneTimeSetUp]
    public void SetUpSuite()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    [UnitySetUp]
    public IEnumerator SetUpScene()
    {
        // Debug.Log("loading scene");
        // I _think_ this will reload the scene and unload the previously loaded one? EDIT: seems like it after listening to SceneManager.sceneUnloaded.
        // Not positive. Maybe need LoadSceneMode.Additive and something like in comments here: https://answers.unity.com/questions/1115864/why-is-it-not-supported-to-unload-the-firstly-load.html
        SceneManager.LoadScene("Island", LoadSceneMode.Single);

        // wait for scene to fully load
        yield return new WaitWhile(() => sceneLoaded == false);
        // Debug.Log("scene loaded");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        // Debug.Log("Scene unloaded");
        sceneLoaded = false;
    }

    [UnityTearDown]
    public IEnumerator TearDownScene()
    {
        // Debug.Log("unloading scene");
        // // unload the scene
        // AsyncOperation unload = SceneManager.UnloadSceneAsync("Island");
        // unload.completed += OnSceneUnloaded;

        // // wait until fully unloaded
        // yield return new WaitWhile(() => sceneLoaded == true);
        // Debug.Log("scene unloaded");
        yield return null;
    }
}
