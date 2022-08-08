using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using TMPro;

namespace UI
{
    public abstract class ContextWindow : MonoBehaviour2
    {
        public abstract void Construct(ContextWindowModel windowModel);
    }
}