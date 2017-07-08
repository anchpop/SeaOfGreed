using TeamUtility.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatObject : CombatObject {

    //combat object; if you hit C it spawns a projectile (controlled by damage.cs)
    //TODO: move this control to something better when we flesh out attacking.
    [SerializeField] private GameObject childObjectL;

    [SerializeField] private GameObject childObjectR;
    protected override void onDeath() {
        SceneManager.LoadScene("menu");
    }

    protected override void childUpdate() {
        if (InputManager.GetButtonDown("LFire")) {
            childObjectL.SetActive(true);
        }
        if (InputManager.GetButtonDown("RFire")) {
            childObjectR.SetActive(true);
        }
    }
}