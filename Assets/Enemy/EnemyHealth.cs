using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject dieVFX_SFX;
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] float difficultyRamp = 1;

    public int currentHitPoints = 0;
    Enemy enemy;
    

    void OnEnable()
    {
        currentHitPoints = maxHitPoints;        
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnParticleCollision(GameObject other) 
    {
        ProcessHit();
    }

    void ProcessHit() 
    {
        currentHitPoints--;

        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints = (int)Mathf.Ceil(maxHitPoints * difficultyRamp);
            enemy.RewardGold();
            Instantiate(dieVFX_SFX, transform.position, Quaternion.identity);
        }

        Instantiate(hitVFX,transform.position, Quaternion.identity);
    }
}
