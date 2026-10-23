using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FistPersonController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float speed = 3.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 1.0f;
    [SerializeField] private float upDownLookRange = 80.0f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] public Animator targetAnimator;

    [Header("AccuseSettings")]
    [SerializeField] public bool canAccuse = false;
    [SerializeField] public bool accusationInProgress = false;
    [SerializeField] public bool hasResolvedAccuse = false;
    [SerializeField] private float accuseRayDistance = 10000.0f;

    private Vector3 currentMovement;
    private float verticalRotation;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        AccuseWitness();
    }
    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDir = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDir = transform.TransformDirection(inputDir);
        return worldDir.normalized;
    }

    private void AccuseWitness()
    {
        if (!canAccuse || accusationInProgress)
        {
            return;
        }

        if (playerInputHandler.CanAccuse && !pauseMenu.isPaused)
        {
            Debug.Log("Accusation made!");
            accusationInProgress = true;
            hasResolvedAccuse = false;
            playerAnimator.ResetTrigger("Accuse");
            playerAnimator.SetTrigger("Accuse");
            OnAccuseImpact();
        }
    }

    private void OnAccuseImpact()
    {
        if (hasResolvedAccuse)
        {
            return;
        }
        hasResolvedAccuse = true;
        StartCoroutine(LetAnimationsPlay());
        accusationInProgress = false;
        hasResolvedAccuse = false;
    }

    private void HandleMovement()
    {
        Vector3 worldDir = CalculateWorldDirection();
        currentMovement.x = worldDir.x * speed;
        currentMovement.z = worldDir.z * speed;

        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity * Time.deltaTime * 5;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity * Time.deltaTime * 5;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    private IEnumerator LetAnimationsPlay()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, accuseRayDistance))
        {
            Witness witness = hit.collider.GetComponentInParent<Witness>();
            targetAnimator = hit.collider.GetComponentInParent<Animator>();

            if (witness != null)
            {
                canAccuse = true;

                if (targetAnimator != null)
                {
                    targetAnimator.ResetTrigger("Accused");
                    targetAnimator.SetTrigger("Accused");
                }
            }

            accusationInProgress = false;
            hasResolvedAccuse = false;

            yield return new WaitForSecondsRealtime(3);
                
            if (witness.isKiller)
            {
                SceneManager.LoadScene(3);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }

        accusationInProgress = false;
    }
}
