using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeapon : MonoBehaviour
{
    public static PlayerWeapon instance;
    public bool isHoldKnife;
    public bool isHoldGun;
    public Transform gun;
    public Transform knife;
    private Animator anim;
    public GameObject knifeObject;
    public bool canShot;
    public bool knifeAttack;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
        isHoldKnife = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isKnife",isHoldKnife);
        anim.SetBool("GunHold",isHoldGun);
        if(isHoldGun){
            gun.gameObject.SetActive(true);
            knife.gameObject.SetActive(false);
        }
        else{
            PlayerMovement.instance.FixRotation();
            knife.gameObject.SetActive(true);
            gun.gameObject.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isHoldKnife){
            isHoldKnife = true;
            isHoldGun = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && !isHoldGun){
            isHoldGun = true;
            isHoldKnife = false;
        }
        if(Input.GetMouseButtonDown(0) && isHoldKnife && !knifeAttack){
            canShot = false;
            anim.SetTrigger("Attack");
            knifeAttack = true;
            PlayerMovement.instance.canMove = false;
            knifeObject.GetComponent<Collider>().enabled = true;
            StartCoroutine(CoolDownMove(2));
        }
        else if(Input.GetMouseButton(1) && isHoldGun && PlayerMovement.instance.isCrouch){
            canShot = true;
            anim.SetBool("GunShot",true);
            PlayerMovement.instance.aimCamera.Priority = 11;
            PlayerMovement.instance.crossHair.SetActive(true);
            PlayerMovement.instance.canMove = false;
            PlayerMovement.instance.Aim.weight = 1;
            PlayerMovement.instance.RotAimRig();
            PlayerMovement.instance.CameraRotation();
        }
        else if(isHoldGun && PlayerMovement.instance.isCrouch){
            PlayerMovement.instance.FixRotation();
            canShot = false;
            anim.SetBool("GunShot",false);
            PlayerMovement.instance.aimCamera.Priority = 9;
            PlayerMovement.instance.crossHair.SetActive(false);
            PlayerMovement.instance.canMove = true;
            PlayerMovement.instance.Aim.weight = 0;
        }
    }

    IEnumerator CoolDownMove(float time){
        yield return new WaitForSeconds(time);
        PlayerMovement.instance.canMove = true;
        knifeObject.GetComponent<Collider>().enabled = false;
        knifeAttack = false;
    }
}
