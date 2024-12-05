using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] GameObject shoot_sound;
    [SerializeField] float shoot_interval = 1f;
    [SerializeField] float range = 25f;
    [SerializeField] Transform top_mesh;
    [SerializeField] public bool buffed;
    bool previous_buffed;
    [SerializeField] GameObject buff_sound;
    [SerializeField] GameObject debuff_sound;
    ParticleSystem[] buffParticles;
    Light buffLight;
    string buffEffect_name = "FantasyEffect";
    float last_shoot;
    Transform target;
    
    void Start()
    {
        //last_shoot= 0f;
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
        if      (previous_buffed == false && buffed == true) Instantiate(buff_sound, transform.position, Quaternion.identity);
        //else if (previous_buffed == true && buffed == false) Instantiate(debuff_sound, transform.position, Quaternion.identity);
        previous_buffed = buffed;

        FindClosestTarget();
        AimWeapon();
        CheckBuff(buffed);
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
        //if (isActive && Time.time - last_shoot >= shoot_interval && top_mesh.gameObject.activeSelf)
        //{
        //    Instantiate(shoot_sound, transform.position, Quaternion.identity);
        //    last_shoot = Time.time; // Update the last shoot time
        //}
    }
    void CheckBuff(bool state)
    {
        foreach(var buff_effect in buffParticles)
        {
            var buff_emission = buff_effect.emission;
            buff_emission.enabled = state;
        }
        buffLight.enabled = state;
        var shoot_effect = projectileParticles.emission;
        if (state) shoot_effect.rateOverTime = 2f;
        else       shoot_effect.rateOverTime = 0.75f;
    }
}
