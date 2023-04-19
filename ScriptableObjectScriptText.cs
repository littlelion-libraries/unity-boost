using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityExtensions
{
    [CreateAssetMenu(fileName = "Script Text", menuName = "Unity Extensions/Script Text", order = 1)]
    public class ScriptableObjectScriptText : ScriptableObject
    {
        [Serializable]
        public struct Data
        {
            public bool array;
            public bool indented;
            public TextAsset textAsset;
            public UnityType unityType;
        }

        public Data[] data;
    }
}