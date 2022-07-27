using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using GameControllers.Models;

namespace Environment
{
    public class LayerCollider : MonoBehaviour2
    {
        Action clickCallback;
        Action onMouseEnterCallback;
        Action onMouseExitCallback;
        Action<DragEventModel> onDragCallback;
        Action<DragEventModel> onDragEndCallback;
        [Inject]
        public void Construct(Vector2 _size,
                                string _layer,
                                IList<Action> _callbacks,
                                IList<Action<DragEventModel>> _dragCallBacks)
        {
            this.gameObject.layer = LayerMask.NameToLayer(_layer);
            this.gameObject.tag = "AllowMovement";
            this.GetComponent<BoxCollider2D>().size = _size;
            this.transform.position = Vector3.zero;
            this.clickCallback = _callbacks[0];
            this.onMouseEnterCallback = _callbacks[1];
            this.onMouseExitCallback = _callbacks[2];
            this.onDragCallback = _dragCallBacks[0];
            this.onDragEndCallback = _dragCallBacks[1];
        }

        public override void OnClickedByUser()
        {
            this.clickCallback();
        }

        public override void OnDrag(DragEventModel dragEvent)
        {
            this.onDragCallback(dragEvent);
        }

        public override void OnDragEnd(DragEventModel dragEvent)
        {
            this.onDragEndCallback(dragEvent);
        }

        public override void OnMouseEnter()
        {
            this.onMouseEnterCallback();
        }

        public override void OnMouseExit()
        {
            this.onMouseExitCallback();
        }

        public class Factory : PlaceholderFactory<Vector2, string, IList<Action>, IList<Action<DragEventModel>>, LayerCollider>
        {
            
        }
    }

}
