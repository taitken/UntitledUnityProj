using System;
using System.Collections.Generic;
using Characters.Utils;
using GameControllers.Services;
using UnityEngine;
using Zenject;

namespace UtilityClasses
{
    public class MovementHelper : MonoBehaviour
    {
        private IEnvironmentService envService { get; set; }
        private CharacterPathLine.Factory pathLineFactory { get; set; }
        private IList<ObjectMovement> objectMovements { get; set; }
        private IList<ObjectMovement> movementsToRemove { get; set; }

        [Inject]
        public void Construct(IEnvironmentService _environmentService,
                                CharacterPathLine.Factory _pathLineFactory)
        {
            this.envService = _environmentService;
            this.objectMovements = new List<ObjectMovement>();
            this.movementsToRemove = new List<ObjectMovement>();
            this.pathLineFactory = _pathLineFactory;
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < this.objectMovements.Count; i++)
            {
                ObjectMovement objMove = this.objectMovements[i];
                if (objMove.movePath != null && objMove.movePath.Count > 0 && objMove.transform != null)
                {
                    objMove.finalDirection = this.MoveObjectTransform(objMove.transform, new Vector2(1, 1), objMove.moveSpeed, objMove.movePath, objMove.updateSpriteDirection);
                    if (objMove.showPathingLine) objMove.pathLine = this.GetCharacterPathLine(objMove);
                } 
                else
                {
                    this.movementsToRemove.Add(objMove);
                }
            }
            this.movementsToRemove.ForEach(objMove =>
            {
                objMove.onMovementFinished.Emit();
                if (objMove.pathLine != null) objMove.pathLine.Destroy();
                this.objectMovements.Remove(objMove);
            });
            this.movementsToRemove.Clear();
        }

        public ObjectMovement MoveObject(Transform _transform, Vector2 _distance, float _moveSpeed, IList<Vector3> _movePath, bool _updateSpriteDirection = true, bool showPathingLine = true)
        {
            ObjectMovement existingMovementForObject = this.objectMovements.Find(objectMovement =>{return objectMovement.transform == _transform;});
            if(existingMovementForObject != null) this.objectMovements.Remove(existingMovementForObject);
            ObjectMovement newObjMove = new ObjectMovement(_transform, _movePath, _moveSpeed, _updateSpriteDirection, showPathingLine);
            this.objectMovements.Add(newObjMove);
            return newObjMove;
        }
        private Vector2 MoveObjectTransform(Transform _transform, Vector2 _distance, float _moveSpeed, IList<Vector3> _currentPath, bool _updateSpriteDirection = true)
        {
            Vector2 finalDirection = this.UpdateObjectPosition(_transform, _distance, _moveSpeed, _currentPath, _updateSpriteDirection);
            if (_updateSpriteDirection) this.UpdateSpriteDirection(_transform.gameObject, finalDirection);
            return finalDirection;
        }

        private CharacterPathLine GetCharacterPathLine(ObjectMovement objMove)
        {
            IList<Vector3> newLinePath = objMove.movePath.Map(item => { return item; });
            newLinePath.Insert(0, objMove.transform.position);
            if (objMove.pathLine == null)
            {
                return this.pathLineFactory.Create(newLinePath);
            }
            else
            {
                return objMove.pathLine.UpdateLine(newLinePath);
            }
        }

        public Vector2 GetDirection(Vector3 origin, Vector3 destination)
        {
            float x = 0;
            float y = 0;
            if (origin.x < destination.x)
            {
                x = 1;
            }
            if (origin.x > destination.x)
            {
                x = -1;
            }
            if (origin.y < destination.y)
            {
                y = 1;
            }
            if (origin.y > destination.y)
            {
                y = -1;
            }
            return new Vector2(x, y);
        }

        private Vector2 UpdateObjectPosition(Transform _transform, Vector2 _distance, float _moveSpeed, IList<Vector3> _currentPath, bool _updateSpriteDirection)
        {
            if (_distance != Vector2.zero)
            {
                Vector3 nextPoint = _currentPath[0];
                Vector2 direction = this.GetDirection(_transform.localPosition, nextPoint);
                Vector2 newPosition = (Vector2)_transform.localPosition + (_distance * direction * _moveSpeed * GameTime.fixedDeltaTime);
                Vector2 overshootDistance = this.GetMovementOvershoot(direction, newPosition, nextPoint);
                // Overshot movement location
                if (overshootDistance.x > 0 && direction.y == 0
                    || overshootDistance.y > 0 && direction.x == 0
                    || overshootDistance.y > 0 && overshootDistance.x > 0)
                {
                    _transform.localPosition = nextPoint;
                    _currentPath.RemoveAt(0);
                    if (_currentPath.Count == 0 || _currentPath == null) this.movementsToRemove.Add(this.objectMovements.Find(movement => { return movement.transform == _transform; }));
                    if (_currentPath.Count > 0
                        && (overshootDistance.x + overshootDistance.y) > 0.02f)
                    {
                        return this.MoveObjectTransform(_transform, overshootDistance / _moveSpeed / GameTime.fixedDeltaTime, _moveSpeed, _currentPath, _updateSpriteDirection);
                    }
                    else
                    {
                        return direction;
                    }
                }
                // Did not overshoot
                else
                {
                    _transform.localPosition = newPosition;
                    if ((Vector3)newPosition == nextPoint) _currentPath.RemoveAt(0);
                    if (_currentPath.Count == 0 || _currentPath == null) this.movementsToRemove.Add(this.objectMovements.Find(movement => { return movement.transform == _transform; }));
                    return direction;
                }
            }
            return Vector2.zero;
        }
        private Vector2 GetMovementOvershoot(Vector2 direction, Vector2 newEndPosition, Vector3 currentPathPoint)
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


        public void UpdateSpriteDirection(GameObject gameObject, Vector2 movement)
        {
            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                if (movement.x > 0)
                {
                    this.SetSpriteDirection(gameObject, false, false, new Vector3(0, 0, -55));
                }
                else if (movement.x < 0)
                {
                    this.SetSpriteDirection(gameObject, true, false, new Vector3(0, 0, 65));
                }
                else if (movement.y > 0)
                {
                    this.SetSpriteDirection(gameObject, false, false, new Vector3(0, 0, 0));
                }
                else if (movement.y < 0)
                {
                    this.SetSpriteDirection(gameObject, false, true, new Vector3(0, 0, 0));
                }
            }
        }

        private void SetSpriteDirection(GameObject gameObject, bool flipX, bool flipY, Vector3 angle)
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