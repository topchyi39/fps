using System;
using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif

namespace Input
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool fire;
		public bool ultimate;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private bool _inputEnabled = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnFire(InputValue value)
		{
			FireInput(value.isPressed);
		}

		public void OnUltimate(InputValue value)
		{
			UltimateInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			if (!_inputEnabled)
			{
				move = Vector2.zero;
				return;
			}
			
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			if (!_inputEnabled)
			{
				look = Vector2.zero;
				return;
			}

			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (!_inputEnabled)
			{
				jump = false;
				return;
			}
			
			jump = newJumpState;
		}

		public void FireInput(bool isFire)
		{
			if (!_inputEnabled)
			{
				fire = false;
				return;
			}
			
			fire = isFire;
		}

		public void UltimateInput(bool isUltimate)
		{
			if (!_inputEnabled)
			{
				ultimate = false;
				return;
			}
			
			ultimate = isUltimate;
		}

		public void SprintInput(bool newSprintState)
		{
			if (!_inputEnabled)
			{
				sprint = false;
				return;
			}
			
			if(newSprintState)
				sprint = !sprint;
		}

		public void Enable()
		{
			_inputEnabled = true;
		}

		public void Disable()
		{
			_inputEnabled = false;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}