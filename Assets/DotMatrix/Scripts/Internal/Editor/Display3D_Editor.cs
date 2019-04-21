//    DotMatrix Display - Custom Editor


using UnityEngine;
using UnityEditor;

namespace Leguar.DotMatrix {
	
	[CustomEditor(typeof(Display_3D))]
	public class Display3D_Editor : Display_Editor {
		
		public override void OnInspectorGUI() {
			
			//  Undo
			
			Undo.RecordObject(target,"DotMatrix Display_3D Change");
			
			//  Variables
			
			Display_3D displayTarget=(Display_3D)(target);
			
			bool changes=false;

			// Display in editor
			
			base.createDotsInEditor(displayTarget,ref changes);

			// Display size in dots
			
			int totalDots=base.displaySizeInDots(displayTarget,ref changes);
			
			// Prefabs
			
			base.prefabs(displayTarget,ref changes);
			
			// Dot size in units
			
			EditorCommon.addHeader("Dot Size in Units");
			
			displayTarget.DotSize=EditorCommon.vector3Field("Dot Size",displayTarget.DotSize,true,ref changes);
			displayTarget.DotSpacing=EditorCommon.vector2Field("Dot Spacing",displayTarget.DotSpacing,false,ref changes);
			
			// State rotations
			
			EditorCommon.addHeader("Dot States (Rotations)");

			displayTarget.RotationCount=EditorCommon.intSlider("Dot Rotation Count",displayTarget.RotationCount,2,6,ref changes);

			displayTarget.OffRotation=EditorCommon.vector3Field("Dot Off Rotation",displayTarget.OffRotation,false,ref changes);
			displayTarget.OnRotation=EditorCommon.vector3Field("Dot "+(displayTarget.RotationCount>2?"Default ":"")+"On Rotation",displayTarget.OnRotation,false,ref changes);
			for (int n=2; n<displayTarget.RotationCount; n++) {
				displayTarget.SetRotation(n,EditorCommon.vector3Field("Dot On Rotation "+n,displayTarget.GetRotation(n),false,ref changes));
			}

			// Real life problems
			
			base.realism(displayTarget,totalDots,(displayTarget.RotationCount>2),ref changes);
			
			// Possibly recreate or redraw display in editor
			
			if (changes) {
				EditorUtility.SetDirty(displayTarget);
			}
			
		}
		
	}
	
}
