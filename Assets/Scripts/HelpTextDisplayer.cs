using System.Collections;
using System.Collections.Generic;
using System;
using TeamUtility.IO;
using UnityEngine;
using UnityEngine.UI;



public class HelpTextDisplayer : MonoBehaviour {
    public enum helpText
    {
        boardText,
        dockText,
        steerText,
        none,
    }

    public string boardText;
    public string dockText;
    public string steerText;

    public Text textObject;

    [HideInInspector]
    public helpText textToShow = helpText.none;
    private helpText oldTextToShow = helpText.none;
    // Use this for initialization
    void Start () {
        textObject = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (textToShow != oldTextToShow)
        {
            var shipAxes = InputManager.GetAxisConfiguration("Default", "Enter Ship").positive.ToString();
            var useAxes = InputManager.GetAxisConfiguration("Default", "Use").positive.ToString();


            if (textToShow == helpText.none)
                textObject.text = "";
            else if (textToShow == helpText.dockText)
                textObject.text = "Press '" + shipAxes + "' " + dockText;
            else if (textToShow == helpText.steerText)
                textObject.text = "Press '" + useAxes + "' " + steerText;
            else if (textToShow == helpText.boardText)
                textObject.text = "Press '" + shipAxes + "' " + boardText;
            textObject.fontSize = Map((int)SeaOfGreed.GameManager.options.game.UIScale, 1, 3, 10, 20);
            oldTextToShow = textToShow;
        }
    }

    static private int Map(int oldValue, int oldMin, int oldMax, int newMin, int newMax)
    {
        int oldRange = (oldMax - oldMin);
        int newRange = (newMax - newMin);
        int newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
        return newValue;
    }
}
