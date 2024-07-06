using Utils.MonoBehaviourUtils;

namespace Utils.Singleton
{
    public class DontDestroyMonoBehaviour : GameMonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}