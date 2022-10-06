
using UnityEngine;
using GameControllers.Services;
using GameControllers;
using Zenject;
using UtilityClasses;

namespace GameControllers
{
    public class DayCycleController : MonoBehaviour2
    {
        private IDayCycleService dayCycleService;

        [Inject]
        public void Construct(IDayCycleService _dayCycleService)
        {
            this.dayCycleService = _dayCycleService;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            this.dayCycleService.UpdateCycle(GameTime.deltaTime);
        }
    }
}