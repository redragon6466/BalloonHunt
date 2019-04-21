//    DotMatrix - Display - 3D-object based for 3D world


using UnityEngine;
using Leguar.DotMatrix.Internal;

namespace Leguar.DotMatrix {
	
	/// <summary>
	/// Display where dots are based on actual 3D objects which rotates when turning on and off.
	/// </summary>
	public class Display_3D : Display {
		
		[Header("Dot size in units")]

		[SerializeField]
		private Vector3 dotSize=Vector3.one;
		
		/// <summary>
		/// Size of single dot.
		/// </summary>
		/// <value>
		/// Size of single dot as Vector3
		/// </value>
		public Vector3 DotSize {
			set {
				if (base.runTimeCheck(value,dotSize,"DotSize")) {
					dotSize=value;
				}
			}
			get {
				return dotSize;
			}
		}

		[Header("Dot rotations")]

		[SerializeField]
		private Vector3[] rotations=new Vector3[] {
			new Vector3(0f,90f,0f),
			new Vector3(0f,0f,0f)
		};
		
		/// <summary>
		/// Gets or sets the count of rotations this display's dots can have. Default and minimum is two rotations.
		/// </summary>
		/// <value>
		/// Amount of different possible rotation states.
		/// </value>
		public int RotationCount {
			set {
				if (value!=rotations.Length && this.runTimeCheck(value,rotations.Length,"RotationCount")) {
					Vector3[] oldRotationsArray=rotations;
					rotations=new Vector3[value];
					for (int n=0; n<value; n++) {
						rotations[n]=(n<oldRotationsArray.Length?oldRotationsArray[n]:Vector3.zero);
					}
				}
			}
			get {
				return rotations.Length;
			}
		}
		
		/// <summary>
		/// Rotation of single dot object when it is turned to off-state.
		/// This have same effect than using SetRotation(0,Vector3) or GetRotation(0,Vector3) methods and is here for backward compatibility.
		/// </summary>
		/// <value>
		/// Dot rotation when in off-state.
		/// </value>
		public Vector3 OffRotation {
			set {
				if (base.playingInitAndChanged(value,rotations[0])) {
					this.needFullStateUpdate=true;
				}
				rotations[0]=value;
			}
			get {
				return rotations[0];
			}
		}

		/// <summary>
		/// Rotation of single dot object when it is turned to on-state.
		/// This have same effect than using SetRotation(1,Vector3) or GetRotation(1,Vector3) methods and is here for backward compatibility.
		/// </summary>
		/// <value>
		/// Dot rotation when in on-state.
		/// </value>
		public Vector3 OnRotation {
			set {
				if (base.playingInitAndChanged(value,rotations[1])) {
					this.needFullStateUpdate=true;
				}
				rotations[1]=value;
			}
			get {
				return rotations[1];
			}
		}

		/// <summary>
		/// Sets rotation used in dot objects when dot state is set to 'index'.
		/// </summary>
		/// <param name="state">
		/// 0 is dot 'off' state, 1 is default 'on' state, 2 and above are additional 'on' state rotations.
		/// </param>
		/// <param name="rotation">
		/// Rotation to use.
		/// </param>
		public void SetRotation(int state, Vector3 rotation) {
			if (base.playingInitAndChanged(rotation,rotations[state])) {
				this.needFullStateUpdate=true;
			}
			rotations[state]=rotation;
		}

		/// <summary>
		/// Gets rotation used in dot objects when dot state is set to 'index'.
		/// </summary>
		/// <param name="state">
		/// 0 is dot 'off' state, 1 is default 'on' state, 2 and above are additional 'on' state rotations.
		/// </param>
		/// <returns>
		/// Rotation used.
		/// </returns>
		public Vector3 GetRotation(int state) {
			return rotations[state];
		}

		internal override GameObject createGroupObject(string groupObjectName, Transform parent) {
			GameObject groupObject=new GameObject(groupObjectName);
			setParent(groupObject,parent);
			groupObject.transform.localPosition=Vector3.zero;
			groupObject.transform.localRotation=Quaternion.identity;
			groupObject.transform.localScale=Vector3.one;
			return groupObject;
		}
		
		internal override void setParent(GameObject ntGameObject, Transform parent) {
			Transform got=ntGameObject.transform;
			got.parent=parent;
			got.localPosition=Vector3.zero;
			got.localRotation=Quaternion.identity;
			got.localScale=Vector3.one;
		}
		
		internal override DisplayDot createDisplayDot(GameObject dotObject, int x, int y) {
			Transform dotTransform=dotObject.transform;
			return (new DisplayDot_3D(this,x,y,dotTransform));
		}
		
		internal override void setDisplayDotScaleAndPosition(DisplayDot displayDot) {
			((DisplayDot_3D)(displayDot)).setObjectScaleAndPosition(dotSize,DotSpacing);
		}
		
		protected override int getStateCount() {
			return RotationCount;
		}

	}
	
}
