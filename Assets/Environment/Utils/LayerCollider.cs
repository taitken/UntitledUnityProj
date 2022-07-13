using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace Environment
{
    public class LayerCollider : MonoBehaviour2
    {
        Action clickCallback;
        [Inject]
        public void Construct(Vector2 _size,
                                string _layer,
                                Action _callBack)
        {
            this.gameObject.layer = LayerMask.NameToLayer(_layer);
            this.gameObject.tag = "AllowMovement";
            this.GetComponent<BoxCollider2D>().size = _size;
            this.transform.position = Vector3.zero;
            this.clickCallback = _callBack;
        }

        public override void  OnClickedByUser()
        {
            this.clickCallback();
        }

        public class Factory : PlaceholderFactory<Vector2, string, Action, LayerCollider>
        {
        }
    }

}
