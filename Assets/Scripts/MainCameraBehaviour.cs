using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MainCameraBehaviour : MonoBehaviour {

	#region Variables

	public Texture2D crosshair;
	public Vector2 crosshairOffset;
	public CursorMode cursorMode = CursorMode.Auto;

	public Vector2 positionOffset;

	//target follow
	public bool followTarget = true;
	private Player primaryTarget;

	//bound to terrain limits
	public bool boundToTerrain = true;
	private GameObject boundingTerrain;

	//camera shake
	public bool cameraShakeOn = true;

	//todo: encapsulate
	public AnimationCurve cameraShakeCurve;
	public float cameraShakeCurveEndTime;
	public float cameraShakeCurrentTime = 1000; //super dirty quick fix

	//camera zoom
	public AnimationCurve zoomingCurve;

	public static bool aimingMode = false;

	private float scrollWheelAxis;

	private Camera camera; //todo: use different variable name
	private Bounds terrainBounds;

	private float previousOrthographicSize;
	private float cameraTargetOrthographicSize;
	private Vector2 cameraTargetPosition;
	
	#endregion


	//Making sure we have the player game object referenced
	void Start()
	{
		camera = GetComponent<Camera>();

		if (primaryTarget == null)
		{
			//assuming we want to follow the player
			if ( Player.Instance != null ) {
				primaryTarget = Player.Instance;
			} else {
				Debug.LogWarning("Couldn't find a player game object to follow.");

				//don't even try to follow the non-existant target
				followTarget = false;
			}
		}

		boundingTerrain = GameObject.FindGameObjectWithTag("Terrain");

		if (boundingTerrain == null)
		{
			boundToTerrain = false;
			Debug.Log("Couldn't find a terrain to bound to. Make sure to link it in the inspector.");
		}
		else
		{
			OnTerrainSizeChange();
		}

		//restrict cursor to game window (very helpful for dual screens)
		Cursor.lockState = CursorLockMode.Confined;
		
		cameraTargetOrthographicSize = camera.orthographicSize;
		previousOrthographicSize = camera.orthographicSize;
		cameraTargetPosition = camera.transform.position;
	}

	private void Update()
	{	

		//zoom in and out with mouse wheel or keypad plus/minus
		scrollWheelAxis = Input.GetAxis("Mouse ScrollWheel");

		if (scrollWheelAxis < 0f || Input.GetKeyDown(KeyCode.KeypadMinus) && camera.orthographicSize < 2f)
		{
			cameraTargetOrthographicSize += .1f;
		}
		else if (scrollWheelAxis > 0f || Input.GetKeyDown(KeyCode.KeypadPlus) && camera.orthographicSize > .5f)
		{
			cameraTargetOrthographicSize -= .1f;
		}
		
	}

	//re-store bound extents if Terrain Size is changed (or at game startup)
	void OnTerrainSizeChange()
	{
		terrainBounds = boundingTerrain.GetComponent<Renderer>().bounds;

		//Debug.Log("TERRAIN BOUNDS V:" + terrainBounds.extents.x + " - H:" + terrainBounds.extents.y);
	}

	//LateUpdate() is used because "a follow camera should always be implemented in LateUpdate because it tracks objects that might have moved inside Update" - Unity LateUpdate() doc
	void LateUpdate()
	{

		//detect aiming mode
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			aimingMode = true;

			//store camera zoom prior, so we can go back to it when done with aiming
			previousOrthographicSize = camera.orthographicSize;

			//Debug.Log("PREVIOUS ZOOM:" + previousOrthographicSize);

			//new cursor for feedback and aim assist
			Cursor.SetCursor(crosshair, crosshairOffset, cursorMode);
		}
		else if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			aimingMode = false;

			//revert to previous camera zoom
			cameraTargetOrthographicSize = previousOrthographicSize;

			//Debug.Log("TARGET ZOOM:" + cameraTargetOrthographicSize);

			//revert to previous cursor
			Cursor.SetCursor(null, Vector2.zero, cursorMode);
		}

		//aiming camera logic upon right click	
		if (aimingMode)
		{
			//center camera between player and crosshair
			Vector2 mousePositionInSpace = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 centerPoint = ((Vector2)primaryTarget.transform.position + mousePositionInSpace) / 2;
			cameraTargetPosition = centerPoint;

			//adjust camera zoom to fit player and crosshair on each end of the screen
			Vector2 playerToCrosshair = mousePositionInSpace - (Vector2)primaryTarget.transform.position;

			if (playerToCrosshair.magnitude > Const.MIN_PLAYER_CROSSHAIR_DISTANCE && playerToCrosshair.magnitude < Const.MAX_PLAYER_CROSSHAIR_DISTANCE)
			{
				//TODO: Skip the clamped lerp process, simply use a curve with the correct metrics for simplicity

				//calculate the zoom ratio based on player distance ratio
				float zoomRatio = Mathf.InverseLerp(Const.MIN_PLAYER_CROSSHAIR_DISTANCE, Const.MAX_PLAYER_CROSSHAIR_DISTANCE, playerToCrosshair.magnitude);

				//using an animation curve to define the correct zoom levels
				zoomRatio = zoomingCurve.Evaluate(zoomRatio);

				cameraTargetOrthographicSize = Mathf.Lerp(Const.MIN_CAMERA_ZOOM_SIZE, Const.MAX_CAMERA_ZOOM_SIZE, zoomRatio);
			}
		}
		else if (followTarget)
		{ 
			//todo: apply smoothing over deltaTime if necessary
			cameraTargetPosition = (Vector2)primaryTarget.transform.position + positionOffset;
		}

		//adjust camera size and position, smoothly

		float cameraCalculatedOrthographicSize = Mathf.Lerp(camera.orthographicSize, cameraTargetOrthographicSize, Const.CAMERA_SMOOTHING_FACTOR);

		cameraTargetPosition = Vector3.Lerp(transform.position, (Vector3)cameraTargetPosition, Const.CAMERA_SMOOTHING_FACTOR);

		//if bound to terrain, camera position is clamped so as to not display empty void
		if (boundToTerrain)
		{
			//TODO: account for camera shake margins

			//update Orthographic Camera metrics if Screen Resolution is changed (or zoom)
			float verticalOrthographicSize = cameraCalculatedOrthographicSize;
			float horizontalOrthographicSize = verticalOrthographicSize * Screen.width / Screen.height;


			//calculate accurate bounds taking into account terrain size and position as well as screen size AND a margin for camera shake
			float cameraShakeMargin = Const.MAX_SHAKE_INTENSITY * Const.MAX_SHAKE_DISTANCE;

			float boundsVerticalMax = boundingTerrain.transform.position.y + terrainBounds.extents.y - verticalOrthographicSize - cameraShakeMargin;
			float boundsVerticalMin = boundingTerrain.transform.position.y - terrainBounds.extents.y + verticalOrthographicSize + cameraShakeMargin;

			float boundsHorizontalMax = boundingTerrain.transform.position.x + terrainBounds.extents.x - horizontalOrthographicSize - cameraShakeMargin;
			float boundsHorizontalMin = boundingTerrain.transform.position.x - terrainBounds.extents.x + horizontalOrthographicSize + cameraShakeMargin;

			//Debug.Log("BOUNDS ~ VMAX:" + boundsVerticalMax + " - VMIN:" + boundsVerticalMin + " - HMAX:" + boundsHorizontalMax + " - HMIN:" + boundsHorizontalMin);

			cameraTargetPosition.y = Mathf.Clamp(cameraTargetPosition.y, boundsVerticalMin, boundsVerticalMax);
			cameraTargetPosition.x = Mathf.Clamp(cameraTargetPosition.x, boundsHorizontalMin, boundsHorizontalMax);
		}

		//camera shake using Perlin Noise	
		if (cameraShakeOn)
		{

			if (cameraShakeCurrentTime < cameraShakeCurveEndTime)
			{
				float currentIntensity = Mathf.Clamp(cameraShakeCurve.Evaluate(cameraShakeCurrentTime), 0, Const.MAX_SHAKE_INTENSITY);

				cameraTargetPosition.x += Mathf.Lerp(-Const.MAX_SHAKE_DISTANCE * currentIntensity, Const.MAX_SHAKE_DISTANCE * currentIntensity, Mathf.PerlinNoise(Random.value, 0));
				cameraTargetPosition.y += Mathf.Lerp(-Const.MAX_SHAKE_DISTANCE * currentIntensity, Const.MAX_SHAKE_DISTANCE * currentIntensity, Mathf.PerlinNoise(Random.value, 0));

				cameraShakeCurrentTime += Time.deltaTime;
			}
		}

		camera.orthographicSize = cameraCalculatedOrthographicSize;
		transform.position = cameraTargetPosition;

	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

}
