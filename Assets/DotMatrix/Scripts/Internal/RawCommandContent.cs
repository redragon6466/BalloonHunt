﻿//    DotMatrix - RawCommand


using UnityEngine;

namespace Leguar.DotMatrix.Internal {
	
	internal class RawCommandContent : RawCommand {
		
		private int[,] content;
		private int posX;
		private int posY;
		
		private AbsCmdPosition.Movements movement;
		private int push;
		
		private float secondsPerDot;
		
		private int counter;
		
		internal RawCommandContent(int[,] content, int posX, int posY) {
			this.content=content;
			this.posX=posX;
			this.posY=posY;
			counter=-1;
		}
		
		internal RawCommandContent(int[,] content, int posX, int posY, AbsCmdPosition.Movements movement, int push, float dotsPerSecond) {
			this.content=content;
			this.posX=posX;
			this.posY=posY;
			this.movement=movement;
			this.push=push;
			secondsPerDot=1f/dotsPerSecond;
			counter=0;
		}

		internal override float runStep(DisplayModel displayModel, float timeToConsume) {

			if (counter<0) {
				displayModel.Clear();
				displayModel.SetPartialContent(content,posX,posY);
				return timeToConsume;
			}

			while (timeToConsume>=secondsPerDot && counter<push) {

				if (movement==AbsCmdPosition.Movements.MoveLeftAndStop || movement==AbsCmdPosition.Movements.MoveLeftAndPass) {
					if (counter<content.GetLength(1)) {
						displayModel.pushLeftAndSetRightColumn(content,counter,posY);
					} else {
						displayModel.PushLeft();
					}
				} else if (movement==AbsCmdPosition.Movements.MoveRightAndStop || movement==AbsCmdPosition.Movements.MoveRightAndPass) {
					if (counter<content.GetLength(1)) {
						displayModel.pushRightAndSetLeftColumn(content,content.GetLength(1)-counter-1,posY);
					} else {
						displayModel.PushRight();
					}
				} else if (movement==AbsCmdPosition.Movements.MoveUpAndStop || movement==AbsCmdPosition.Movements.MoveUpAndPass) {
					if (counter<content.GetLength(0)) {
						displayModel.pushUpAndSetBottomRow(content,counter,posX);
					} else {
						displayModel.PushUp();
					}
				} else if (movement==AbsCmdPosition.Movements.MoveDownAndStop || movement==AbsCmdPosition.Movements.MoveDownAndPass) {
					if (counter<content.GetLength(0)) {
						displayModel.pushDownAndSetTopRow(content,content.GetLength(0)-counter-1,posX);
					} else {
						displayModel.PushDown();
					}
				}

				timeToConsume-=secondsPerDot;
				counter++;

			}

			return timeToConsume;

		}
		
		internal override bool isFinished(DisplayModel displayModel) {
			return (counter==-1 || counter>=push);
		}
		
	}

}
