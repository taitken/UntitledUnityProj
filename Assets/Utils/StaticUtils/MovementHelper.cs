using System;
using System.Collections.Generic;
using Characters.Utils;
using GameControllers.Services;
using UnityEngine;

namespace UtilityClasses
{
    public class MovementHelper
    {
        // Moves a given rigid body a given distance along a defined path. Returns final direction sprite is facing.
        public static Vector2 MoveRigidBody2D(Rigidbody2D _rb, Vector2 _distance, float _moveSpeed, IList<Vector3Int> _currentPath, IEnvironmentService _envService)
        {
            Vector2 finalDirection = MovementHelper.MoveObject(_rb, _distance, _moveSpeed, _currentPath, _envService);
            MovementHelper.UpdateSpriteDirection(_rb.gameObject, finalDirection);
            return finalDirection;
        }

        public static CharacterPathLine UpdateMoveLine(CharacterPathLine _currentPathLine, CharacterPathLine.Factory _pathLineFactory, Vector3 _lineStartPos,IList<Vector3Int> _currentPath, IEnvironmentService _envService)
        {
            IList<Vector3> newLinePath = _currentPath.Map(item => { return _envService.CellToLocal(item); });
            newLinePath.Insert(0, _lineStartPos);
            if (_currentPathLine == null)
            {
                return _pathLineFactory.Create(newLinePath);
            }
            else
            {
                return _currentPathLine.UpdateLine(newLinePath);
            }
        }

        private static Vector2 MoveObject(Rigidbody2D _rb, Vector2 _distance, float _moveSpeed, IList<Vector3Int> _currentPath, IEnvironmentService _envService)
        {
            if (_distance != Vector2.zero)
            {
                Vector3 nextPoint = _envService.CellToLocal(_currentPath[0]);
                Vector2 direction = _rb.gameObject.transform.position.GetDirection(nextPoint);
                Vector2 newPosition = _rb.position + (_distance * direction * _moveSpeed * GameTime.fixedDeltaTime);
                Vector2 overshootDistance = MovementHelper.GetMovementOvershoot(direction, newPosition, nextPoint);
                // Overshot movement location
                if (overshootDistance.x > 0 && direction.y == 0
                    || overshootDistance.y > 0 && direction.x == 0
                    || overshootDistance.y > 0 && overshootDistance.x > 0)
                {
                    _rb.MovePosition(nextPoint);
                    _currentPath.RemoveAt(0);
                    if (_currentPath.Count > 0
                        && (overshootDistance.x + overshootDistance.y) > 0.02f)
                    {
                        return MovementHelper.MoveRigidBody2D(_rb, overshootDistance / _moveSpeed / GameTime.fixedDeltaTime, _moveSpeed, _currentPath, _envService);
                    }
                    else
                    {
                        return direction;
                    }
                }
                // Did not overshoot
                else
                {
                    _rb.MovePosition(newPosition);
                    if ((Vector3)newPosition == nextPoint) _currentPath.RemoveAt(0);
                    return direction;
                }
            }
            return Vector2.zero;
        }
        private static Vector2 GetMovementOvershoot(Vector2 direction, Vector2 newEndPosition, Vector3 currentPathPoint)
        {
            Vector2 overShootMovement = new Vector2(0, 0);
            if (direction.x > 0 && newEndPosition.x > currentPathPoint.x)
            {
                overShootMovement += new Vector2(newEndPosition.x - currentPathPoint.x, 0);
            }
            if (direction.x < 0 && newEndPosition.x < currentPathPoint.x)
            {
                overShootMovement += new Vector2(newEndPosition.x - currentPathPoint.x, 0);
            }
            if (direction.y > 0 && newEndPosition.y > currentPathPoint.y)
            {
                overShootMovement += new Vector2(0, newEndPosition.y - currentPathPoint.y);
            }
            if (direction.y < 0 && newEndPosition.y < currentPathPoint.y)
            {
                overShootMovement += new Vector2(0, newEndPosition.y - currentPathPoint.y);
            }
            return new Vector2(Math.Abs(overShootMovement.x), Math.Abs(overShootMovement.y));
        }


        private static void UpdateSpriteDirection(GameObject gameObject, Vector2 movement)
        {
            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                if (movement.x > 0)
                {
                    MovementHelper.SetSpriteDirection(gameObject, false, false, new Vector3(0, 0, -55));
                }
                else if (movement.x < 0)
                {
                    MovementHelper.SetSpriteDirection(gameObject, true, false, new Vector3(0, 0, 65));
                }
                else if (movement.y > 0)
                {
                    MovementHelper.SetSpriteDirection(gameObject, false, false, new Vector3(0, 0, 0));
                }
                else if (movement.y < 0)
                {
                    MovementHelper.SetSpriteDirection(gameObject, false, true, new Vector3(0, 0, 0));
                }
            }
        }

        private static void SetSpriteDirection(GameObject gameObject, bool flipX, bool flipY, Vector3 angle)
        {
            gameObject.transform.eulerAngles = angle;
            gameObject.GetComponent<SpriteRenderer>().flipX = flipX;
            gameObject.GetComponent<SpriteRenderer>().flipY = flipY;
            gameObject.GetComponentsInChildren<SpriteRenderer>().ForEach(sprite =>
            {
                sprite.flipX = flipX;
                sprite.flipY = flipY;
            });
        }
    }
}