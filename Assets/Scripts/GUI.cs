using System;
using TeamUtility.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SeaOfGreed {

    public class GUI : MonoBehaviour {
        public RawImage Minimap;
        public Canvas UICanvas;
        public Text[] helperText;
        public Text dockText;
        public Text boardText;
        public Text sailText;

        public bool showUI = true;

        private void Update() {
            var shipAxes = InputManager.GetAxisConfiguration("Default", "Enter Ship").positive.ToString();
            var useAxes = InputManager.GetAxisConfiguration("Default", "Use").positive.ToString();

            Minimap.transform.localScale = new Vector3(.7f, .7f, .7f) * GameManager.options.game.UIScale;
            foreach (var text in helperText) {
                text.fontSize = Map((int)GameManager.options.game.UIScale, 1, 3, 10, 20);
            }
            dockText.text = "Press '" + shipAxes + "' to dock";
            boardText.text = "Press '" + shipAxes + "' to board";
            sailText.text = "Press '" + useAxes + "' to set sail";
            if (showUI) {
                UICanvas.enabled = GameManager.options.game.DrawUI;
            } else {
                UICanvas.enabled = false;
            }
        }

        private int Map(int oldValue, int oldMin, int oldMax, int newMin, int newMax) {
            int oldRange = (oldMax - oldMin);
            int newRange = (newMax - newMin);
            int newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
            return newValue;
        }
    }
}