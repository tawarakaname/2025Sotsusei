﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TriggerBase : MonoBehaviour
{
    [SerializeField] protected UnityEvent action;
}
