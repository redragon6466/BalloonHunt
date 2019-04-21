//    DotMatrix - Display


using UnityEngine;
using System.Collections.Generic;
using Leguar.DotMatrix.Internal;

namespace Leguar.DotMatrix {
	
	[ExecuteInEditMode]
	/// <summary>
	/// Abstract base class for all the display types (3D, sprite based and UI). Display script is attached to
	/// DotMatrix prefabs and is defining how actual, visible display looks like.
	/// 
	/// Note that many fields in this class are public so they can be changed in Unity Editor inspector.
	/// Changing values on runtime may case errors or unexpected or delayed results.
	/// </summary>
	public abstract class Display : MonoBehaviour {
		
		private const string EDITOR_GROUP_NAME="Generated EditorDots";
		private const string RUNTIME_GROUP_NAME="Generated RunTimeDots";

		private bool runtimeInitDone=false;

		[Header("Display size in dots")]

		[SerializeField]
		private int widthInDots=59;

		/// <summary>
		/// Width of the display in dots. Usually set through inspector in Unity editor or by another script if this DotMatrix was created from
		/// prefab during runtime.
		/// </summary>
		/// <value>
		/// Display width in dots.
		/// </value>
		public int WidthInDots {
			set {
				if (runTimeCheck(value,widthInDots,"WidthInDots")) {
					widthInDots=value;
				}
			}
			get {
				return widthInDots;
			}
		}

		[SerializeField]
		private int heightInDots=7;

		/// <summary>
		/// Height of the display in dots. Usually set through inspector in Unity editor or by another script if this DotMatrix was created from
		/// prefab during runtime.
		/// </summary>
		/// <value>
		/// Display height in dots.
		/// </value>
		public int HeightInDots {
			set {
				if (runTimeCheck(value,heightInDots,"HeightInDots")) {
					heightInDots=value;
				}
			}
			get {
				return heightInDots;
			}
		}

		[SerializeField]
		private int borderDots=0;

		/// <summary>
		/// Easy and quick way to add borders around display. Border dots go around the display, they are always off and can't be accessed in any way.
		/// Set 0 for no borders.
		/// </summary>
		/// <value>
		/// Amount of dots that act as border around the active display area.
		/// </value>
		public int BorderDots {
			set {
				if (runTimeCheck(value,borderDots,"BorderDots")) {
					borderDots=value;
				}
			}
			get {
				return borderDots;
			}
		}

		[Header("Prefabs")]

		[SerializeField]
		private GameObject dotPrefab;

		/// <summary>
		/// Prefab for a single dot in display.
		/// </summary>
		/// <value>
		/// Prefab object.
		/// </value>
		public GameObject DotPrefab {
			set {
				if (runTimeCheck(value,dotPrefab,"DotPrefab")) {
					dotPrefab=value;
				}
			}
			get {
				return dotPrefab;
			}
		}

		[Header("Dot size in units")]

		[SerializeField]
		private Vector2 dotSpacing=Vector2.zero;

		/// <summary>
		/// Spacing between dots. 0 means no spacing. Negative values will cause dots to overlap each other but that is not recommended.
		/// </summary>
		/// <value>
		/// Spacing between dots as Vector2
		/// </value>
		public Vector2 DotSpacing {
			set {
				if (runTimeCheck(value,dotSpacing,"DotSpacing")) {
					dotSpacing=value;
				}
			}
			get {
				return dotSpacing;
			}
		}

		[Header("Realism (runtime only)")]

		[SerializeField]
		private int brokenAlwaysOffDots=0;

		/// <summary>
		/// Set some of the dots on display to be broken in the way that they are always off.
		/// </summary>
		/// <value>
		/// Amount of dots that always remain off.
		/// </value>
		public int BrokenAlwaysOffDots {
			set {
				brokenAlwaysOffDots=value;
			}
			get {
				return brokenAlwaysOffDots;
			}
		}

		[SerializeField]
		private int brokenAlwaysOnDots=0;

		/// <summary>
		/// Set some of the dots on display to be broken in the way that they are always on.
		/// </summary>
		/// <value>
		/// Amount of dots that always remain on.
		/// </value>
		public int BrokenAlwaysOnDots {
			set {
				brokenAlwaysOnDots=value;
			}
			get {
				return brokenAlwaysOnDots;
			}
		}

		[SerializeField]
		private float offDelaySeconds=0f;

