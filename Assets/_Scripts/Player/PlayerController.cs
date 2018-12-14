﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : ExtendedMonoBehaviour
{
    [SerializeField] private CharacterMovementSettings Movement;
    [SerializeField] private CharacterMouseSettings Mouse;
    [SerializeField] private Gun gun;

    private Player player;
    private Vector3 velocity;
    private Vector3 mousePoint;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Rotate player to match mouse target
        // NOTE: May need to be updated if more than one level (height)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        // Player should always look at mouse point
        if (ground.Raycast(ray, out rayDistance))
        {
            mousePoint = ray.GetPoint(rayDistance);
            player.LookAt(mousePoint);

            if (GameManager.Instance.DebugMode)
            {
                Debug.DrawLine(ray.origin, mousePoint, Color.yellow);
            }
        }

        // Player movement
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        velocity = Movement.GetTargetVelocity(moveInput.normalized);
        player.Move(velocity);

        // Fire weapon
        // TODO: Move to weapon class? Needs to know if held though
        if (Input.GetMouseButtonDown(0) && gun != null)
        {
            gun.Fire();
        }

        // Reload gun
        if (Input.GetKeyDown(KeyCode.R))
        {
            gun.Reload();
        }
    }

    private void OnDrawGizmos()
    {
        if (!GameManager.Instance.DebugMode) return;

        // Show mouse position
        Gizmos.color = new Color(239, 193, 186);
        Gizmos.DrawSphere(mousePoint, 0.15f);
    }
}
