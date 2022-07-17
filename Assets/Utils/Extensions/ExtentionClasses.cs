using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Extensions
{
    public static class ExtenionClasses
    {
        public static void OnClickedByUser(this MonoBehaviour monoBehaviour)
        {
            Debug.Log("parent");
        }

        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };
    }
}