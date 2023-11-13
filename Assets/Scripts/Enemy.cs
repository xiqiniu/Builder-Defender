using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float lookForTargetTimer;
    private float lookForTargetTimerMax=0.2f;
    private HealthSystem healthSystem;
    public static Enemy Create(Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy,position,Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    private Transform targetTransform;
    private Rigidbody2D rbody2D;
    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        rbody2D = GetComponent<Rigidbody2D>();
        if(BuildingManager.Instance.GetHQbuilding()!=null)
        targetTransform=BuildingManager.Instance.GetHQbuilding().transform;
        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(2f,0.05f);
        ChromaticAberrationEffect.Instance.SetWeight(0.5f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(this.gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(7f,0.15f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building=collision.gameObject.GetComponent<Building>();
        if(building!=null)
        {
            //collided with a building
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(999);
        }
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            float moveSpeed = 6f;
            rbody2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            rbody2D.velocity = Vector2.zero;
        }
    }
     private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }
    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position,targetMaxRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                //is a building
                if (targetTransform == null)
                    targetTransform = building.transform;
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position)
                        < Vector3.Distance(transform.position, targetTransform.position))
                        targetTransform = building.transform;
                 }
            }
        }
        if(targetTransform==null)
            if(BuildingManager.Instance.GetHQbuilding()!=null)
            targetTransform= BuildingManager.Instance.GetHQbuilding().transform; 
    }

    
}
