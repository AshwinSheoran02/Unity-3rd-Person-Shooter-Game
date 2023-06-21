using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ActivateRagdoll : MonoBehaviour
{
    List<Collider>ragdollColliders = new List<Collider>();
    public NavMeshAgent agent;
    public Enemy enemy;
    Animator anim;
    Rigidbody rb;
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        SetRagdollParts();
        if(GetComponent<NavMeshAgent>() != null){
            agent = GetComponent<NavMeshAgent>();
        }
        if(GetComponent<CharacterController>() != null){
            controller = GetComponent<CharacterController>();
        }
        if(GetComponent<Enemy>() != null)
            enemy = GetComponent<Enemy>();
    }

    void SetRagdollParts(){
        foreach(Collider col in GetComponentsInChildren<Collider>()){
            if(col.gameObject == this.gameObject){
                continue;
            }
            col.isTrigger = true;
            ragdollColliders.Add(col);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableRagDoll(){
        if(agent != null){
            agent.enabled = false;
        }
        else{
            controller.enabled = false;
        }
        if(enemy != null){
            enemy.enabled = false;

        }
        anim.enabled = false;
        anim.avatar = null;
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        if(GetComponent<CapsuleCollider>() != null)
            GetComponent<CapsuleCollider>().enabled = false;
        foreach(Collider col in ragdollColliders){
            col.isTrigger = false;
            col.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}