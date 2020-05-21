namespace Zeef.GameManagement
{
	public abstract class GameState : SingleInstance<GameState>
    {	
		public static bool IsPlaying => !Instance || Instance._IsPlaying();

		protected abstract bool _IsPlaying();
	}
}