		/// <summary>
		/// Set or get delay (in seconds) when dot is turning from any on-state to off-state. Especially in 3D-object based display
		/// (where each dot rotates when turning off) this adds realism but can be also used to simulate lagging display.
		/// </summary>
		/// <value>
		/// Seconds how long it takes to dot to turn off.
		/// </value>
		public float OffDelaySeconds {
			set {
				offDelaySeconds=value;
			}
			get {
				return offDelaySeconds;
			}
		}

		[SerializeField]
		private float onDelaySeconds=0f;

		/// <summary>
		/// Set or get delay (in seconds) when dot is turning from off-state to any on-state. Especially in 3D-object based display
		/// (where each dot rotates when turning on) this adds realism but can be also used to simulate lagging display.
		/// </summary>
		/// <value>
		/// Seconds how long it takes to dot to turn on.
		/// </value>
		public float OnDelaySeconds {
			set {
				onDelaySeconds=value;
			}
			get {
				return onDelaySeconds;
			}
		}

		[SerializeField]
		private float changeDelaySeconds=0f;
		
		/// <summary>
		/// Set or get delay (in seconds) when dot is changing state from on one state to another on state.
		/// This have effect only in displays that are using more than 2 colors.
		/// Note that 'enableDelays' have to be set true already before DotMatrix initializes.
		/// </summary>
		/// <value>
		/// Seconds how long it takes to dot to turn from one on state to another.
		/// </value>
		public float ChangeDelaySeconds {
			set {
				changeDelaySeconds=value;
			}
			get {
				return changeDelaySeconds;
			}
		}

		[Header("Editor/Debug")]

		[SerializeField]
		private bool createDotsInEditor=true;

		/// <summary>
		/// Set whatever display dots should be created and shown also in edit mode in Unity Editor.
		/// 
		/// This setting is normally used only through Unity Inspector window and have no effect when application is playing.
		/// </summary>
		/// <value>
		/// Boolean value, true to show display dots in editor, false to not.
		/// </value>
		public bool CreateDotsInEditor {
			set {
				if (Application.isPlaying && value!=createDotsInEditor) {
					Debug.Log("DotMatrix ("+this.gameObject.name+"): Changing Display 'CreateDotsInEditor' have no effect when application is playing");
				}
				createDotsInEditor=value;
			}
			get {
				return createDotsInEditor;
			}
		}

		/// <summary>
		/// Different types of content to display when showing dots in Unity Editor.
		/// </summary>
		public enum ContentsInEditor {
			/// <summary>Totally empty display.</summary>
			Empty,
			/// <summary>Display with all dots in on state.</summary>
			Filled,
			/// <summary>Empty display with borders.</summary>
			Borders,
			/// <summary>Display with borders and ruler.</summary>
			Ruler,
			/// <summary>Show any wanted text in display.</summary>
			Text
		}

		[SerializeField]
		private ContentsInEditor contentInEditor=ContentsInEditor.Ruler;

		/// <summary>
		/// Set or get what kind of content is shown in display in Unity Editor.
		/// 
		/// This setting is normally used only through Unity Inspector window and have no effect when application is playing.
		/// </summary>
		/// <value>
		/// One of the choices from enum ContentsInEditor
		/// </value>
		public ContentsInEditor ContentInEditor {
			set {
				if (Application.isPlaying && value!=contentInEditor) {
					Debug.Log("DotMatrix ("+this.gameObject.name+"): Changing Display 'ContentInEditor' have no effect when application is playing");
				}
				contentInEditor=value;
			}
			get {
				return contentInEditor;
			}
		}

		[SerializeField]
		private TextCommand.Fonts textInEditorFont=TextCommand.Fonts.Normal;

		/// <summary>
		/// Set or get font that is used when showing text in display in Unity Editor.
		/// 
		/// This setting is normally used only through Unity Inspector window and have no effect when application is playing.
		/// </summary>
		/// <value>
		/// One of the choices from enum TextCommand.Fonts
		/// </value>
		public TextCommand.Fonts TextInEditorFont {
			set {
				if (Application.isPlaying && value!=textInEditorFont) {
					Debug.Log("DotMatrix ("+this.gameObject.name+"): Changing Display 'TextInEditorFont' have no effect when application is playing");
				}
				textInEditorFont=value;
			}
			get {
				return textInEditorFont;
			}
		}

		[SerializeField]
		private string textInEditorText="TEST";

		/// <summary>
		/// Set or get text that is shown in display in Unity Editor.
		/// 
		/// This setting is normally used only through Unity Inspector window and have no effect when application is playing.
		/// </summary>
		/// <value>
		/// Text to be shown in display when in editor.
		/// </value>
		public string TextInEditorText {
			set {
				if (Application.isPlaying && value!=textInEditorText) {
					Debug.Log("DotMatrix ("+this.gameObject.name+"): Changing Display 'TextInEditorText' have no effect when application is playing");
				}
				textInEditorText=value;
			}
			get {
				return textInEditorText;
			}
		}

