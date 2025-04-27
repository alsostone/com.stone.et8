using System;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner instance { get; private set; }
    
    public DamageNumber DamageNumber;
    public DamageNumber HealNumber;
    public DamageNumber ExpNumber;
    public DamageNumber TextNumber;

    public List<string> texts;
    
    private void Awake()
    {
        instance = this;
    }

    public void FloatingDamageNumber(float damage, Vector3 position)
    {
        DamageNumber.Spawn(position, damage);
    }
    
    public void FloatingHealNumber(float value, Vector3 position)
    {
        DamageNumber.Spawn(position, value);
    }
    
    public void FloatingExpNumber(float value, Vector3 position, Transform target)
    {
        var number = DamageNumber.Spawn(position, value);
        number.enableFollowing = true;
        number.followedTarget = target;
    }
    
    public void FloatingTextNumber(int value, Vector3 position)
    {
        var number = DamageNumber.Spawn(position);
        number.leftText = this.texts[Math.Clamp(value, 0, this.texts.Count - 1)];
    }
}
