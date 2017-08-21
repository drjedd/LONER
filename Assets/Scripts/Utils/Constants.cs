using System.Collections;
using UnityEngine;

/* 
 * CONST STATIC CLASS
 * 
 * Hosts all game-wide constants for easy access and changing of stuff
 * 
 */

public static class Const {

	public const float CAMERA_SMOOTHING_FACTOR = .1f;

	public const float MIN_PLAYER_CROSSHAIR_DISTANCE = .2f;
	public const float MAX_PLAYER_CROSSHAIR_DISTANCE = 5f;

	public const float MIN_CAMERA_ZOOM_SIZE = .5f;
	public const float MAX_CAMERA_ZOOM_SIZE = 2f;

	public const float MAX_SHAKE_INTENSITY = .4f;
	public const float MAX_SHAKE_DISTANCE = .1f;

	public const float SCENE_SWITCH_PADDING = .5f;

	public const float MAX_TIME_BETWEEN_DOUBLE_CLICK = 0.25f;

	public const float BULLET_LIFE_TIME = 3f;

	public const float THROW_UI_FADEOUT_TIME = 1f;

}
