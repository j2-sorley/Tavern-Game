using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController characterController;
    private Animator animator;

    [Header("Stats")]
    [SerializeField] private float lookSpeed = 100f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standingHeight = 1.8f;

    // Input varibles
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 lookInput = Vector2.zero;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isCrouching;

    // Logic variables
    private float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;
    private Transform cameraTransform;
    private float verticalLookRotation;
    public float pushPower = 50.0F;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Camera _camera = GetComponentInChildren<Camera>();
        cameraTransform = _camera.gameObject.transform;
    }


    void Update()
    {

        Move();

        Jump();

        Look();

        
    }

    public void ToggleCrouch(bool isCrouchButtonPressed)
    {
        if (isCrouchButtonPressed)
        {
            isCrouching = !isCrouching;
            characterController.height = isCrouching ? crouchHeight : standingHeight;
        }
    }

    private void Move()
    {
        // Calculate movement
        Vector3 move = transform.right *0.7f * moveInput.x + transform.forward * moveInput.y;
        animator.SetFloat("ForwardVelocity", moveInput.y);
        animator.SetFloat("RightVelocity", moveInput.x);
        float speed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);
        characterController.Move(move * speed * Time.deltaTime);
    }

    private void Jump()
    {
        // Un Crouch if crounched
        if(isCrouching && isJumping)
        {
            isJumping = false;
            Crouch();
        }

        // Jump
        if (isJumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isJumping = false;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Ground Check
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }


    }

    private void Look()
    {

        // Inputs
        float horizontalLook = lookInput.x * lookSpeed * Time.deltaTime;
        float verticalLook = lookInput.y * lookSpeed * Time.deltaTime;

        // Vertical rotation
        verticalLookRotation -= verticalLook;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Rotatation
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        transform.Rotate(Vector3.up * horizontalLook);
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        float currentHeight = isCrouching ? crouchHeight : standingHeight;
        characterController.height = currentHeight;
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, currentHeight, cameraTransform.localPosition.z);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        var Script = hit.collider.GetComponent<ItemHealth>();  

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        //if (hit.moveDirection.y < -0.3f)
         //   return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
        Script.ItemHit(); 


         if (hit.gameObject.tag == "Glass")
        {
            Debug.Log("Glass"); 
        }

        if (hit.gameObject.tag == "Wood")
        {
            Debug.Log("Wood");
        }

    }
}
