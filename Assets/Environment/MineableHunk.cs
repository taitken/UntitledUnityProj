using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

namespace Environment
{
    public class MineableHunk : MonoBehaviour2
    {
        private IUnitActionService actionService;
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;
        private eMouseAction mouseAction;

        [Inject]
        public void Construct(IUnitActionService _actionService, Vector3 position)
        {
            this.transform.position = position;
            this.actionService = _actionService;
            this.subscriptions.Add(this.actionService.mouseAction.Subscribe(action => { this.mouseAction = action; }));
        }
        void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
        }

        public override void OnClickedByUser()
        {
            if (this.mouseAction == eMouseAction.Dig)
            {
                this.Destroy();
            }
        }

        public void updateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

        public class Factory : PlaceholderFactory<Vector3,  MineableHunk>
        {
        }
    }
}