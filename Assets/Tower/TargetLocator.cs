using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float shoot_range = 20f;
    [SerializeField] float range;
    [SerializeField] float shoot_rate = 0.75f;
    [SerializeField] float shoot_speed = 40f;
    [SerializeField] Transform top_mesh;
    Transform        target;

    [SerializeField] GameObject   buff_sound;
    [SerializeField] public bool  buffed;
    [SerializeField] public float buff_factor = 1.5f;
    bool             previous_buffed;
    ParticleSystem[] buffParticles;
    Light            buffLight;
    string           buffEffect_name = "FantasyEffect";
    
    void Start()
    {
        top_mesh = transform.Find("BallistaTopMesh");
        Transform fantasy_effect = transform.Find(buffEffect_name);
        if (fantasy_effect != null)
        {
            buffParticles = fantasy_effect.GetComponentsInChildren<ParticleSystem>();
            buffLight= fantasy_effect.GetComponentInChildren<Light>();
        }
        buffed = false;
        previous_buffed = false;
    }
    void Update()
    {
        if (previous_buffed == false && buffed == true) Instantiate(buff_sound, transform.position, Quaternion.identity);
        previous_buffed = buffed;
        CheckBuff();
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    void AimWeapon() 
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(target);

        if (targetDistance < range)
        {
            Attack(true); 
        }
        else Attack(false);
    }
    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
    void CheckBuff()
    {
        foreach(var buff_effect in buffParticles)
        {
            var buff_emission = buff_effect.emission;
            buff_emission.enabled = buffed;
        }
        buffLight.enabled = buffed;
        var shoot_effect = projectileParticles.emission;
        if (buffed) 
        {
            shoot_effect.rateOverTime      = shoot_rate  * buff_factor;
            projectileParticles.startSpeed = shoot_speed * buff_factor;
            range                          = shoot_range * buff_factor;
        }
        else
        {
            shoot_effect.rateOverTime      = shoot_rate;
            projectileParticles.startSpeed = shoot_speed;
            range                          = shoot_range;
        }
    }
}
