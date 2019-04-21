//    DotMatrix - AbsCmdSpeed


using UnityEngine;

namespace Leguar.DotMatrix {
	
	/// <summary>
	/// Abstract base class for all commands that may be executed with different speeds, such as scrolling text.
	/// </summary>
	public abstract class AbsCmdSpeed : AbsCmd {
		
		private float dotsPerSecond;
		private bool dotsPerSecondSet;
		
		/// <summary>
		/// Set moving content speed. If this is not set, default value from Controller is used.
		/// </summary>
		/// <value>
		/// Speed in dots per second.
		/// </value>
		public float DotsPerSecond {
			set {
				dotsPerSecond=value;
				dotsPerSecondSet=true;
			}
			get {
				if (!dotsPerSecondSet) {
					Debug.LogError("Unable to get command 'DotsPerSecond' since value is not set. Command will use Controller default value when executed.");
				}
				return dotsPerSecond;
			}
		}
		
		internal AbsCmdSpeed() {
			dotsPerSecondSet=false;
		}
		
		internal float getDotsPerSecond(float controllerDefault) {
			return (dotsPerSecondSet?dotsPerSecond:controllerDefault);
		}
		
	}

}
