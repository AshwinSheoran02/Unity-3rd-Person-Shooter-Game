using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
public class Enemy : MonoBehaviour
{
    public LayerMask obstacle;
    public Transform leftCheck;
    public Transform rightCheck;
    public Transform[] PatrolPoints;
    public NavMeshAgent agent;
    bool isPatrol = false;
    public Animator anim;
    int i = 0;
    public float detectionRadius;
    public float waitingTime;
    public LayerMask layerMask;
    public float detectionAngle;
    public GameObject chasing;
    public float timeToChase;
    float currentTimeChase;
    public bool waiting;
    public float shootingDistance;
    public float shootingCoolDown;
    public float currentShootingTime;
    public Light spotLight;
    public float stoppingDistance;
    public Transform FirePoint;
    public float cooldownTime = 0.1f;
    public AudioSource movementSource;
    public AudioSource shootingSource;
    public float damage;
    public GameObject hitImpact;
    public AudioClip shootClip;
    public bool movementCooldown = false;
    public AudioClip walkClip;
    public AudioClip runClip;
    public float prev;
    public ParticleSystem bulletshellVfx;
    public VisualEffect muzzleFlash;
    public Vector3 offset;
    public float closeRadius;
    public GameObject target;
    public bool haveShot;
    public bool isCampMan;
    // Start is called before the first frame update
    void Start()
    {
        if(!isCampMan){
            currentTimeChase = timeToChase;
            target = PlayerMovement.instance.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isCampMan){
            if(CarController.instance != null){
                target = CarController.instance.gameObject;
                chasing = target;
            }
            SoundPart();
            anim.SetFloat("Speed",agent.speed);
            if(target != null && Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(target.transform.position.x,0,target.transform.position.z)) <= stoppingDistance){
                agent.speed = 0;
                Vector3 toTarget = target.transform.position - transform.position;
                var toTargetRotation = Quaternion.LookRotation(toTarget);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,toTargetRotation,360*Time.deltaTime);
            }
            if(currentShootingTime > 0){
                currentShootingTime -= Time.deltaTime;
            }
            if(chasing != null && Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(chasing.transform.position.x,0,chasing.transform.position.z)) <= shootingDistance && currentShootingTime <= 0){
                currentShootingTime = shootingCoolDown;
                Shoot();
                anim.SetBool("StandShoot",true);
            }
            else{
                anim.SetBool("StandShoot",false);
            }
        }
        else{
            SoundPart();
            anim.SetFloat("Speed",agent.speed);
            if(Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(target.transform.position.x,0,target.transform.position.z)) <= stoppingDistance){
                agent.speed = 0;
                Vector3 toTarget = target.transform.position - transform.position;
                var toTargetRotation = Quaternion.LookRotation(toTarget);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,toTargetRotation,360*Time.deltaTime);
            }
            if(currentShootingTime > 0){
                currentShootingTime -= Time.deltaTime;
            }
            if(PlayerMovement.instance.detectionFull || haveShot){
                chasing = target;
            }
            else{
                DetectPlayer();
            }
            if(chasing != null){
                agent.stoppingDistance = stoppingDistance;
                spotLight.color = Color.red;
                if(Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(chasing.transform.position.x,0,chasing.transform.position.z)) <= stoppingDistance){
                    shootingCoolDown = 0.2f;
                    anim.SetBool("StandShoot",true);
                    agent.speed = 0;
                }
                else{
                    shootingCoolDown = 0.5f;
                    anim.SetBool("StandShoot",false);
                    agent.speed = 3;
                }
                agent.SetDestination(chasing.transform.position);
            }
            else if(currentTimeChase < timeToChase && chasing == null){
                currentTimeChase+=Time.deltaTime;
                if(currentTimeChase >= timeToChase){
                    agent.stoppingDistance = 0;
                    spotLight.color = Color.white;
                    agent.speed = 1;
                    agent.SetDestination(PatrolPoints[i].position);
                }
            }
            else if(isPatrol && (transform.position.x == PatrolPoints[i].position.x && transform.position.z == PatrolPoints[i].position.z)){
                StartCoroutine(WaitInIdle());
            }
            else if(isPatrol){
                agent.speed = 1;
            }
            else if(!isPatrol && !waiting){
                i++;
                isPatrol = true;
                if(i >= PatrolPoints.Length){
                    i = 0;
                }
                agent.SetDestination(PatrolPoints[i].position);
            }
            if(chasing != null && Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(chasing.transform.position.x,0,chasing.transform.position.z)) <= shootingDistance && currentShootingTime <= 0){
                currentShootingTime = shootingCoolDown;
                Shoot();
            }
            prev = agent.speed;
        }
    }

    private void SoundPart(){
        if(!movementSource.isPlaying || prev != agent.speed){
            movementSource.Stop();
            movementCooldown = false;
        }
        if(agent.speed == 0){
            movementSource.Stop();
        }
        if(agent.speed == 1 && !movementCooldown){
            movementCooldown = true;
            movementSource.PlayOneShot(walkClip);
        }
        if(agent.speed == 3 && !movementCooldown){
            movementCooldown = true;
            movementSource.PlayOneShot(runClip);
        }
    }
    public void Shoot(){
        RaycastHit hit;
        Vector3 dir = (chasing.transform.position + offset - FirePoint.position).normalized;
        Ray ray = new Ray(FirePoint.position,dir);
        if(chasing != null && Vector3.Dot(dir,transform.forward) > Mathf.Cos(0.5f*detectionAngle*Mathf.Deg2Rad) && Physics.Raycast(ray,out hit)){
            if(!shootingSource.isPlaying)
                shootingSource.PlayOneShot(shootClip);
            muzzleFlash.Play();
            bulletshellVfx.Emit(1);
            GameObject impact = Instantiate(hitImpact,hit.point,Quaternion.identity);
            impact.transform.up = hit.normal;
            Destroy(impact,1);
            if(hit.collider.gameObject.tag == "Player"){
                hit.collider.gameObject.GetComponent<Health>().TakeDamage(damage);
            }            
            if(hit.collider.gameObject.tag == "Vehicle"){
                CarController.instance.vehicleFill.fillAmount -= damage*Time.deltaTime;
            }
        }
    }
    public void DetectPlayer(){
        Collider[] col = Physics.OverlapSphere(transform.position,detectionRadius,layerMask);
        foreach (Collider c in col)
        {
            Vector3 dir = (c.gameObject.transform.position - transform.position);
            dir.y = 0;
            if(Vector3.Distance(transform.position,c.transform.position) <= closeRadius && !PlayerMovement.instance.isCrouch){
                chasing = c.gameObject;
                currentTimeChase = 0;
            }
            else if(dir.magnitude <= detectionRadius){
                if(Vector3.Dot(dir.normalized,transform.forward) > Mathf.Cos(0.5f*detectionAngle*Mathf.Deg2Rad)){
                    Vector3 left = leftCheck.position - transform.position;
                    Vector3 right = rightCheck.position - transform.position;
                    RaycastHit midhit;
                    RaycastHit leftHit;
                    RaycastHit rightHit;
                    Physics.Raycast(transform.position, dir.normalized,out midhit,detectionRadius,obstacle);
                    Physics.Raycast(transform.position, left.normalized, out leftHit, detectionRadius, obstacle);
                    Physics.Raycast(transform.position, right.normalized, out rightHit, detectionRadius, obstacle);
                    if (midhit.collider != null && midhit.collider.tag == "Player")
                    {
                        chasing = c.gameObject;
                        currentTimeChase = 0;
                    }
                    else if (leftHit.collider != null && leftHit.collider.tag == "Player")
                    {
                        chasing = c.gameObject;
                        currentTimeChase = 0;
                    }
                    else if(rightHit.collider != null && rightHit.collider.tag == "Player")
                    {
                        chasing = c.gameObject;
                        currentTimeChase = 0;
                    }
                }
            }
            else{
                if(chasing != null){
                    chasing = null;
                }
            }
        }
    }
    IEnumerator WaitInIdle(){
        agent.speed = 0;
        waiting = true;
        isPatrol = false;
        yield return new WaitForSeconds(waitingTime);
        waiting = false;
    }
}
