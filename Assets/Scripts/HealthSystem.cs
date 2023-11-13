using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthAmountMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    public event EventHandler OnHealed;
    [SerializeField]private int healthAmountMax=100;
    private int healthAmount;
    
    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if(isDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public bool isDead()
    {
        return healthAmount <= 0;
    }
    
    public int GetHealthAmount()
    {
        return healthAmount;
    }    
    public int GetHealthAmountMax()
    {
        return healthAmountMax;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }

    public bool isFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    public void SetHealthAmountMax(int healthAmountMax,bool updateHealthAmount=true)
    {
        this.healthAmountMax = healthAmountMax;
        if(updateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }
        OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healAmount,0,healthAmountMax);
        OnHealed?.Invoke(this, EventArgs.Empty);
     }

    public void HealFull()
    {
        healthAmount = healthAmountMax;
        OnHealed?.Invoke(this, EventArgs.Empty);
    }
}
