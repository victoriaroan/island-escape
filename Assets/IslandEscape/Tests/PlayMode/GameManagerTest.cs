using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTest : BaseTest
{
    /// <summary>
    /// Tests that <c>GameManager.StartGame()</c> unpauses the game.
    /// </summary>
    [UnityTest]
    public IEnumerator StartGameUnpausesGame()
    {
        // call StartTime (this is done automatically on Awake right now)
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // manager.StartGame();

        yield return null;

        // verify game is unpaused
        Assert.False(manager.gamePaused);
    }

    /// <summary>
    /// Tests that <c>GameManager.StartGame()</c> sets the game's start time.
    /// </summary>
    [UnityTest]
    public IEnumerator StartGameSetsStartTime()
    {
        // call StartTime (this is done automatically on Awake right now)
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // manager.StartGame();

        yield return null;

        // verify start time is set
        Assert.That(manager.startTime, Is.GreaterThan(0f));
    }
}
