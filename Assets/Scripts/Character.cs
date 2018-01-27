using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	
	public Rigidbody characterRigidBody;
	Animator CharacterControllerV2;
	private float inputV;
	private float inputH;

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
		}

		if (MainSceneController.ControlsEnabled) {
			bool isShiftPressed = Input.GetKey("left shift");
			CharacterControllerV2.SetBool("ShiftPressed", isShiftPressed);

			bool isSpacePressed = Input.GetKey("space");
			CharacterControllerV2.SetBool("SpacePressed", isSpacePressed);

			inputH = Input.GetAxis ("Horizontal");
			inputV = Input.GetAxis ("Vertical");

			CharacterControllerV2.SetFloat("inputH", inputH);
			CharacterControllerV2.SetFloat("inputV", inputV);
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
