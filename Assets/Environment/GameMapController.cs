using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Zenject;


namespace Environment
{
    public class GameMapController : MonoBehaviour2
    {
        public MineableLayer mineableLayer;
        public UnitOrdersLayer unitOrdersLayer;
        private IUnitActionService actionService;

        [Inject]
        public void Construct(IUnitActionService _actionService)
        {
            this.actionService = _actionService;

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnClickedByUser()
        {
            Debug.Log("clicked");
        }
    }
}