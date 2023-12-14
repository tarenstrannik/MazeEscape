using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanDie
{
    public bool IsDead
    {
        get;
    }

    public void Die();
}
