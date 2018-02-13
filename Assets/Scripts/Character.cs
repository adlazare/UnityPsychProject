﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	
	public Rigidbody characterRigidBody;
	Animator CharacterControllerV2;
	private float v;
	private float h;
	private bool SpacePressed;
	public static int SelectedDoor = 0;
	public static bool DoorTriggered = false;
	public static bool RollStarted = false;
	public static bool transitionFlag = false;
	public static bool transitionFlag2 = false;
	public static bool transitionFlag3 = false;



	// Use this for initialization
	void Start () {
		CharacterControllerV2 = GetComponent<Animator>();
		characterRigidBody = GetComponent<Rigidbody>();
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
		case MainSceneController.GamePhase.WaitingForPlayerAtRiverBank:
			if (transform.position.x > 20) {
				MainSceneController.mainSceneController.GoToNextPhase();
			}
			break;
		case MainSceneController.GamePhase.ClipboardSwimsAcrossRiver:
			CharacterControllerV2.SetFloat("inputV", 0);
			CharacterControllerV2.SetFloat("inputH", 0);
			break;
		case MainSceneController.GamePhase.RiverCrossing:
			if (transform.position.x > 40) {
				MainSceneController.mainSceneController.GoToNextPhase();
			}
			break;
		case MainSceneController.GamePhase.ClipboardRunsToFireMinigame:
			if (transform.position.x > 60) {
				MainSceneController.mainSceneController.GoToNextPhase();
			}
			break;
		case MainSceneController.GamePhase.DialogueInterruptedByFireWall:
			CharacterControllerV2.SetFloat("inputV", 0);
			CharacterControllerV2.SetFloat("inputH", 0);
			break;
		case MainSceneController.GamePhase.StairsAppear:
			if (transform.rotation.eulerAngles.y < 135) {
				CharacterControllerV2.SetFloat("inputH", 1);
			}
			if (transform.rotation.eulerAngles.y > 135) {
				CharacterControllerV2.SetFloat("inputH", 0);
			}
			break;
		case MainSceneController.GamePhase.WaitForPlayerToPushSpace:
			bool isSpacePressed = Input.GetKey("space");
			if (isSpacePressed == true) {
				MainSceneController.mainSceneController.GoToNextPhase();
			}
			break;
		case MainSceneController.GamePhase.CharacterFallsIntoFire:
			CharacterControllerV2.SetFloat("inputV", 1);
			characterRigidBody.freezeRotation = false;
			if (transitionFlag == false) {
				StartCoroutine(WaitForTransition());
				transitionFlag = true;
			}
			break;
		case MainSceneController.GamePhase.CharacterExitsFire:
			CharacterControllerV2.SetFloat("inputV", 0);
			if (transform.position.x < 68) {
				CharacterControllerV2.SetBool("ShiftPressed", true);
				CharacterControllerV2.SetFloat("inputV", 1);
			}
			if (transform.position.x > 68) {
				CharacterControllerV2.SetBool("ShiftPressed", false);
				CharacterControllerV2.SetFloat("inputV", 0);
				if (transitionFlag2 == false) {
					StartCoroutine(TransitionPhase());
					transitionFlag2 = true;
				}
			}
			break;
		case MainSceneController.GamePhase.TransitionToNextHallway:
			if (transform.position.x > 91) {
				CharacterControllerV2.SetFloat("inputV", 0);
				CharacterControllerV2.SetFloat("inputH", 0);
				if (transitionFlag3 == false) {
					MainSceneController.mainSceneController.GoToNextPhase();
					transitionFlag3 = true;
				}
			}
			break;
		}


		if (MainSceneController.ControlsEnabled) {
			bool isShiftPressed = Input.GetKey("left shift");
			CharacterControllerV2.SetBool("ShiftPressed", isShiftPressed);

			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");

			CharacterControllerV2.SetFloat("inputH", h);
			CharacterControllerV2.SetFloat("inputV", v);
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

	private IEnumerator WaitForTransition () {
		yield return new WaitForSeconds(3f);
		characterRigidBody.freezeRotation = true;
		MainSceneController.mainSceneController.GoToNextPhase();
	}

	private IEnumerator TransitionPhase () {
		yield return new WaitForSeconds(2f);
		MainSceneController.mainSceneController.GoToNextPhase();
	}
}
