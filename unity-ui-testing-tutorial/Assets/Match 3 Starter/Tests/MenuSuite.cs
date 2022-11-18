using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class MenuSuite : InputTestFixture
    {
        Mouse mouse;

        public override void Setup()
        {
            base.Setup();
            SceneManager.LoadScene("Match 3 Starter/Scenes/Menu");
            mouse = InputSystem.AddDevice<Mouse>();
        }

        [UnityTest]
        public IEnumerator TestGameStart()
        {
            GameObject playButton = GameObject.Find("MenuCanvas/PlayButton");

            ClickUI(playButton);
            yield return new WaitForSeconds(2f);

            string sceneName = SceneManager.GetActiveScene().name;
            Assert.That(sceneName, Is.EqualTo("Game"));

        }
        public void ClickUI(GameObject uiElement)
        {
            Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position);
            Set(mouse.position, screenPos);
            Click(mouse.leftButton);
        }

    }
}