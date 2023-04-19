using System;

namespace UnityExtensions.Editors
{
    public class DrawObject
    {
        public bool Foldout;
        public string Label;
        public virtual object Impl { get; set; }
        public virtual Type Type { get; set; }
    }
}