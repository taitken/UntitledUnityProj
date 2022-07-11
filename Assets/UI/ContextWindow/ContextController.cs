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
            this.subscriptions.Add(this.contextWindowService.contextSubscribable.Subscribe(_newContext =>
            {
                this.contextModels = _newContext;
                this.GenerateWindows(this.contextModels);
            }));
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        void Update()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            this.windows.ForEach(window =>
            {
                window.transform.position = new Vector3(Mouse.current.position.ReadValue().x + 100, Mouse.current.position.ReadValue().y - 80, 1);
            });
        }

        void GenerateWindows(IList<ContextWindowModel> context)
        {
            this.windows.ForEach(window => { window.Destroy(); });
            this.windows = new List<ContextWindow>();
            context.ForEach(contextModel =>
            {
                ContextWindow newWindow = this.windowFactory.Create(contextModel);
                newWindow.rectTransform.SetParent(this.GetComponent<RectTransform>().transform);
                newWindow.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                newWindow.transform.position = new Vector3(Mouse.current.position.ReadValue().x + 100, Mouse.current.position.ReadValue().y - 80, 1);
                this.windows.Add(newWindow);
            });
        }
    }
}