
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
        protected IUnitOrderService orderService;
        protected IPathFinderService pathFinderService;
        protected IEnvironmentService environmentService;
        protected PathFinderMap pathFindMap;
        protected CharacterPathLine.Factory pathLineFactory;
        protected CharacterPathLine pathingLine;
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;
        protected Animator animator;
        public ContactFilter2D movementFilter = new ContactFilter2D();
        protected List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        public UnitModel unitModel { get; set; }
        public virtual float collisionOffset { get; set; } = 0.00f;
        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IPathFinderService _pathFinderService,
                              IEnvironmentService _environmentService,
                              CharacterPathLine.Factory _pathLineFactory,
                              UnitModel _unitModel
        )
        {
            this.orderService = _orderService;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.pathLineFactory = _pathLineFactory;
            this.unitModel = _unitModel;
            this.subscriptions.Add(this.orderService.orders.Subscribe(orders =>
            {
                if (this.unitModel.currentOrder == null && this.unitModel.currentPath != null) this.CancelMoving();
            }));
            this.movementFilter.SetLayerMask(LayerMask.GetMask("MineableLayer"));
            this.pathFinderService.pathFinderMap.Subscribe(map =>
            {
                this.pathFindMap = map;
            });
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
        }

        void Start()
        {

        }

        void Update()
        {
            this.unitModel.position = this.gameObject.transform.position;
        }

        protected void FixedUpdate()
        {
            if (this.unitModel.currentPath != null && this.unitModel.currentPath.Count > 0)
            {
                Vector3 nextPoint = this.environmentService.CellToLocal(this.unitModel.currentPath[0]);
                Vector2 direction = this.gameObject.transform.position.GetDirection(nextPoint);
                if (direction == Vector2.zero)
                {
                    this.unitModel.currentPath.RemoveAt(0);
                }
                else
                {
                    this.moveUnit(direction);
                }
                IList<Vector3> newLinePath = this.unitModel.currentPath.Map(item => { return this.environmentService.CellToLocal(item); });
                newLinePath.Insert(0, this.gameObject.transform.position);
                if (this.pathingLine == null)
                {
                    this.pathingLine = this.createMovePath(newLinePath);
                }
                else
                {
                    this.pathingLine.UpdateLine(newLinePath);
                }
            }
        }

        protected void CancelMoving()
        {
            this.unitModel.currentPath = null;
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
                rb.MovePosition(rb.position + new Vector2(horizontalCollison ? 0 : movement.x, verticalCollison ? 0 : movement.y) * this.unitModel.moveSpeed * Time.fixedDeltaTime);
            }
            this.animator.SetBool("isMoving", movement != Vector2.zero);
        }

        protected bool collisionCheck(Vector2 movement)
        {
            int count = Physics2D.Raycast(
                this.transform.position,
                movement,
                movementFilter,
                castCollisions,
                this.unitModel.moveSpeed * (Time.fixedDeltaTime * 1f) + collisionOffset
            );
            return count != 0;
        }

        public class Factory : PlaceholderFactory<UnitModel, WorldCharacter>
        {
        }
    }
}