		private DisplayModel displayModel;
		
		private DisplayDot[,] displayDots;
		private List<DisplayDot> displayBorderDots;
		
//		private SlowChange[,] slowChanges;
		private bool runOwnUpdate;

		private int currentBrokenAlwaysOffDots;
		private int currentBrokenAlwaysOnDots;
		protected bool needFullStateUpdate;

		private GameObject editor_dotPrefab;
		private int editor_widthInDots;
		private int editor_heightInDots;
		private int editor_borderDots;
		
		internal void init(DotMatrix dotMatrix, DisplayModel displayModel) {

			// Set references and get display size
			this.displayModel=displayModel;
			int width=displayModel.Width;
			int height=displayModel.Height;

			// Create dots
			createDots(RUNTIME_GROUP_NAME,width,height,borderDots);

			// Broken dots
			setBrokenDots(width,height,false);
			currentBrokenAlwaysOffDots=brokenAlwaysOffDots;
			currentBrokenAlwaysOnDots=brokenAlwaysOnDots;

			// Set each dot initial state
			for (int y=0; y<height; y++) {
				for (int x=0; x<width; x++) {
					displayDots[y,x].setNewStateInstantly(0);
				}
			}

			// Done
			runOwnUpdate=false;
			needFullStateUpdate=false;
			runtimeInitDone=true;

		}

		internal void update(float deltaTime) {

			// Size from DisplayModel
			int width=displayModel.Width;
			int height=displayModel.Height;

			// Changes in broken dots?
			if (brokenAlwaysOffDots!=currentBrokenAlwaysOffDots || brokenAlwaysOnDots!=currentBrokenAlwaysOnDots) {
				brokenAlwaysOffDots=Mathf.Clamp(brokenAlwaysOffDots,0,width*height);
				brokenAlwaysOnDots=Mathf.Clamp(brokenAlwaysOnDots,0,width*height);
				if (brokenAlwaysOffDots+brokenAlwaysOnDots>width*height) {
					brokenAlwaysOnDots=width*height-brokenAlwaysOffDots;
				}
				if (brokenAlwaysOffDots==0 && brokenAlwaysOnDots==0) {
					for (int x=0; x<width; x++) {
						for (int y=0; y<height; y++) {
							displayDots[y,x].setBroken(-1);
						}
					}
					currentBrokenAlwaysOffDots=0;
					currentBrokenAlwaysOnDots=0;
				} else {
					while (brokenAlwaysOffDots<currentBrokenAlwaysOffDots) {
						removeRandomBrokenDot(width,height,false);
						currentBrokenAlwaysOffDots--;
					}
					while (brokenAlwaysOnDots<currentBrokenAlwaysOnDots) {
						removeRandomBrokenDot(width,height,true);
						currentBrokenAlwaysOnDots--;
					}
					while (brokenAlwaysOffDots>currentBrokenAlwaysOffDots) {
						addRandomBrokenDot(null,width,height,false);
						currentBrokenAlwaysOffDots++;
					}
					while (brokenAlwaysOnDots>currentBrokenAlwaysOnDots) {
						addRandomBrokenDot(null,width,height,true);
						currentBrokenAlwaysOnDots++;
					}
				}
			}

			// Need full redraw? (Display colors are changed on runtime)
			if (needFullStateUpdate) {
				for (int x=0; x<width; x++) {
					for (int y=0; y<height; y++) {
						displayDots[y,x].resetState();
					}
				}
				needFullStateUpdate=false;
			}

			// Any changes to dot target states?
			if (displayModel.checkChangesAndReset()) {
				for (int x=0; x<width; x++) {
					for (int y=0; y<height; y++) {
						int targetState=displayModel.GetDotState(x,y);
						displayDots[y,x].setNewTargetState(targetState);
						if (displayDots[y,x].updateVisibleState(0f)) {
							runOwnUpdate=true;
						}
					}
				}
			}

			// Need to run any delayed changes?
			if (!runOwnUpdate) {
				return;
			}

			// Delayed changes
			runOwnUpdate=false;
			for (int x=0; x<width; x++) {
				for (int y=0; y<height; y++) {
					if (displayDots[y,x].updateVisibleState(Time.deltaTime)) {
						runOwnUpdate=true;
					}
				}
			}

		}
		
