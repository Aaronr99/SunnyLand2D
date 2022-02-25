using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    // Se guardan las layers importantes de manera estatica
    public static LayerMask groundLayer = 1 << 6;
    public static LayerMask enemyLayer = 1 << 8;
}
