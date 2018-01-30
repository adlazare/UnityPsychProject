using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	
	public Rigidbody characterRigidBody;
	Animator CharacterControllerV2;
	private float inputV;
	private float inputH;
	public static int SelectedDoor = 0;
	public static bool DoorTriggered = false;

	// Use this for initialization
	void Start () {
		CharacterControllerV2 = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		switch (MainSceneController.gamePhase)
		{
		case MainSceneController.GamePhase.StartMenuShowing:
			break;
		case MainSceneController.GamePhase.WalkingIntoRoom:
			if (transform.position.x < 0) {
				CharacterControllerV2.SetFloat("inputV", 1);
			}
			else {
				CharacterControllerV2.SetFloat("inputV", 0);
				StartCoroutine(WaitThenTurnLeft());
				MainSceneController.mainSceneController.GoToNextPhase();
			}
			break;
		case MainSceneController.GamePhase.TurnLeft:
			break;
		case MainSceneController.GamePhase.Tutorial1Walking:
			break;
		case MainSceneController.GamePhase.Dialogue1:
			CharacterControllerV2.SetFloat("inputV", 0);
			CharacterControllerV2.SetFloat("inputH", 0);
			break;
		case MainSceneController.GamePhase.HallwayChase:
			break;
		case MainSceneController.GamePhase.WaitingForPlayer:
			if (transform.position.z > 28) {
				MainSceneController.mainSceneController.GoToNextPhase();
				CharacterControllerV2.SetFloat("inputV", 0);
				CharacterControllerV2.SetFloat("inputH", 0);
			}
			break;
		case MainSceneController.GamePhase.PlayerChoosesDoor:

			break;
		}

		if (MainSceneController.ControlsEnabled) {
			bool isShiftPressed = Input.GetKey("left shift");
			CharacterControllerV2.SetBool("ShiftPressed", isShiftPressed);

			inputH = Input.GetAxis ("Horizontal");
			inputV = Input.GetAxis ("Vertical");

			CharacterControllerV2.SetFloat("inputH", inputH);
			CharacterControllerV2.SetFloat("inputV", inputV);
		}
	}

	void OnTriggerEnter (Collider Other) {
		// Using 'DoorTriggered' flag to only run this code once - otherwise the trigger will register 4 times
		if (!DoorTriggered) {
			Debug.Log("Entered trigger");
			Debug.Log(Other.gameObject);
			if (Other.gameObject.name == "MinigameDoorParent1") {SelectedDoor = 1;}
			else if (Other.gameObject.name == "MinigameDoorParent2") {SelectedDoor = 2;}
			else if (Other.gameObject.name == "MinigameDoorParent3") {SelectedDoor = 3;}
			Debug.Log(SelectedDoor);
			MainSceneController.mainSceneController.GoToNextPhase();
			DoorTriggered = true;
		}
	}

	private IEnumerator WaitThenTurnLeft () {
		yield return new WaitForSeconds(1);
		CharacterControllerV2.SetFloat("inputH", -1);
		yield return new WaitForSeconds(1);
		CharacterControllerV2.SetFloat("inputH", 0);	
		MainSceneController.mainSceneController.GoToNextPhase();
	}
}
