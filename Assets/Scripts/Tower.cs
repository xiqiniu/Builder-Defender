using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Enemy targetEnemy;

    //����
    private float shootTimer;
    [SerializeField] private float shootTimerMax;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;
    private Vector3 projectileSpawnPosition;
    private void Awake()
    {
        projectileSpawnPosition= transform.Find("projectileSpawnPosition").position;
    }
    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }
    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer<0)
        {
            shootTimer += shootTimerMax;

            if(targetEnemy!=null)
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
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
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
           Enemy enemy= collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                //is a enemy
                if (targetEnemy == null)
                    targetEnemy = enemy;
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position)
                        < Vector3.Distance(transform.position, targetEnemy.transform.position))
                        targetEnemy = enemy;
                }
            }
        }
       
    }
}
