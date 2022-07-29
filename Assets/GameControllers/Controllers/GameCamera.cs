using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameControllers
{
    public class GameCamera : MonoBehaviour
    {
        private const float DRAG_SPEED = 3.5f;
        private const float ZOOM_SPEED = .1f;
        private const float MIN_ZOOM = .5f;
        private const float MAX_ZOOM = 5f;
        private Vector3 dragOrigin;
        private Vector3 dragLastFrame;
        private float dragDistance;
        private Camera cameraControl;
        // Start is called before the first frame update
        void Start()
        {
            this.cameraControl = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            this.Drag();
            this.Zoom();
        }

        void Drag()
        {
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                dragOrigin = Mouse.current.position.ReadValue();
                this.dragLastFrame = Mouse.current.position.ReadValue();
                return;
            }

            if (Mouse.current.middleButton.isPressed)
            {
                Vector3 dragThisFrame = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
                Vector3 pos = Camera.main.ScreenToViewportPoint(dragThisFrame - dragLastFrame);
                Vector3 move = new Vector3((pos.x * DRAG_SPEED * ((float)this.GetZoomCoefficient())) * -1, (pos.y * DRAG_SPEED * ((float)this.GetZoomCoefficient())) * -1, 0);
                this.transform.Translate(move);
                this.dragLastFrame = dragThisFrame;
            }
            if (Mouse.current.middleButton.wasReleasedThisFrame)
            {
                dragOrigin = Mouse.current.position.ReadValue();
                return;
            }
        }

        private double GetZoomCoefficient()
        {
            return this.cameraControl.orthographicSize / 1.5;
        }

        void Zoom()
        {
            if (Mouse.current.scroll.ReadValue().y > 0)
            {
                this.cameraControl.orthographicSize = Math.Max(this.cameraControl.orthographicSize - ZOOM_SPEED, MIN_ZOOM);
            }
            if (Mouse.current.scroll.ReadValue().y < 0)
            {
                this.cameraControl.orthographicSize = Math.Min(this.cameraControl.orthographicSize + ZOOM_SPEED, MAX_ZOOM);
            }
        }
    }
}