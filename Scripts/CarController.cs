using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.VFX;
public class CarController : MonoBehaviour
{
    public VisualEffect muzzleflash;
    public static CarController instance;
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    public LayerMask ColliderLayers;
    public float shootingCoolDown =  0.8f;
    public GameObject carPointer;
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    public Camera cam;
    public GameObject vehicleHealthBar;
    public Image vehicleFill;
    public GameObject turret;
    public float RotationSpeed = 10f;
    private float currentTime = 0.8f;    
    public GameObject crossHair;
    public GameObject Missile;
    public Transform firePoint;
    public HUDManager manager;
    public float force;

    public void Start(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
        HUDManager.instance.StartTimer(180);
        carPointer.SetActive(false);
        vehicleHealthBar.SetActive(true);
        crossHair.SetActive(true);
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }
    public void Update(){
        if(vehicleFill.fillAmount <= 0){
            manager.Failed();
            crossHair.SetActive(false);
            this.enabled = false;
        }
        HandleRotation();
        if(currentTime > 0){
            currentTime -= Time.deltaTime;
        }
        if(currentTime < 0 && Input.GetMouseButtonDown(0)){
            Shoot();
            muzzleflash.Play();
            currentTime = shootingCoolDown;
        }
    }

    public void Shoot(){
        
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f , Screen.height*(3/4f));
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,Mathf.Infinity,ColliderLayers)){
            if(hit.collider != null){        
                Vector3 dir = (hit.point - firePoint.position).normalized;
                GameObject bullet = Instantiate(Missile,firePoint.position,firePoint.rotation);
                bullet.GetComponent<Missile>().target = hit.point;
                bullet.GetComponent<Rigidbody>().AddForce(dir*force,ForceMode.Impulse);
            }
        }
    }
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    public void HandleRotation(){
        Vector3 targetDir = cam.transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        targetRotation.x = 0;
        targetRotation.z = 0;
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
    }


    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }


}
