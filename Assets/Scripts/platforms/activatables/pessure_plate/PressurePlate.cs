﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : ActivatorBase
{
    private ActivationStateChangeEvent onStateChangeBacking;
    [SerializeField]
    private bool currentState;
    [SerializeField] private bool inverted;
    [SerializeField] private List<GameTagsEnum> input_tags;
    private List<string> tags;
    private int countOfColliders;

    private Vector3 initPos;
    private Vector3 initScale;
    private Vector3 shiftedPos;
    private Vector3 shiftedScale;
    
    private void Awake()
    {
        currentState = inverted;
        if (onStateChangeBacking == null)
        {
            onStateChangeBacking = new ActivationStateChangeEvent();
        }
        tags = new List<string>();
        input_tags.ForEach(it => tags.Add(GameTags.of(it)));
        initPos = transform.position;
        initScale = transform.localScale;
        shiftedPos = new Vector3(initPos.x, initPos.y + initPos.y / 2, initPos.z);
        shiftedScale = new Vector3(initScale.x, initScale.y / 2, initScale.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTagInList(other.tag)) return;
        countOfColliders++;
        if (countOfColliders == 1)
        {
            currentState = true;
            onStateChangeBacking.Invoke(true);   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isTagInList(other.tag)) return;
        countOfColliders--;
        if (countOfColliders <= 0)
        {
            currentState = false;
            onStateChangeBacking.Invoke(false);   
        }
    }

    private void FixedUpdate()
    {
        return;
        if (currentState)
        {
            transform.localScale = shiftedScale;
            transform.localPosition = shiftedPos;
        }
        else
        {
            transform.localScale = initScale;
            transform.localPosition = initPos;
        }
    }

    private bool isTagInList(string it) => tags.Contains(it);

    public override ActivationStateChangeEvent onStateChange => onStateChangeBacking;
    public override bool getCurrent() => currentState;
}