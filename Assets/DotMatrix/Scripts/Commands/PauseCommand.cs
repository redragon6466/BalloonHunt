//    DotMatrix - Command - Pause


using Leguar.DotMatrix.Internal;

namespace Leguar.DotMatrix {
	
	/// <summary>
	/// Command to pause executing commands until certain amount of time is passed. Whatever is on display when pause is reached will stay there.
	/// </summary>
	public class PauseCommand : AbsCmd {
		
		private float pauseSeconds;
		
		/// <summary>
		/// Creates new pause command.
		/// </summary>
		/// <param name="pauseSeconds">
		/// Seconds how long Controller and display will be paused.
		/// </param>
		public PauseCommand(float pauseSeconds) {
			this.pauseSeconds=pauseSeconds;
		}
		
		internal override RawCommand getRawCommand(DotMatrix dotMatrix, DisplayModel displayModel, Controller controller) {
			return (new RawCommandDelay(pauseSeconds));
		}
		
	}

}
