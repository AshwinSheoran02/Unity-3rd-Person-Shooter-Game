using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class Shooting : MonoBehaviour
{
    public AudioClip pistolShoot;
    public AudioSource shootingSound;
    public float damage;
    public float force;
    public VisualEffect muzzleflash;
    public Transform firePoint;
    public float currentAmmo = 8;
    public float totalAmmo;
    public bool onReload;
    public float fireRate = 0.5f;
    public float currentTime;
    public float reloadingTime;
    public ParticleSystem shellEffect;
    public GameObject hitImpact;
    // Start is called before the first frame update
    void Start()
    {
        HUDManager.instance.UpdateAmmo(currentAmmo);
        HUDManager.instance.UpdatetotalAmmo(totalAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > 0){
            currentTime -= Time.deltaTime;
        }
        if(((Input.GetKeyDown(KeyCode.R) && currentAmmo < 8) || currentAmmo == 0) && !onReload && totalAmmo > 0){
            onReload = true;
            StartCoroutine(Reload());
        }
        if(currentTime <= 0 && Input.GetMouseButtonDown(0) && (PlayerMovement.instance.canShot || PlayerWeapon.instance.canShot) && currentAmmo > 0 && !onReload){
            Shoot();
        }
    }

    public void Shoot(){
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f , Screen.height/2f);
        Ray ray = PlayerMovement.instance.cam.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit)){
            shootingSound.PlayOneShot(pistolShoot);
            Vector3 dir = (hit.point - firePoint.position).normalized;
            currentAmmo--;
            HUDManager.instance.UpdateAmmo(currentAmmo);
            muzzleflash.Play();
            shellEffect.Emit(1);
            currentTime = fireRate;
            GameObject impact = Instantiate(hitImpact,hit.point,Quaternion.identity);
            impact.transform.up = hit.normal;
            Destroy(impact,1);
            if(hit.collider.gameObject.tag == "Enemy"){
                hit.collider.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    IEnumerator Reload(){
        yield return new WaitForSeconds(reloadingTime);
        float previousAmmo = currentAmmo;
        currentAmmo = Mathf.Min(totalAmmo,8);
        onReload = false;
        HUDManager.instance.UpdateAmmo(currentAmmo);
        totalAmmo -= currentAmmo - previousAmmo;
        HUDManager.instance.UpdatetotalAmmo(totalAmmo);
    }
    public void addAmmo(int ammo){
        totalAmmo += ammo;
        HUDManager.instance.UpdatetotalAmmo(totalAmmo);
    }
}