		void Start() {
			if (Application.isPlaying) {
				destroyEditorDots(false);
			}
		}
		
		void Update() {

			// Display's own update is not used when in Play mode
			if (Application.isPlaying) {
				return;
			}

			// Destroy editor dots?
			if (!createDotsInEditor) {
				destroyEditorDots(true);
				return;
			}

			// Create/update editor dots
			if (this.transform.Find(EDITOR_GROUP_NAME)==null || displayDots==null || dotPrefab!=editor_dotPrefab || widthInDots!=editor_widthInDots || heightInDots!=editor_heightInDots || borderDots!=editor_borderDots) {
				// Destroy previous ones
				destroyEditorDots(true);
				// Save values used to create dots in editor mode
				editor_dotPrefab=dotPrefab;
				editor_widthInDots=widthInDots;
				editor_heightInDots=heightInDots;
				editor_borderDots=borderDots;
				// Create dots
				createDots(EDITOR_GROUP_NAME,editor_widthInDots,editor_heightInDots,editor_borderDots);
				// Set dot states
				setEditorDisplayContent();
			} else {
				for (int y=0; y<heightInDots; y++) {
					for (int x=0; x<widthInDots; x++) {
						displayDots[y,x].setBroken(-1);
					}
				}
				setSizeAndPositionOfAllDots();
				setEditorDisplayContent();
			}

		}

		protected void setSizeAndPositionOfAllDots() {
			// Set size and position
			for (int y=0; y<heightInDots; y++) {
				for (int x=0; x<widthInDots; x++) {
					setDisplayDotScaleAndPosition(displayDots[y,x]);
				}
			}
			// Same for border dots
			if (displayBorderDots!=null) {
				foreach (DisplayDot displayBorderDot in displayBorderDots) {
					setDisplayDotScaleAndPosition(displayBorderDot);
				}
			}
		}

		protected bool playingAndInit() {
			return (Application.isPlaying && runtimeInitDone);
		}

		protected bool playingInitAndChanged(object newValue, object oldValue) {
			return (Application.isPlaying && runtimeInitDone && !newValue.Equals(oldValue));
		}

		protected bool runTimeCheck(object newValue, object oldValue, string valueName) {
			if (playingInitAndChanged(newValue,oldValue)) {
				Debug.LogWarning("DotMatrix ("+this.gameObject.name+"): Display '"+valueName+"' can't be changed after DotMatrix and display is already initialized",this.gameObject);
				return false;
			}
			return true;
		}

		protected abstract int getStateCount();

		private void setEditorDisplayContent() {
			// Draw content
			if (contentInEditor!=ContentsInEditor.Text) {
				// Other content than text
				for (int y=0; y<editor_heightInDots; y++) {
					for (int x=0; x<editor_widthInDots; x++) {
						if (contentInEditor==ContentsInEditor.Empty) {
							displayDots[y,x].setNewStateInstantly(0);
						} else if (contentInEditor==ContentsInEditor.Filled) {
							displayDots[y,x].setNewStateInstantly(((x+y)%(getStateCount()-1))+1);
						} else if (contentInEditor==ContentsInEditor.Borders) {
							displayDots[y,x].setNewStateInstantly((x==0 || y==0 || x==editor_widthInDots-1 || y==editor_heightInDots-1) ? 1 : 0);
						} else {
							displayDots[y,x].setNewStateInstantly((x==0 || y==0 || x==editor_widthInDots-1 || y==editor_heightInDots-1 ||
							                                      (y==1 && (x+1)%5==0) || (x==1 && (y+1)%5==0) ||
							                                      (y==2 && (x+1)%10==0) || (x==2 && (y+1)%10==0)) ? 1 : 0);
						}
					}	
				}
			} else {
				// Text
				int[,] textContent=TextToContent.getContent(textInEditorText,textInEditorFont,true,false,1);
				int height=textContent.GetLength(0);
				int width=textContent.GetLength(1);
				int startY=editor_heightInDots/2-height/2;
				int startX=editor_widthInDots/2-width/2;
				for (int y=0; y<editor_heightInDots; y++) {
					for (int x=0; x<editor_widthInDots; x++) {
						if (x>=startX && x<startX+width && y>=startY && y<startY+height) {
							displayDots[y,x].setNewStateInstantly(textContent[y-startY,x-startX]);
						} else {
							displayDots[y,x].setNewStateInstantly(0);
						}
					}	
				}
			}
			// Clear border dots
			if (displayBorderDots!=null) {
				foreach (DisplayDot displayBorderDot in displayBorderDots) {
					displayBorderDot.setNewStateInstantly(0);
				}
			}
			// Broken dots for example
			setBrokenDots(editor_widthInDots,editor_heightInDots,true);
		}

