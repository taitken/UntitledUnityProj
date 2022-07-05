
using System;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Characters.Utils;
using Zenject;

namespace Characters
{
    public class WorldCharacter : MonoBehaviour2
    {
        protected UnitOrderModel currentOrder;
        protected IUnitActionService actionService;
        protected IPathFinderService pathFinderService;
        protected IEnvironmentService environmentService;
        protected PathFinderMap pathFindMap;
        protected IList<Vector3Int> currentPath;
        protected CharacterPathLine.Factory pathLineFactory;
        protected CharacterPathLine pathingLine;
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;
        protected Animator animator;
        public ContactFilter2D movementFilter;
        protected List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        public virtual float moveSpeed { get; set; } = 1f;
        public virtual float collisionOffset { get; set; } = 0.05f;
        [Inject]
        public void Construct(IUnitActionService _actionService,
                              IPathFinderService _pathFinderService,
                              IEnvironmentService _environmentService,
                              CharacterPathLine.Factory _pathLineFactory
        )
        {
            this.actionService = _actionService;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.pathLineFactory = _pathLineFactory;
            this.subscriptions.Add(this.actionService.orders.Subscribe(orders =>
            {
                this.currentOrder = orders.Find(order => { return order.ID == this.currentOrder?.ID; });
                if (this.currentOrder == null && this.currentPath != null) this.CancelMoving();
            }));
            this.pathFinderService.pathFinderMap.Subscribe(map =>
            {
                this.pathFindMap = map;
            });
            InvokeRepeating("CheckAndAssignOrder", 2.0f, 2.0f);
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        protected void FixedUpdate()
        {
            if (this.currentPath != null && this.currentPath.Count > 0)
            {
                Vector3 nextPoint = this.environmentService.CellToLocal(this.currentPath[0]);
                Vector2 direction = this.getDirection(this.gameObject.transform.position, nextPoint);
                if (direction == Vector2.zero)
                {
                    this.currentPath.RemoveAt(0);
                }
                else
                {
                    this.moveUnit(direction);
                    IList<Vector3> newLinePath = this.currentPath.Map(item => { return this.environmentService.CellToLocal(item); });
                    newLinePath.Insert(0, this.gameObject.transform.position);
                    this.pathingLine.UpdateLine(newLinePath);
                }
            }
        }

        private Vector2 getDirection(Vector3 startingPoint, Vector3 endPoint)
        {
            float x = 0;
            float y = 0;
            if (startingPoint.x < endPoint.x)
            {
                x = 1;
            }
            if (startingPoint.x > endPoint.x)
            {
                x = -1;
            }
            if (startingPoint.y < endPoint.y)
            {
                y = 1;
            }
            if (startingPoint.y > endPoint.y)
            {
                y = -1;
            }
            if (Math.Abs(startingPoint.x - endPoint.x) < 0.04f)
            {
                x = 0;
            }
            if (Math.Abs(startingPoint.y - endPoint.y) < 0.04f)
            {
                y = 0;
            }
            return new Vector2(x, y);
        }

        public bool TryMoveTo(Vector3Int endPos)
        {
            this.currentPath = this.pathFinderService.FindPath(this.environmentService.LocalToCell(this.gameObject.transform.position), endPos, this.pathFindMap, true);
            if (this.currentPath != null)
            {
                IList<Vector3> vec3Path = this.currentPath.Map(pathStep => { return this.environmentService.CellToLocal(pathStep); });
                vec3Path[0] = this.gameObject.transform.position;
                this.pathingLine = this.createMovePath(vec3Path);
            }
            return this.currentPath != null;
        }

        protected void CancelMoving()
        {
            this.currentPath = null;
            this.pathingLine.Destroy();
        }

        private CharacterPathLine createMovePath(IList<Vector3> path)
        {
            return this.pathLineFactory.Create(path);
        }

        protected void moveUnit(Vector2 movement)
        {
            if (movement != Vector2.zero)
            {
                bool horizontalCollison = this.collisionCheck(new Vector2(movement.x, 0));
                bool verticalCollison = this.collisionCheck(new Vector2(0, movement.y));
                rb.MovePosition(rb.position + new Vector2(horizontalCollison ? 0 : movement.x, verticalCollison ? 0 : movement.y) * moveSpeed * Time.fixedDeltaTime);
                if (!horizontalCollison || !verticalCollison)
                {
                }
            }
            this.animator.SetBool("isMoving", movement != Vector2.zero);
        }

        protected bool collisionCheck(Vector2 movement)
        {
            int count = rb.Cast(
                movement,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );
            return count != 0;
        }

        void CheckAndAssignOrder()
        {
            if (this.currentOrder == null)
            {
                this.currentOrder = this.actionService.GetNextOrder();
                if (this.currentOrder != null)
                {
                    Debug.Log("Order assigned");
                    if (this.TryMoveTo(this.currentOrder.coordinates))
                    {
                        // Success
                    }
                    else
                    {
                        this.currentOrder = null;
                    }
                }
                else
                {
                    //Debug.Log("No orders to assign");
                }
            }
            else
            {
                //Debug.Log("Order already assigned");
            }
        }
    }
}
