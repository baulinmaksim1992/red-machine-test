using System.Collections.Generic;

namespace Connection
{
    public class ColorConnectionHistoryHandler
    {
        private readonly Stack<ColorConnectionData> _connectionsHistoryQueue = new();


        public void RememberConnection(ColorNode mainColorNode, ColorNode targetColorNode,
            ColorConnector colorConnector)
        {
            _connectionsHistoryQueue.Push(
                new ColorConnectionData(mainColorNode, targetColorNode, colorConnector));
        }

        public ColorConnectionData GetPreviousConnectionData()
        {
            return _connectionsHistoryQueue.TryPop(out var result) 
                ? result 
                : null;
        }
    }
}