		private void createDots(string groupObjectName, int width, int height, int borderDots) {

			displayDots=new DisplayDot[height,width];
			displayBorderDots=(borderDots>0?new List<DisplayDot>():null);

			GameObject groupObject=createGroupObject(groupObjectName,this.transform);

			GameObject borderObject=null;
			if (borderDots>0) {
				borderObject=createGroupObject("Border",groupObject.transform);
			}

			for (int y=-borderDots; y<height+borderDots; y++) {

				GameObject rowObject=null;
				if (y>=0 && y<height) {
					rowObject=createGroupObject("Row "+y,groupObject.transform);
				}

				for (int x=-borderDots; x<width+borderDots; x++) {

					GameObject dotObject=(GameObject)(Instantiate(dotPrefab));
					if (y>=0 && y<height && x>=0 && x<width) {
						dotObject.name="Col "+x;
						setParent(dotObject,rowObject.transform);
					} else {
						dotObject.name="Border dot";
						setParent(dotObject,borderObject.transform);
					}

					DisplayDot displayDot=createDisplayDot(dotObject,x,y);
					setDisplayDotScaleAndPosition(displayDot);
					displayDot.setNewStateInstantly(0);

					if (y>=0 && y<height && x>=0 && x<width) {
						displayDots[y,x]=displayDot;
					} else {
						displayBorderDots.Add(displayDot);
					}

				}

			}

		}

		private void setBrokenDots(int width, int height, bool staticRandomSeq) {
			// Default none broken
			for (int y=0; y<height; y++) {
				for (int x=0; x<width; x++) {
					displayDots[y,x].setBroken(-1);
				}
			}
			// Sanity check
			brokenAlwaysOffDots=Mathf.Clamp(brokenAlwaysOffDots,0,width*height);
			brokenAlwaysOnDots=Mathf.Clamp(brokenAlwaysOnDots,0,width*height);
			if (brokenAlwaysOffDots+brokenAlwaysOnDots>width*height) {
				brokenAlwaysOnDots=width*height-brokenAlwaysOffDots;
			}
			// Possibly add broken dots
			System.Random staticRandom=null;
			if (staticRandomSeq) {
				staticRandom=new System.Random(width*height*5);
			}
			for (int n=0; n<brokenAlwaysOffDots; n++) {
				addRandomBrokenDot(staticRandom,width,height,false);
			}
			if (staticRandomSeq) {
				staticRandom=new System.Random(width*height*7);
			}
			for (int n=0; n<brokenAlwaysOnDots; n++) {
				addRandomBrokenDot(staticRandom,width,height,true);
			}
		}

		private void addRandomBrokenDot(System.Random staticRandom, int width, int height, bool brokenState) {
			int x,y;
			do {
				x=(staticRandom!=null?staticRandom.Next(0,width):Random.Range(0,width));
				y=(staticRandom!=null?staticRandom.Next(0,height):Random.Range(0,height));
			} while (displayDots[y,x].getBroken()>=0);
			displayDots[y,x].setBroken( brokenState
			                            ? ( staticRandom!=null
			                                ? staticRandom.Next(1,this.getStateCount())
			                                : Random.Range(1,this.getStateCount()) )
			                            : 0 );
		}
		
		private void removeRandomBrokenDot(int width, int height, bool brokenState) {
			do {
				int x=Random.Range(0,width);
				int y=Random.Range(0,height);
				if (!brokenState && displayDots[y,x].getBroken()==0) {
					displayDots[y,x].setBroken(-1);
					return;
				}
				if (brokenState && displayDots[y,x].getBroken()>0) {
					displayDots[y,x].setBroken(-1);
					return;
				}
			} while (true);
		}

		private void destroyEditorDots(bool immediate) {
			foreach (Transform t in this.transform) {
				if (t.name.Equals(EDITOR_GROUP_NAME)) {
					if (immediate) {
						DestroyImmediate(t.gameObject);
					} else {
						Destroy(t.gameObject);
					}
				}
			}
		}

		internal abstract GameObject createGroupObject(string groupObjectName, Transform parent);
		internal abstract void setParent(GameObject gameObject, Transform parent);
		internal abstract DisplayDot createDisplayDot(GameObject dotObject, int x, int y);
		internal abstract void setDisplayDotScaleAndPosition(DisplayDot displayDot);

	}
	
}
