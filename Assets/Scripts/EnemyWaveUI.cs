using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    private Camera mainCamera;
    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private RectTransform enemyWaveSpawnIndicator;
    private RectTransform enemyClosestPositionIndicator;
    private void Awake()
    {
        mainCamera = Camera.main;
        waveNumberText=transform.Find("WaveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText= transform.Find("WaveMessageText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnIndicator= transform.Find("EnemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("EnemyClosestPositionIndicator").GetComponent<RectTransform>();
    }
    private void Update()
    {
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestIndicator();
    }
    private void Start()
    {
        enemyWaveManager.onEnemyWaveChanged += EnemyWaveManager_onEnemyWaveChanged;
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }
    
    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetWaveMessageText("");
        }
        else
        {
            SetWaveMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
        }
    }
    private void HandleEnemyWaveSpawnPositionIndicator()
    {
        Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
        enemyWaveSpawnIndicator.anchoredPosition = dirToNextSpawnPosition * 300f;
        enemyWaveSpawnIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));
        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);
        enemyWaveSpawnIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }
    private void HandleEnemyClosestIndicator()
    {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);
        Enemy targetEnemy = null;
        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
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
        if (targetEnemy != null)
        {
            enemyClosestPositionIndicator.gameObject.SetActive(true);
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
            enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 250f;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));
            float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
            enemyWaveSpawnIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        }
        else
        {
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }
    private void EnemyWaveManager_onEnemyWaveChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void SetWaveMessageText(string message)
    {
        waveMessageText.SetText(message);
    }
    private void SetWaveNumberText(string message)
    {
        waveNumberText.SetText(message);
    }
}
