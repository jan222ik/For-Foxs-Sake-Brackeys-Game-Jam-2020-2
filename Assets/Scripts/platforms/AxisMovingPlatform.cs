﻿using System;
using System.Collections.Generic;
using UnityEngine;


// ReSharper disable once CheckNamespace
public class AxisMovingPlatform : MonoBehaviour
{
    [SerializeField] private float transitionSpeed = 50f;
    [SerializeField] private Type movementType = Type.LERP;

    [SerializeField]
    private bool movePositiveX;
    [SerializeField]
    private bool movePositiveY;
    
    [SerializeField]
    private int moveRangeInTilesToPositiveX;
    [SerializeField]
    private int moveRangeInTilesToNegativeX;
    
    [SerializeField]
    private int moveRangeInTilesToPositiveY;
    [SerializeField]
    private int moveRangeInTilesToNegativeY;

    private float maxX, minX, maxY, minY;
    private bool invertX, invertY;

    public bool freezeMovement;
    public bool flipSpriteAtTurnPoint;
    
    [SerializeField]
    private bool freezeAtTurnPoint;

    internal void Start()
    {
        var position = transform.position;
        var positionX = position.x;
        maxX = positionX + moveRangeInTilesToPositiveX;
        minX = positionX - moveRangeInTilesToNegativeX;
        
        var positionY = position.y;
        maxY = positionY + moveRangeInTilesToPositiveY;
        minY = positionY - moveRangeInTilesToNegativeY;
    }

    private void FixedUpdate()
    {
        if (freezeMovement) return;
        var position = transform.position;
        var currentPosX = position.x;
        var currentPosY = position.y;
        float posX, posY;
        const float turnThreshold = 0.05f;
        if (movePositiveX)
        {
            posX = maxX;
            if (Mathf.Abs(maxX - currentPosX) <= turnThreshold)
            {
                invertX = true;
            }
        }
        else
        {
            posX = minX;
            if (Mathf.Abs(minX - currentPosX) <= turnThreshold)
            {
                invertX = true;
            }
        }
        
        if (movePositiveY)
        {
            posY = maxY;
            if (Mathf.Abs(maxY - currentPosY) <= turnThreshold)
            {
                invertY = true;
            }
        }
        else
        {
            posY = minY;
            if (Mathf.Abs(minY - currentPosY) <= turnThreshold)
            {
                invertY = true;
            }
        }

        if (invertX && invertY)
        {
            invertX = invertY = false;
            movePositiveX = !movePositiveX;
            movePositiveY = !movePositiveY;
            if (freezeAtTurnPoint)
            {
                freezeMovement = true;
            }

            if (flipSpriteAtTurnPoint)
            {
                var spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }

        var targetPos = new Vector2(posX, posY);
        switch (movementType)
        {
            case Type.LERP:
                transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * transitionSpeed);
                break;
            case Type.UNIFORM:
                transform.position =
                    Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * transitionSpeed);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    enum Type
    {
        LERP, UNIFORM
    }

}
