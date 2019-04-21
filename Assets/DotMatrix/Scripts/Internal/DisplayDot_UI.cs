//    DotMatrix - DisplayDot - UI


namespace Leguar.DotMatrix.Internal {
	
	using UnityEngine;
	using UnityEngine.UI;
	
	public class DisplayDot_UI : DisplayDot {
		
		private Display_UI displayUI;
		private Image image;
		private Color imageColor; // Keeping separate copy of this like 'rectTransform' because reading color from image is surprisingly slow in certain platforms
		private RectTransform rectTransform;

		private int fromState;
		private int toState;
		private Color fromColor;
		private Color toColor;
		private float stateChangeElapsed;

		internal DisplayDot_UI(Display_UI displayUI, int x, int y, Image image) : base(displayUI,x,y) {
			this.displayUI=displayUI;
			this.image=image;
			imageColor=image.color;
			rectTransform=image.GetComponent<RectTransform>();
		}
		
		internal void setObjectScaleAndPosition(Vector2 dotSize, Vector2 dotSpacing, Display_UI.UIPositions uiPosition, RectTransform displayRectTransform) {
			Rect displayRect=displayRectTransform.rect;
			float dotWidth=dotSize.x;
			float dotHeight=dotSize.y;
			float dotSpacingX=dotSpacing.x;
			float dotSpacingY=dotSpacing.y;
			int dotCountX=displayUI.WidthInDots+displayUI.BorderDots*2;
			int dotCountY=displayUI.HeightInDots+displayUI.BorderDots*2;
			float totalWidth=dotWidth*dotCountX+dotSpacingX*(dotCountX-1);
			float totalHeight=dotHeight*dotCountY+dotSpacingY*(dotCountY-1);
			if (displayUI.UIFit!=Display_UI.UIFits.KeepDotSize) {
				float panelWidth=displayRect.width;
				float panelHeight=displayRect.height;
				float multipX=panelWidth/totalWidth;
				float multipY=panelHeight/totalHeight;
				if (displayUI.UIFit==Display_UI.UIFits.FitButKeepAspectRatio) {
					multipX=multipY=Mathf.Min(multipX,multipY);
				}
				dotWidth*=multipX;
				dotHeight*=multipY;
				dotSpacingX*=multipX;
				dotSpacingY*=multipY;
				totalWidth*=multipX;
				totalHeight*=multipY;
			}
			int drawX=base.getDrawX();
			int drawY=base.getDrawY();
			rectTransform.sizeDelta=new Vector2(dotWidth,dotHeight);
			float tx = getAnchoredPositionX(uiPosition,displayRect,displayRectTransform,totalWidth) + drawX*(dotWidth+dotSpacingX);
			float ty = getAnchoredPositionY(uiPosition,displayRect,displayRectTransform,totalHeight) + drawY*(dotHeight+dotSpacingY);
			rectTransform.anchoredPosition3D=new Vector3(tx,ty,0f);
		}

		internal override void setVisibleStateInstantly(int state) {
			fromState=toState=state;
			imageColor=fromColor=toColor=displayUI.GetColor(state);
			image.color=imageColor;
		}
		
		internal override void setNewVisibleTargetState(int state) {
			fromState=toState;
			toState=state;
			fromColor=imageColor;
			toColor=displayUI.GetColor(state);
			stateChangeElapsed=0f;
		}
		
		internal override bool updateVisibleState(float deltaTime) {
			stateChangeElapsed+=deltaTime;
			float delay=base.getDelay(fromState,toState);
			if (delay<=0f || stateChangeElapsed>=delay) {
				imageColor=toColor;
				image.color=imageColor;
				return false; // Done, no need for new updates until new target state is set
			}
			float percent=stateChangeElapsed/delay;
			imageColor=Color.Lerp(fromColor,toColor,percent);
			image.color=imageColor;
			return true; // Need new updates to reach final color
		}

		private float getAnchoredPositionX(Display_UI.UIPositions uiPosition, Rect displayRect, RectTransform displayRectTransform, float totalWidth) {
			if (uiPosition==Display_UI.UIPositions.UpperLeft || uiPosition==Display_UI.UIPositions.MiddleLeft || uiPosition==Display_UI.UIPositions.LowerLeft) {
				return 0f;
			} else if (uiPosition==Display_UI.UIPositions.UpperCenter || uiPosition==Display_UI.UIPositions.MiddleCenter || uiPosition==Display_UI.UIPositions.LowerCenter) {
				return (displayRect.width*0.5f-totalWidth*0.5f);
			} else if (uiPosition==Display_UI.UIPositions.UpperRight || uiPosition==Display_UI.UIPositions.MiddleRight || uiPosition==Display_UI.UIPositions.LowerRight) {
				return (displayRect.width-totalWidth);
			} else {
				return (-displayRect.xMin-displayRectTransform.pivot.x*totalWidth);
			}
		}
		
		private float getAnchoredPositionY(Display_UI.UIPositions uiPosition, Rect displayRect, RectTransform displayRectTransform, float totalHeight) {
			if (uiPosition==Display_UI.UIPositions.UpperLeft || uiPosition==Display_UI.UIPositions.UpperCenter || uiPosition==Display_UI.UIPositions.UpperRight) {
				return (displayRect.height-totalHeight);
			} else if (uiPosition==Display_UI.UIPositions.MiddleLeft || uiPosition==Display_UI.UIPositions.MiddleCenter || uiPosition==Display_UI.UIPositions.MiddleRight) {
				return (displayRect.height*0.5f-totalHeight*0.5f);
			} else if (uiPosition==Display_UI.UIPositions.LowerLeft || uiPosition==Display_UI.UIPositions.LowerCenter || uiPosition==Display_UI.UIPositions.LowerRight) {
				return 0f;
			} else {
				return (-displayRect.yMin-displayRectTransform.pivot.y*totalHeight);
			}
		}

	}

}
