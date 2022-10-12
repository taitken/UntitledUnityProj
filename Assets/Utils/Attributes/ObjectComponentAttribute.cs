

using ObjectComponents;
using UnityEngine;

namespace System
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ObjectComponentAttribute : Attribute
    {
        public Type objectComponentType;
        public ObjectComponentAttribute(Type _objectComponentType)
        {
            if (_objectComponentType.IsSubclassOf(typeof(ObjectComponent)))
            {
                this.objectComponentType = _objectComponentType;
            }
            else
            {
                Debug.LogException(new System.Exception("ObjectComponentAttributes can only be declared with child types of ObjectComponent"));
            }
        }

        public Type GetCustomType()
        {
            return this.objectComponentType;
        }
    }
}