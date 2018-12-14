﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add maximum projectile distance (or damage dropoff rate)

public class Projectile : ExtendedMonoBehaviour
{
    public float MAX_PROJECTILE_DISTANCE = 100f;
    public float MAX_PROJECTILE_LIFETIME = 10f;
    public ProjectileData Data;

    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        if (Data == null) Data = new ProjectileData();

        origin = transform.position;

        // Projectiles can have limited lifetime (unusual)
        float lifetimeCap = Data.HasLifetime ? Mathf.Min(Data.MaxLifetime, MAX_PROJECTILE_LIFETIME) : MAX_PROJECTILE_LIFETIME;
        Wait(lifetimeCap, () =>
        {
            DestroyProjectile(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        // Projectiles can have limited range (semi-unusual)
        float distanceCap = Data.HasMaxRange ? Mathf.Min(Data.MaxRange, MAX_PROJECTILE_DISTANCE) : MAX_PROJECTILE_DISTANCE;
        float distanceTravelled = Vector3.Distance(origin, transform.position);

        if (distanceTravelled > distanceCap)
        {
            DestroyProjectile(false);
            return;
        }

        if (GameManager.Instance.DebugMode)
        {
            Debug.DrawLine(origin, transform.position, Color.blue);
            Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        }

        transform.Translate(Vector3.forward * Data.Speed * Time.deltaTime);
    }

    public void DestroyProjectile(bool ShowEffect = false)
    {
        // Display effect when projectile is destroyed
        if (ShowEffect && Data.DestroyEffect != null)
        {
            Instantiate(Data.DestroyEffect, transform.position, Quaternion.identity);
        }

        // TODO: Play sound effect

        Destroy(gameObject);
    }
}