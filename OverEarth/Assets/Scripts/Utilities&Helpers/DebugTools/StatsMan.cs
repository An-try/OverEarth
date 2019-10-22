using System.Text;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

[RequireComponent (typeof(Text))]
public class StatsMan : MonoBehaviour
{
	public Color tx_Color = Color.white;
	public bool HideGameObject = true;

	StringBuilder tx;

	[SerializeField]
	float updateInterval = 0.5f;
	float lastInterval;
	// Last interval end time
	float frames = 0;
	// Frames over current interval
	float framesavtick = 0;
	float framesav = 0.0f;

	public Text UiTextComponent;

	void Start ()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
		framesav = 0;
		tx = new StringBuilder ();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		UiTextComponent = GetComponent<Text> ();
		UiTextComponent.color = tx_Color;
	}

	void Update ()
	{
		++frames;

		var timeNow = Time.realtimeSinceStartup;

		if (timeNow > lastInterval + updateInterval) {

			float fps = frames / (timeNow - lastInterval);
			float ms = 1000.0f / Mathf.Max (fps, 0.00001f);

			++framesavtick;
			framesav += fps;
			float fpsav = framesav / framesavtick;

			tx.Length = 0;
			tx.Capacity = 0;

			tx.Append ("Time : ").Append (ms.ToString ("f1")).Append ("ms   ")
            .Append ("Current FPS : ").Append (fps.ToString ("f2"))
            .Append ("   AvgFps : ").Append (fpsav.ToString ("f2")).Append ('\n')
            .Append ('\n').Append ("GPU memory : ").Append (SystemInfo.graphicsMemorySize)
            .Append ("    Sys Memory : ").Append (SystemInfo.systemMemorySize)
				.Append ('\n').Append ("TotalAllocatedMemory : ").Append (Profiler.GetTotalAllocatedMemoryLong () / 1048576).Append ("mb")
				.Append ("   TotalReservedMemory : ").Append (Profiler.GetTotalReservedMemoryLong () / 1048576).Append ("mb")
				.Append ("   TotalUnusedReservedMemory : ").Append (Profiler.GetTotalUnusedReservedMemoryLong () / 1048576).Append ("mb");
            
			#if UNITY_EDITOR
			tx.Append ("\nBatches (DrawCalls) : ").Append (UnityStats.drawCalls).Append ('\n')
            .Append ("Used Texture Memory : ").Append (UnityStats.usedTextureMemorySize / 1024 / 1024).Append ("mb").Append ('\n')
            .Append ("renderedTextureCount : ").Append (UnityStats.usedTextureCount);
			#endif

			UiTextComponent.text = tx.ToString ();

			frames = 0;
			lastInterval = timeNow;
		}

	}
}


