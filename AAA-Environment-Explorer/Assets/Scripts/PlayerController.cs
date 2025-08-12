using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Properties")]
    private CharacterController characterController;
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float rotationDuration = 0.15f;
    [SerializeField] float gravityMultiplier = 2f;
    private Vector3 moveDir;
    float Gravity = -9.8f;
    float velY;
    private float rotationProgress = 0.0f;
    private Quaternion currentRotation;
    private Quaternion targetRotation;

    //Inputs and Animator
    private InputsControl playerInputs;
    private Animator animator;
    float currentBlend;
    float targetBlend;
    bool turn_L=false;
    bool turn_R=false;
    public float animationBlend = 10f;

    [SerializeField] Camera cam;
    Vector3 camRight;
    Vector3 camForward;
    Vector3 playerForward;
    Vector3 playerRight;

    float stayTime=0f;
    private void Awake()
    {
        playerInputs = GetComponent<InputsControl>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //For Camera relative Movement
        CameraRelative();
        moveDir = new Vector3(playerInputs.MoveInput.x, 0, playerInputs.MoveInput.y).normalized;
        moveDir = camRight * moveDir.x + camForward * moveDir.z;
        ApplyAnimation();
        //
        //For Gravity
        ApplyGravity(Time.fixedDeltaTime);
        moveDir.y = velY;
        //

        characterController.Move(moveDir * moveSpeed * Time.fixedDeltaTime);
        
        //For Rotation
        if (targetBlend > 0.01f)
        {
            StartRotation(new Vector3(moveDir.x,0,moveDir.z));
            Rotation();
        }
        //

        //For Turn enable
        if (new Vector2(moveDir.x, moveDir.z).magnitude == 0f)
        {
            stayTime += Time.fixedDeltaTime;
            if (stayTime > 2f)
            {
                stayTime = 2f;
            }
        } else { stayTime = 0f; }
        if (stayTime > 1.5f)
            Turn();
        //
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
    #region Gravity
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
    private void ApplyGravity(float delta)
    {
     
        
        if (characterController.isGrounded && velY < 0f)
        {
            velY = -2f;
        }
        else
        {
            velY = gravityMultiplier * Gravity * delta;
        }
        moveDir.y = velY;
    }
    #endregion

    private void CameraRelative()
    {
        camForward = new Vector3(cam.transform.forward.x,0,cam.transform.forward.z);
        camRight = new Vector3(cam.transform.right.x,0,cam.transform.right.z);
        camForward.Normalize();
        camRight.Normalize();
    }
    #region Turn
    private void Turn()
    {
        playerForward = transform.forward;
        playerRight = transform.right;

        if(new Vector2(moveDir.x,moveDir.z).magnitude==0f)
        {
            if (Vector3.Dot(playerForward, camForward) < 0.5f&& Vector3.Dot(playerForward, camForward)>-0.3f)
            {
                if (Vector3.Dot(playerRight, camForward) > 0.001f)
                {
                    turn_R = true;
                    animator.applyRootMotion = true;
                }
                else
                {
                    animator.applyRootMotion = true;
                    turn_L = true;
                }
            }
            if (Vector3.Dot(playerForward, camForward) > 0.95f)
            {
                animator.applyRootMotion = false;
                turn_R = false;
                turn_L = false;
            }
            animator.SetBool("Turn_L", turn_L);
            animator.SetBool("Turn_R", turn_R);
        }
        
    }
    #endregion
}
