using System;
using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using Room.Models;
using TMPro;
using UI.GenericComponents;
using UnityEngine;
using UtilityClasses;

namespace UI.Panel
{
    public class RoomDetailsTab : BaseTabContent
    {
        public TextMeshProUGUI textBox;
        public RoomModel roomModel;
        public bool Initalise(RoomModel _roomModel)
        {
            this.roomModel = _roomModel;
            this.SetText();
            return true;
        }

        public void Update()
        {
            this.SetText();
        }

        private void SetText()
        {
            if (this.roomModel != null)
            {
                string boxString = "Room Details \n\n";
                IList<string> itemRow = new List<string>();
                itemRow.Add("Room Type: " + this.roomModel.floorType.ToString());
                itemRow.Add("Connected Tiles: " + this.roomModel.connectedTiles.Count);
                itemRow.Add("Border Tiles: " + this.roomModel.borderTiles.Count);
                itemRow.Add("Enclosed Border Tiles: " + this.roomModel.enclosedBorders);
                string objectCompositionString = boxString + itemRow.ConcatStrings("\n");
                this.textBox.SetText(objectCompositionString);
            }
        }
    }
}