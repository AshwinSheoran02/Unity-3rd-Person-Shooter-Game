using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Animations.Rigging;
using Cinemachine;
public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private bool isGrounded = false;
    private Vector3 velocity;
    public static PlayerMovement instance;
    public bool isCrouch;
    public float speed;
    public float walkSpeed;
    public float runSpeed;
    public float gravityMultiplier = 1f;
    public float gravityValue = -9.81f;
    public float turnSmoothVelocity;
    public float groundCheckingRadius = 0.5f;
    public Camera cam;
    public LayerMask groundLayer;
    public float smoothTime = 0.1f;
    private Vector3 move;
    int crouchLayerIndex;
    int gunAimLayerIndex;
    int knifeLayerIndex;
    int gunHoldLayerIndex;

    public LayerMask aimColliderlayerMask;
    public float _cinemachineTargetYaw;
    public float _cinemachineTargetPitch;
    public float BottomClamp = -30.0f;
    public float TopClamp = 70.0f;
    public Transform AimOffset;
    public Rig Aim;    
    public CinemachineVirtualCamera aimCamera;
    public Transform aimTarget;
    public GameObject crossHair;
    public bool canMove;
    public bool canShot;
    public CinemachineFreeLook freeLookCam;
    public float currentDetection;
    public bool detectionFull;
    public Image DetectionImage;
    // Start is called before the first frame update

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        speed = walkSpeed;
        controller = GetComponent<CharacterController>();
        crouchLayerIndex = anim.GetLayerIndex("Crouch");
        knifeLayerIndex = anim.GetLayerIndex("Knife");
        gunAimLayerIndex = anim.GetLayerIndex("Pistol");
        gunHoldLayerIndex = anim.GetLayerIndex("PistolHold");
    }
    // Update is called once per frame
    void Update()
    {
        if(DetectionImage != null && !detectionFull){
            if(DetectionImage.fillAmount < 1)
                DetectionImage.fillAmount=currentDetection/100;
            if(DetectionImage.fillAmount >= 1){
                detectionFull = true;
            }
        }
        if(canMove){
            Locomotion();
        }
        else{
            move = Vector3.zero;
        }
        PlayAnimation();
        if(Input.GetKeyDown(KeyCode.C)){
            isCrouch = !isCrouch;
        }
        if(Input.GetKey(KeyCode.LeftShift) && !isCrouch){
            speed = runSpeed;
        }
        else{
            if(isCrouch){
                speed = 3;
            }
            else{
                speed = walkSpeed;
            }
        }
    }

    public void PlayAnimation(){
        if(isCrouch){
            anim.SetLayerWeight(crouchLayerIndex,1);
            anim.SetLayerWeight(knifeLayerIndex,0);
            anim.SetLayerWeight(gunAimLayerIndex,0);
            anim.SetLayerWeight(gunHoldLayerIndex,0);
        }
        else if(PlayerWeapon.instance.isHoldKnife){
            canShot = false;
            anim.SetLayerWeight(knifeLayerIndex,1);
            anim.SetLayerWeight(gunAimLayerIndex,0);
            anim.SetLayerWeight(crouchLayerIndex,0);
            anim.SetLayerWeight(gunHoldLayerIndex,0);
            Aim.weight = 0;
            aimCamera.Priority = 9;
            crossHair.SetActive(false);
            FixRotation();
        }
        else{
            if(Input.GetMouseButton(1)){
                anim.SetLayerWeight(gunAimLayerIndex,1);
                anim.SetLayerWeight(knifeLayerIndex,0);
                anim.SetLayerWeight(crouchLayerIndex,0);
                anim.SetLayerWeight(gunHoldLayerIndex,0);
                Aim.weight = 1;
                aimCamera.Priority = 11;
                crossHair.SetActive(true);
                RotAimRig();
                CameraRotation();
                canShot = true;
           }
            else{
                FixRotation();
                canShot = false;
                crossHair.SetActive(false);
                anim.SetLayerWeight(gunAimLayerIndex,0);
                anim.SetLayerWeight(knifeLayerIndex,0);
                anim.SetLayerWeight(crouchLayerIndex,0);
                anim.SetLayerWeight(gunHoldLayerIndex,1);
                aimCamera.Priority = 9;
                Aim.weight = 0;
            }
        }
        anim.SetFloat("Speed",move.magnitude*speed);
    }
    public void Locomotion(){
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        isGrounded = Physics.CheckSphere(transform.position,groundCheckingRadius,groundLayer);
        move = new Vector3(x, 0, z).normalized;
        if(move.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDir = Quaternion.Euler(0, targetangle, 0) * Vector3.forward;
            controller.Move(moveDir*Time.deltaTime*speed);
        }
        velocity.y += gravityValue*Time.deltaTime*gravityMultiplier;
        controller.Move(velocity * Time.deltaTime);
    }

    public void RotAimRig(){
        Vector3 mouseWorldPos = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f , Screen.height / 2f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast (ray, out RaycastHit raycastHit, 99999f, aimColliderlayerMask)){
            mouseWorldPos = raycastHit.point;
        }
        if(mouseWorldPos != Vector3.zero){
            aimTarget.position = mouseWorldPos;
            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward , aimDirection , Time.deltaTime*20f);
        }
    }

    public void FixRotation(){
        var rotationLR = cam.transform.localEulerAngles;
        _cinemachineTargetYaw = rotationLR.y;
        _cinemachineTargetPitch = -rotationLR.x;
        //_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch,BottomClamp,TopClamp);
    }

    public void CameraRotation()
    {
        _cinemachineTargetYaw += Input.GetAxisRaw("Mouse X");
        _cinemachineTargetPitch -= Input.GetAxisRaw("Mouse Y");
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch,BottomClamp,TopClamp);
        AimOffset.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,_cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
