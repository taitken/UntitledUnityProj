using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Item.Models;
using Zenject;

namespace Item
{
    public class ItemObject : MonoBehaviour2
    {
        ItemObjectModel itemObject;
        [Inject]
        public void Construct(ItemObjectModel _itemObjectModel)
        {
            this.itemObject = _itemObjectModel;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Factory : PlaceholderFactory<ItemObjectModel, ItemObject>
        {
        }
    }
}
