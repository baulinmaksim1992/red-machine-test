using UnityEngine;

namespace Connection
{
    public class ColorConnectionData
    {
        public readonly ColorNode MainColorNode;
        public readonly ColorNode TargetColorNode;
        public readonly ColorConnector ColorConnector;
        public readonly Color PreviousTargetColorNodeColor;
        public readonly bool WasTargetNodeEmpty;

        public ColorConnectionData(ColorNode mainColorNode, ColorNode targetColorNode,
            ColorConnector colorConnector)
        {
            MainColorNode = mainColorNode;
            TargetColorNode = targetColorNode;
            ColorConnector = colorConnector;
            PreviousTargetColorNodeColor = targetColorNode.Color;
            WasTargetNodeEmpty = targetColorNode.IsEmpty;
        }
    }
}