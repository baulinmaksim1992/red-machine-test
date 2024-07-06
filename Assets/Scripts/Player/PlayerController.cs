using Utils.Singleton;


namespace Player
{
    public class PlayerController : DontDestroyMonoBehaviour
    {
        public static PlayerState PlayerState { get; private set; }

        private PlayerSateObserver _observer;


        protected override void Awake()
        {
            base.Awake();

            _observer = new PlayerSateObserver(SetPlayerState);
            _observer.Subscribe();
        }

        private void OnDestroy()
        {
            _observer.Unsubscribe();
        }

        private void SetPlayerState(PlayerState playerState)
        {
            PlayerState = playerState;
        }
    }
}