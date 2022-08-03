using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using UI.Models;
using UI.Services;

namespace UI
{
    public class ContextController : MonoBehaviour2
    {
        private ContextWindow.Factory windowFactory;
        private IContextWindowService contextWindowService;
        private IList<ContextWindowModel> contextModels = new List<ContextWindowModel>();
        private IList<ContextWindow> windows = new List<ContextWindow>();
        [Inject]
        public void Construct(ContextWindow.Factory _windowFactory,
                                IContextWindowService _contextService)
        {
            this.windowFactory = _windowFactory;
            this.contextWindowService = _contextService;
            this.contextWindowService.contextObseravable.Subscribe(this, _newContext =>
            {
                this.contextModels = _newContext;
                this.GenerateWindows(this.contextModels);
            });
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        void Update()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            this.windows.ForEach((window, index) =>
            {
                window.transform.position = CalcWindowPosition(Mouse.current.position.ReadValue(), window.rectTransform.rect.height, index);
            });
        }

        void GenerateWindows(IList<ContextWindowModel> context)
        {
            this.windows.ForEach(window => { window.Destroy(); });
            this.windows = new List<ContextWindow>();
            context.ForEach((contextModel, index) =>
            {
                ContextWindow newWindow = this.windowFactory.Create(contextModel);
                newWindow.rectTransform.SetParent(this.GetComponent<RectTransform>().transform);
                newWindow.rectTransform.sizeDelta = new Vector2(50, -20);
                newWindow.transform.position = CalcWindowPosition(Mouse.current.position.ReadValue(), newWindow.rectTransform.rect.height, index);
                this.windows.Add(newWindow);
            });
        }

        private Vector3 CalcWindowPosition(Vector2 mousePos, float windowHeight, int windowIndex)
        {
            return new Vector3(Mouse.current.position.ReadValue().x + 100,
                                (Mouse.current.position.ReadValue().y - 80) - ((windowHeight + 5) * windowIndex),
                                 1);
        }
    }
}