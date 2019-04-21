//    DotMatrix - RawCommand


using UnityEngine;

namespace Leguar.DotMatrix.Internal {
	
	internal class RawCommandClear : RawCommand {
		
		private int targetState;
		
		private ClearCommand.Methods method;
		private float secondsPerDot;
		
		private int counter;
		private int counterMax;
		
		internal RawCommandClear(int targetState) {
			this.targetState=targetState;
			method=ClearCommand.Methods.Instant;
			secondsPerDot=0f;
		}
		
		internal RawCommandClear(int targetState, ClearCommand.Methods method, float dotsPerSecond) {
			this.targetState=targetState;
			this.method=method;
			this.secondsPerDot=1f/dotsPerSecond;
			counter=-1;
		}
		
		internal RawCommandClear(int targetState, ClearCommand.Methods method, float dotsPerSecond, int counterMax) {
			this.targetState=targetState;
			this.method=method;
			this.secondsPerDot=1f/dotsPerSecond;
			counter=0;
			this.counterMax=counterMax;
		}
		
		internal override float runStep(DisplayModel displayModel, float timeToConsume) {

			if (method==ClearCommand.Methods.Instant) {
				displayModel.SetAll(targetState);
				return timeToConsume;
			}

			if (counter<0) {

				while (timeToConsume>=secondsPerDot) {
					
					if (method==ClearCommand.Methods.MoveLeft) {
						displayModel.PushLeft();
						if (targetState>0) {
							displayModel.SetColumn(displayModel.Width-1,targetState);
						}
					} else if (method==ClearCommand.Methods.MoveRight) {
						displayModel.PushRight();
						if (targetState>0) {
							displayModel.SetColumn(0,targetState);
						}
					} else if (method==ClearCommand.Methods.MoveUp) {
						displayModel.PushUp();
						if (targetState>0) {
							displayModel.SetRow(displayModel.Height-1,targetState);
						}
					} else if (method==ClearCommand.Methods.MoveDown) {
						displayModel.PushDown();
						if (targetState>0) {
							displayModel.SetRow(0,targetState);
						}
					}

					timeToConsume-=secondsPerDot;

					if (isFinished(displayModel)) {
						break;
					}

				}

			} else {

				while (timeToConsume>=secondsPerDot && counter<counterMax) {

					if (method==ClearCommand.Methods.ColumnByColumnFromLeft) {
						displayModel.SetColumn(counter,targetState);
					} else if (method==ClearCommand.Methods.ColumnByColumnFromRight) {
						displayModel.SetColumn(displayModel.Width-counter-1,targetState);
					} else if (method==ClearCommand.Methods.RowByRowFromTop) {
						displayModel.SetRow(counter,targetState);
					} else if (method==ClearCommand.Methods.RowByRowFromBottom) {
						displayModel.SetRow(displayModel.Height-counter-1,targetState);
					}

					timeToConsume-=secondsPerDot;
					counter++;

				}

			}

			return timeToConsume;

		}
		
		internal override bool isFinished(DisplayModel displayModel) {
			if (method==ClearCommand.Methods.Instant) {
				return true;
			}
			if (counter==-1) {
				return displayModel.IsAllDotsInState(targetState);
			}
			return (counter>=counterMax);
		}
		
	}
	
}
