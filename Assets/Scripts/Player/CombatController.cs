using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    //Components
    AudioSource audioSource;
    Animator animator;
    Transform cameraTransform;

    [Header("Combat Stats")]
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private LayerMask attackLayer;

    // VFX and Sounds
    public GameObject hitEffect;
    public AudioClip attackSound;
    public AudioClip hitSound;

    [SerializeField] private bool attacking = false;
    [SerializeField] private bool readyToAttack = true;
    int attackCount;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Camera _camera = GetComponentInChildren<Camera>();
        cameraTransform = _camera.gameObject.transform;
        audioSource = GetComponent<AudioSource>();
    }
    public void Attack()
    {

        // Checks
        if (!readyToAttack || attacking) return;
        Debug.Log("Attack");
        readyToAttack = false;
        attacking = true;

        //Invoke Actions
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        //Sounds
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(attackSound);

        attackCount++;
        StartAnimation("Attack", (attackCount % 2));
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, attackDistance))
        {
            HitTarget(hit.point);
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.TryGetComponent<NPCController>(out NPCController npc))
            {
                Debug.Log("NPC found");
                npc.TakeHit(attackDamage);
            }
        }
    }

    public void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject VFX = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(VFX, 10f);
    }

    public void ResetAttack()
    {
        readyToAttack = true;
        attacking = false;
    }

    public void StartAnimation(string triggerName, int punchIndex)
    {
        animator.SetFloat("PunchIndex", punchIndex);
        animator.SetTrigger(triggerName);
    }
}
