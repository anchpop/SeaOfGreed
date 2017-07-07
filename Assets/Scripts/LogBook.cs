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

        private enum Pages {
            none = 0,
            quests = 1,
            map = 2
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
            }
        }

        private void CloseLogBook() {
            pauseMenu.Unpause();
            logbookCanvas.enabled = false;
            gui.showUI = true;
            currentPage = Pages.none;
        }

        private void OpenToPage(Pages page) {
            gui.showUI = false;
            pauseMenu.Pause();
            logbookCanvas.enabled = true;
            currentPage = page;
        }
    }
}