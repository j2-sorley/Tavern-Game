using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    // references
    private Animator animator;
    public float health = 20;
    public List<GameObject> models;
    public bool randomiseModel = true;
    public int danceOveride = -1;

    private void Awake()
    {
        animator = GetComponent < Animator>();
        if (danceOveride <= -1 || danceOveride >= 5 )
        {
            int num = Random.Range(0, 5);
            animator.SetFloat("Dance", num);
        }
        else
        {
            animator.SetFloat("Dance", danceOveride);
        }
        if (randomiseModel )
        {
            int num = Random.Range(0, 5);
            models[num].SetActive(true);
        }

        
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        if (health <= 0 )
            {
                Die();
            }
    }

    public void Die()
    {
        Debug.Log("Die");
        animator.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }

}
