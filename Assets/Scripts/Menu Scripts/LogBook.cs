using System;
using TeamUtility.IO;
using UnityEngine;

namespace SeaOfGreed {

    public class LogBook : MonoBehaviour {
        private Pages currentPage = Pages.none;
        private bool isOpen = false;

        public Canvas logbookCanvas;
        public PauseMenu pauseMenu;

        public GUI gui;

        public Canvas mapCanvas;
        public Canvas questCanvas;
        public Canvas pauseCanvas;
        public Canvas optionsCanvas;

        private enum Pages {
            none = 0,
            quests = 1,
            map = 2,
            pause = 3,
            options = 4
        }

        // Use this for initialization
        private void Start() {
        }

        // Update is called once per frame
        private void Update() {
            if (InputManager.GetButtonDown("Map")) {
                if (currentPage != Pages.map) {
                    OpenToPage(Pages.map);
                } else if (currentPage == Pages.map) {
                    CloseLogBook();
                }
            } else if (InputManager.GetButtonDown("Quests")) {
                if (currentPage != Pages.quests) {
                    OpenToPage(Pages.quests);
                } else if (currentPage == Pages.quests) {
                    CloseLogBook();
                }
            } else if (InputManager.GetButtonDown("Pause")) {
                if (currentPage != Pages.pause) {
                    OpenToPage(Pages.pause);
                } else if (currentPage == Pages.pause) {
                    if (!pauseMenu.controlsCanvas.GetComponent<ControlOptions>().isEditing && (pauseCanvas.enabled || optionsCanvas.enabled)) {
                        Debug.Log("yes");
                        CloseLogBook();
                    }
                }
            }
        }

        public void CloseLogBook() {
            mapCanvas.enabled = false;
            questCanvas.enabled = false;
            pauseCanvas.enabled = false;
            optionsCanvas.enabled = false;
            pauseMenu.Unpause();
            gui.showUI = true;
            currentPage = Pages.none;
        }

        private void DisableAll() {
            questCanvas.enabled = false;
            mapCanvas.enabled = false;
            pauseCanvas.enabled = false;
            optionsCanvas.enabled = false;
        }

        private void OpenToPage(Pages page) {
            gui.showUI = false;
            pauseMenu.Pause();
            currentPage = page;
            switch (page) {
                case Pages.none:
                    CloseLogBook();
                    break;

                case Pages.quests:
                    DisableAll();
                    questCanvas.enabled = true;
                    break;

                case Pages.map:
                    DisableAll();
                    mapCanvas.enabled = true;
                    break;

                case Pages.pause:
                    DisableAll();
                    pauseMenu.ShowPause();
                    break;

                case Pages.options:
                    DisableAll();
                    optionsCanvas.enabled = true;
                    break;
            }
        }
    }
}