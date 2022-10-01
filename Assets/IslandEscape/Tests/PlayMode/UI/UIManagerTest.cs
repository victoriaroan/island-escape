using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using IslandEscape.UI;

public class UIManagerTest : BaseTest
{
    /// <summary>
    /// Verifies that <c>UIManager.UpdateClock()</c> sets the time elapsed since game start with
    /// the correct format when the game is unpaused.
    /// </summary>
    [UnityTest]
    public IEnumerator UpdateClockSetsClockTextWhenGameUnpaused()
    {
        UIManager manager = GameObject.Find("UI").GetComponent<UIManager>();

        yield return new WaitForSeconds(1f);

        Assert.That(manager.clock.text, Is.EqualTo("0m 1s"));
    }

    /// <summary>
    /// Verifies that <c>UIManager.UpdateClock()</c> does not change clock text when the game is
    /// paused.
    /// </summary>
    [UnityTest]
    public IEnumerator UpdateClockDoesntSetClockTextWhenGamePaused()
    {
        UIManager manager = GameObject.Find("UI").GetComponent<UIManager>();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameManager.gamePaused = true;
        yield return new WaitForSeconds(1f);

        Assert.That(manager.clock.text, Is.EqualTo("0m 0s"));
    }

    // TODO: add test to make sure 60s is displayed as 1m 0s
}
