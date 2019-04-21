//    DotMatrix - DisplayDot - Sprite


namespace Leguar.DotMatrix.Internal {
	
	using UnityEngine;
	
	public class DisplayDot_Sprite : DisplayDot {
		
		private Display_Sprite displaySprite;
		private SpriteRenderer spriteRenderer;

		private int fromState;
		private int toState;
		private Color fromColor;
		private Color toColor;
		private float stateChangeElapsed;

		internal DisplayDot_Sprite(Display_Sprite displaySprite, int x, int y, SpriteRenderer spriteRenderer) : base(displaySprite,x,y) {
			this.displaySprite=displaySprite;
			this.spriteRenderer=spriteRenderer;
		}
		
		internal void setObjectScaleAndPosition(Vector2 dotSize, Vector2 dotSpacing) {
			int drawX=base.getDrawX();
			int drawY=base.getDrawY();
			spriteRenderer.transform.localScale=dotSize;
			spriteRenderer.transform.localPosition=new Vector3(drawX*(dotSize.x+dotSpacing.x)+dotSize.x*0.5f,drawY*(dotSize.y+dotSpacing.y)+dotSize.y*0.5f,0f);
		}

		internal override void setVisibleStateInstantly(int state) {
			fromState=toState=state;
			fromColor=toColor=displaySprite.GetColor(state);
			spriteRenderer.color=toColor;
		}

		internal override void setNewVisibleTargetState(int state) {
			fromState=toState;
			toState=state;
			fromColor=spriteRenderer.color;
			toColor=displaySprite.GetColor(state);
			stateChangeElapsed=0f;
		}

		internal override bool updateVisibleState(float deltaTime) {
			stateChangeElapsed+=deltaTime;
			float delay=base.getDelay(fromState,toState);
			if (delay<=0f || stateChangeElapsed>=delay) {
				spriteRenderer.color=toColor;
				return false; // Done, no need for new updates until new target state is set
			}
			float percent=stateChangeElapsed/delay;
			spriteRenderer.color=Color.Lerp(fromColor,toColor,percent);
			return true; // Need new updates to reach final color
		}

	}

}
