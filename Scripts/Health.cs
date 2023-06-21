using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public GameObject addAmmo;
    public float health;
    public Vector3 offset;
    public Image fill;
    public bool die;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Player"){
            fill.fillAmount = health/100;
        }
        if(health <= 0){
            if(gameObject.tag == "Enemy" && !die){
                die = true;
                Die();
            }
            else if(!die){
                die = true;
                GetComponent<PlayerWeapon>().enabled = false;
                GetComponent<Shooting>().enabled = false;
                PlayerDie();
            }
        }
        
    }

    public void TakeDamage(float damage){
        if(gameObject.tag == "Enemy"){
            gameObject.GetComponent<Enemy>().haveShot = true;
        }
        health -= damage;
    }
    public void AddHealth(float _health){
        health = Mathf.Clamp(health+_health,0,100);
    }

    public void Die(){
        Instantiate(addAmmo,transform.position + offset,Quaternion.identity);
        GetComponent<ActivateRagdoll>().EnableRagDoll();
        GetComponentInChildren<Light>().enabled = false;
        Destroy(gameObject,5);
    }

    public void PlayerDie(){
        GetComponent<ActivateRagdoll>().EnableRagDoll();
        HUDManager.instance.Failed();
    }
}