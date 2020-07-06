using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackData : ScriptableObject {
    public float attackCooldown;
    public float followupCount;
}