using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Zenject;

namespace UI
{
    public class MouseController : MonoBehaviour2
    {
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
    }
}

