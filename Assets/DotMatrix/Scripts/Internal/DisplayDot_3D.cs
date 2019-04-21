//    DotMatrix - DisplayDot - 3D-object


namespace Leguar.DotMatrix.Internal {
	
	using UnityEngine;
	
	public class DisplayDot_3D : DisplayDot {
		
		private Display_3D display3D;
		private Transform dotTransform;

		private int fromState;
		private int toState;
		private Quaternion fromRotation;
		private Quaternion toRotation;
		private float stateChangeElapsed;

		internal DisplayDot_3D(Display_3D display3D, int x, int y, Transform dotTransform) : base(display3D,x,y) {
			this.display3D=display3D;
			this.dotTransform=dotTransform;
		}
		
		internal void setObjectScaleAndPosition(Vector3 dotSize, Vector2 dotSpacing) {
			int drawX=base.getDrawX();
			int drawY=base.getDrawY();
			dotTransform.localScale=dotSize;
			dotTransform.localPosition=new Vector3(drawX*(dotSize.x+dotSpacing.x)+dotSize.x*0.5f,drawY*(dotSize.y+dotSpacing.y)+dotSize.y*0.5f,0f);
		}

		internal override void setVisibleStateInstantly(int state) {
			fromState=toState=state;
			fromRotation=toRotation=Quaternion.Euler(display3D.GetRotation(state));
			dotTransform.localRotation=toRotation;
		}
		
		internal override void setNewVisibleTargetState(int state) {
			fromState=toState;
			toState=state;
			fromRotation=dotTransform.localRotation;
			toRotation=Quaternion.Euler(display3D.GetRotation(state));
			stateChangeElapsed=0f;
		}
		
		internal override bool updateVisibleState(float deltaTime) {
			stateChangeElapsed+=deltaTime;
			float delay=base.getDelay(fromState,toState);
			if (delay<=0f || stateChangeElapsed>=delay) {
				dotTransform.localRotation=toRotation;
				return false; // Done, no need for new updates until new target state is set
			}
			float percent=stateChangeElapsed/delay;
			dotTransform.localRotation=Quaternion.Lerp(fromRotation,toRotation,percent);
			return true; // Need new updates to reach final color
		}

	}

}
