using UnityEngine;
using System.Collections;
using unity = UnityEngine.SceneManagement;

public enum Scene{
	game,
	menu
}

namespace Tode{
	public class SceneManager : UnitySingletonPersistent<SceneManager> {
		public GameObject pnlLoading;
		// Use this for initialization
		void Start () {
			StartCoroutine (InitData( callback => {
				pnlLoading.SetActive(!callback);
			}));
		}

		IEnumerator InitData(System.Action<bool> callback){
			callback (false);
			#if !UNITY_EDITOR
			DIContainer.BindModules ();
			#endif

			UserManager.Init ();
			DataManager.Init ();

			while (DataManager.Instance == null || UserManager.Instance == null){
				yield return null;
			}
			callback (true);
		}

		public void LoadScene(Scene scene){
			RealLoadScene (scene.ToString());
		}

		IEnumerator RealLoadScene(string scene){
			pnlLoading.SetActive (true);
			var op = unity.SceneManager.LoadSceneAsync (scene);
			while (!op.isDone) {
				yield return null;
			}
			pnlLoading.SetActive (false);
		}
	}
}
