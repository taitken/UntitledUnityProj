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
        private ObjectContextWindow.Factory windowFactory;
        private IContextWindowService contextWindowService;
        private IList<ContextWindowModel> contextModels = new List<ContextWindowModel>();
        private IList<ContextWindow> windows = new List<ContextWindow>();
        [Inject]
        public void Construct(ObjectContextWindow.Factory _windowFactory,
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
                this.CalcWindowPosition(window.GetComponent<RectTransform>(), index != 0 ? windows[index - 1].GetComponent<RectTransform>() : null);
            });
        }

        void GenerateWindows(IList<ContextWindowModel> context)
        {
            this.windows.ForEach(window => { window.Destroy(); });
            this.windows = new List<ContextWindow>();
            context.ForEach((contextModel, index) =>
            {
                ContextWindow newWindow = this.windowFactory.Create(contextModel);
                newWindow.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>().transform);
                newWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(50, -20);
                this.CalcWindowPosition(newWindow.GetComponent<RectTransform>(), index != 0 ? windows[index - 1].GetComponent<RectTransform>() : null);
                this.windows.Add(newWindow);
            });
        }

        private void CalcWindowPosition(RectTransform newWindow, RectTransform previousRectT)
        {
            if (previousRectT == null)
            {
                newWindow.position = new Vector3(Mouse.current.position.ReadValue().x + 100, Mouse.current.position.ReadValue().y - 60, 0);
            }
            else
            {
                newWindow.GetComponent<RectTransform>().position = new Vector3(previousRectT.position.x, previousRectT.position.y - previousRectT.rect.height - 5, 0);
            }
        }
    }
}