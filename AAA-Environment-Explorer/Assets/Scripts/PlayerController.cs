using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Properties")]
    private CharacterController characterController;
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float rotationDuration = 0.15f;
    private float rotationProgress = 0.0f;
    private Quaternion currentRotation;
    private Quaternion targetRotation;

    private InputsControl playerInputs;
    private Animator animator;
    float currentBlend;
    float targetBlend;
    public float animationBlend = 10f;
    private Vector3 moveDir;

    private void Awake()
    {
        playerInputs = GetComponent<InputsControl>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // 1. Get input direction
        moveDir = new Vector3(playerInputs.MoveInput.x, 0, playerInputs.MoveInput.y).normalized;

        ApplyAnimation();
        characterController.Move(moveDir * moveSpeed * Time.fixedDeltaTime);

        // 4. Handle rotation if moving
        if (targetBlend > 0.01f)
        {
            StartRotation(moveDir);
            Rotation();
        }
    }

    #region Rotation
    private void StartRotation(Vector3 moveDir)
    {
        currentRotation = transform.rotation;
        targetRotation = Quaternion.LookRotation(moveDir);
        rotationProgress = 0.0f;
    }

    private void Rotation()
    {
        if (rotationProgress < 1.0f)
        {
            rotationProgress += Time.deltaTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationProgress);
        }
    }
    #endregion
    private void ApplyAnimation()
    {
        currentBlend = animator.GetFloat("Speed");
        targetBlend = moveDir.magnitude;
        float newBlend = Mathf.Lerp(currentBlend, targetBlend, Time.deltaTime * animationBlend);
        if (newBlend < 0.01f)
        {
            newBlend = 0f;
        }
        else if (newBlend > 0.95f)
        {
            newBlend = 1f;
        }
        animator.SetFloat("Speed", newBlend);
    }
}
