using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
public class NStarAssetPostprocessor : UnityEditor.AssetPostprocessor
{

    void OnPreprocessModel () {
		if(!assetPath.ToLower().Contains("shadow"))
		{
            UnityEditor.ModelImporter modelImporter  = assetImporter as UnityEditor.ModelImporter;
			modelImporter.normalSmoothingAngle = 180;
			
		}
	}

	void OnPreprocessAudio () {
        UnityEditor.AudioImporter audioImporter = (UnityEditor.AudioImporter) assetImporter;
		Debug.Log("OnPreprocessAudio " + assetImporter.name);
	}

	void OnPreprocessTexture (){
//		TextureImporter textImporter = (TextureImporter) assetImporter;
//		textImporter.textureType = TextureImporterType.Advanced;
//		textImporter.textureFormat = TextureImporterFormat.PVRTC_RGB4;
//		textImporter.compressionQuality = (int)TextureCompressionQuality.Best;
		Debug.Log("OnPreprocessTexture " + assetImporter.name);
	}

}
#endif