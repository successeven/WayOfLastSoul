using UnityEditor;
using UnityEditor.SceneManagement;

namespace EckTechGames
{
		[InitializeOnLoad]
		public class AutoSaveExtension
		{
				// Static constructor that gets called when unity fires up.
				static AutoSaveExtension()
				{
						EditorApplication.playModeStateChanged += AutoSaveWhenPlaymodeStarts;
				}

				private static void AutoSaveWhenPlaymodeStarts(PlayModeStateChange state)
				{
						// If we're about to run the scene...
						//if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
						if (state == PlayModeStateChange.ExitingEditMode)
						{
								// Save the scene and the assets.
								EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
								AssetDatabase.SaveAssets();
						}
				}
		}
}