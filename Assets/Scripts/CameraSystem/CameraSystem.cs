using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] private float fieldOfViewMax = 50f;
	[SerializeField] private float fieldOfViewMin = 10f;
	[SerializeField] private float followOffsetMin = 5f;
	[SerializeField] private float followOffsetMax = 50f;

	private float targetFieldOfView = 50f;
	private Vector3 followOffset;


	private void Awake()
	{
		followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
	}
	private void Update()
	{
		HandleCameraMovement();
		HandleCameraRotation();
		//HandleCameraZoom_FieldOfView();
		HandleCameraZoom_MoveForward();

	}

	private void HandleCameraMovement()
	{
		Vector3 inputDir = new Vector3(0, 0, 0);

		if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
		if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
		if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
		if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

		Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

		float moveSpeed = 30f;
		transform.position += moveDir * moveSpeed * Time.deltaTime;

	}

	private void HandleCameraRotation()
	{
		float rotateDir = 0f;

		if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
		if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

		float rotateSpeed = 100f;
		transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
	}

	private void HandleCameraZoom_FieldOfView()
	{
		//Input.mouseScrollDelta
		if(Input.mouseScrollDelta.y > 0)
		{
			targetFieldOfView -= 5;
		}
		if (Input.mouseScrollDelta.y < 0)
		{
			targetFieldOfView += 5;
		}

		targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);


		float zoomSpeed = 10f;
		
		cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);

	}

	private void HandleCameraZoom_MoveForward()
	{

		float zoomAmount = 3f;
		Vector3 zoomDir = followOffset.normalized;
		if(Input.mouseScrollDelta.y > 0)
		{
			followOffset -= zoomDir * zoomAmount;
		}
		if (Input.mouseScrollDelta.y < 0)
		{
			followOffset += zoomDir * zoomAmount;
		}

		if(followOffset.magnitude < followOffsetMin)
		{
			followOffset = zoomDir * followOffsetMin;
		}
		if (followOffset.magnitude > followOffsetMax)
		{
			followOffset = zoomDir * followOffsetMax;
		}

		float zoomSpeed = 10f;
		Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
		cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = followOffset;
	}
}
