using System;

namespace Utils.MonoBehaviourUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class NotRequiredField : Attribute
    {
    }
}