using TeamUtility.IO;
using UnityEngine;

namespace SeaOfGreed {

    public class PlayerController : MonoBehaviour
    { 
        
        public GameObject helpText;
        HelpTextDisplayer helpTextObject;
        [SerializeField] private GameObject frontChild;
        private static bool canMove;
        public Camera mainCamera;
        public float walkingCameraSize = 4;
        public float interactingCameraSize = 6;
        public float cameraEaseTime = 2;

        public CharacterDriver driver;

        Canvas canvas;


        // Use this for initialization
        private void Start() {
            driver = GetComponent<CharacterDriver>();
            mainCamera =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().player = gameObject;
            canvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();

            helpTextObject = Instantiate(helpText, canvas.transform).GetComponent<HelpTextDisplayer>();
            canMove = true;
        }

        private void walkAccordingToUserInput() {
            var input_x_right = InputManager.GetButton("Player Right") ? 1 : 0;
            var input_x_left = InputManager.GetButton("Player Left") ? 1 : 0;
            var input_y_forward = InputManager.GetButton("Player Forward") ? 1 : 0;
            var input_y_backward = InputManager.GetButton("Player Backward") ? 1 : 0;
            var input_x = input_x_right - input_x_left;
            var input_y = input_y_forward - input_y_backward;

            driver.walkInDirection(new Vector3(input_x, input_y));
        }

        private void steerShipAccordingToUserInput() {
            driver.sprite.transform.localRotation = Quaternion.identity;
            var shipController = driver.shipBorded.GetComponent<ShipController>();
            if (InputManager.GetButton("Ship Accelerate")) {
                shipController.accelerate();
            }
            if (InputManager.GetButton("Ship Brake")) {
                shipController.brake();
            }
            if (InputManager.GetButton("Ship Left")) {
                shipController.turn(1);
            }
            if (InputManager.GetButton("Ship Right")) {
                shipController.turn(-1);
            }
            if (InputManager.GetButtonDown("Use")) {
                //Debug.Log("WantsToSteeringShiptoBoardedShip");
                driver.steeringShipToBoardedShip();
            }
        }
        private Vector3 getCursorDiff(){
            var mousePos = mainCamera.ScreenToWorldPoint(new Vector2(InputManager.mousePosition.x, InputManager.mousePosition.y));
            return(mousePos - driver.transform.position);
        }
        private void lookTowardsMouse() {
            if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip) {
                driver.lookInDirection(getCursorDiff());
            }
        }

        private void displayHelpText() {
            var helpToShow = HelpTextDisplayer.helpText.none;
            if (driver.state == states.boardedShip && driver.canGrabWheel())
                helpToShow = HelpTextDisplayer.helpText.steerText;
            else if (driver.state == states.onLand && driver.canBoardShip())
                helpToShow = HelpTextDisplayer.helpText.boardText;
            else if (driver.state == states.boardedShip && driver.canDockShip())
                helpToShow = HelpTextDisplayer.helpText.dockText;

            helpTextObject.textToShow = helpToShow;
        }
        //used to not instantly interpret a continue "E" press as an interact "E" press, trapping you in a loop of conversation.
        private static bool wasActiveLastFrame;
        private void Update() {
            if (Time.timeScale != 0f && canMove == true) {
                if (driver.state == states.boardedShip || driver.state == states.onLand) {
                    walkAccordingToUserInput();
                }
                displayHelpText();

                if (driver.state == states.onLand && driver.canBoardShip() && InputManager.GetButtonDown("Enter Ship")) {
                    driver.boardShipHelper();
                }
                if (driver.state == states.boardedShip && driver.canDockShip() && InputManager.GetButtonDown("Enter Ship")) {
                    driver.dockShipHelper();
                }
                if (driver.state == states.boardedShip && driver.canGrabWheel() && InputManager.GetButtonDown("Use")) {
                    driver.grabWheelHelper();
                }
                if (driver.state == states.steeringShip) {
                    steerShipAccordingToUserInput();
                }
                if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip) {
                    lookTowardsMouse();
                }
                if (InputManager.GetButtonDown("Sprint")) {
                    driver.isSprinting = true;
                } else if (InputManager.GetButtonUp("Sprint")) {
                    driver.isSprinting = false;
                }
                if(InputManager.GetButtonDown("Use") && wasActiveLastFrame){
                    //not sure if you actually have to normalize, but it makes me feel like it's less likely to break lol
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, getCursorDiff().normalized, 2);
                    for(int i  = 0; i<hits.Length; i++){
                        if(hits[i].transform.gameObject.tag == "interactable"){
                            hits[i].transform.gameObject.GetComponent<NPCPassiveController>().OnInteract();
                            break;
                        }
                    }
                } else{
                    wasActiveLastFrame = true;
                }
            }
        }
        public static void setMove(bool a){
            if(!a)
                wasActiveLastFrame = false;
            canMove = a;
        }
        public static bool getMove(){
            return canMove;
        }
    }
}