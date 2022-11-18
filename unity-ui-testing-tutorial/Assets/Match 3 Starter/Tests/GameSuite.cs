using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tests
{
    public class GameSuite : InputTestFixture
    {
        Mouse mouse;

        public void ClickUI(GameObject uiElement)
        {
            Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position);
            Set(mouse.position, screenPos);
            Click(mouse.leftButton);
        }

        public override void Setup()
        {
            base.Setup();
            SceneManager.LoadScene("Match 3 Starter/Scenes/Game");
            mouse = InputSystem.AddDevice<Mouse>();
        }

        [UnityTest]
        public IEnumerator TestMoveCounterDecrease()
        {
            GUIManager.instance.MoveCounter = 10;
            GameObject moveCounterTxt = GameObject.Find("MoveCounterImage/MoveCounterTxt");
            string movesLeft = moveCounterTxt.GetComponent<Text>().text;
            Assert.That(movesLeft, Is.EqualTo("10"));
            yield return null;

            ClickUI(BoardManager.instance.tiles[0, 0]);
            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[1, 0]);
            yield return new WaitForSeconds(2f);

            movesLeft = moveCounterTxt.GetComponent<Text>().text;
            Assert.That(movesLeft, Is.EqualTo("9"));
        }

        [UnityTest]
        public IEnumerator TestGameOverPlayAgain()
        {
            GUIManager.instance.MoveCounter = 3;
            BoardManager oldBoard = BoardManager.instance;

            for (int i = 0; i < 3; i++)
            {
                ClickUI(BoardManager.instance.tiles[0, 0]);
                yield return new WaitForSeconds(1f);
                ClickUI(BoardManager.instance.tiles[1, 0]);
                yield return new WaitForSeconds(1f);
            }

            GameObject playButton = GameObject.Find("GameOverPanel/PlayButton");
            ClickUI(playButton);

            yield return new WaitForSeconds(2f);

            string sceneName = SceneManager.GetActiveScene().name;
            Assert.That(sceneName, Is.EqualTo("Game"));
            Assert.That(GUIManager.instance.gameOverPanel.activeSelf, Is.EqualTo(false));
            Assert.That(BoardManager.instance, Is.Not.EqualTo(oldBoard));
        }
        [UnityTest]
        public IEnumerator TestGameOverBackToMenu()
        {
            GUIManager.instance.MoveCounter = 3;
            BoardManager oldBoard = BoardManager.instance;

            for (int i = 0; i < 3; i++)
            {
                ClickUI(BoardManager.instance.tiles[0, 0]);
                yield return new WaitForSeconds(1f);
                ClickUI(BoardManager.instance.tiles[1, 0]);
                yield return new WaitForSeconds(1f);
            }

            GameObject playButton = GameObject.Find("GameOverPanel/MenuButton");
            ClickUI(playButton);

            yield return new WaitForSeconds(2f);

            string sceneName = SceneManager.GetActiveScene().name;
            Assert.That(sceneName, Is.EqualTo("Menu"));
        }
        [UnityTest]
        public IEnumerator TestScoreSimpleMatch()
        {
            GameObject scoreTxt = GameObject.Find("ScorePanel/ScoreTxt");
            string[,] grid = new string[3, 3]{
        {"Y", "B", "M"},
        {"Y", "R", "G"},
        {"P", "Y", "M"}
    }; // Note that the grid's x and y dimensions are inverted here.
            BoardManager.instance.InitializeBoard(grid);

            string score = scoreTxt.GetComponent<Text>().text;
            Assert.That(score, Is.EqualTo("0"));

            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[1, 2]);
            yield return new WaitForSeconds(1f);
            ClickUI(BoardManager.instance.tiles[0, 2]);

            yield return new WaitForSeconds(2f);

            score = scoreTxt.GetComponent<Text>().text;
            Assert.That(score, Is.EqualTo("150"));
        }
    }


}
