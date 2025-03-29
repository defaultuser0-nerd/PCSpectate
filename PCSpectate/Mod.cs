using BepInEx;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PCSpectate
{
	[BepInPlugin("default.PCSpectate", "PCSpectate", "1.0.0")]
	public class Mod : BaseUnityPlugin
	{
		bool spectator, init;
		GameObject cameracube, cameracube2;
		Vector3 pos = new Vector3(-64.9141f, 12.2157f, -84.0814f);
		Quaternion rot = Quaternion.Euler(3.75f, 307.5f, 0);

		void Start()
		{
			GorillaTagger.OnPlayerSpawned(delegate
			{
				init = true;
			});
		}
		
		void Update()
		{
			if (!init) return;
			
			var tpc = GorillaTagger.Instance.thirdPersonCamera.transform.GetChild(0).gameObject;
			var liv = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/Main Camera/LCKBodyCameraSpawner(Clone)/CameraRenderModel/");

			if (spectator && !liv.activeSelf)
			{
				if (tpc.GetComponent<CinemachineBrain>().enabled) tpc.GetComponent<CinemachineBrain>().enabled = false;
				tpc.transform.position = pos;
				tpc.transform.rotation = rot;
				movement();
			}
			else
			{
				if (!tpc.GetComponent<CinemachineBrain>().enabled) tpc.GetComponent<CinemachineBrain>().enabled = true;
				if (cameracube.activeSelf) cameracube.SetActive(false);
				if (cameracube2.activeSelf) cameracube2.SetActive(false);
			}
			
			if (cameracube is null)
			{
				cameracube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cameracube.GetComponent<Collider>().Destroy();
				cameracube.transform.SetParent(tpc.transform);
				cameracube.transform.localScale = Vector3.one * .1f;
				cameracube.transform.localPosition = new Vector3(0, 0, -.05f);
				cameracube.transform.eulerAngles = tpc.transform.rotation.eulerAngles;
				cameracube.GetComponent<MeshRenderer>().material = new Material(Shader.Find("GorillaTag/UberShader"));
				cameracube.GetComponent<MeshRenderer>().material.color = Color.black;
			}
			if (cameracube2 is null)
			{
				cameracube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cameracube2.GetComponent<Collider>().Destroy();
				cameracube2.transform.SetParent(tpc.transform);
				cameracube2.transform.localScale = Vector3.one * .02f;
				cameracube2.transform.localPosition = Vector3.zero;
				cameracube2.transform.eulerAngles = tpc.transform.rotation.eulerAngles;
				cameracube2.GetComponent<MeshRenderer>().material = new Material(Shader.Find("GorillaTag/UberShader"));
				cameracube2.GetComponent<MeshRenderer>().material.color = Color.white;
			}
		}

		void movement()
		{
			var tpc = GorillaTagger.Instance.thirdPersonCamera.transform.GetChild(0).gameObject;
			
			if (Keyboard.current.wKey.isPressed) pos += tpc.transform.forward * 0.04f;
			if (Keyboard.current.sKey.isPressed) pos -= tpc.transform.forward * 0.04f;
			if (Keyboard.current.aKey.isPressed) pos -= tpc.transform.right * 0.04f;
			if (Keyboard.current.dKey.isPressed) pos += tpc.transform.right * 0.04f;
			if (Keyboard.current.spaceKey.isPressed) pos += tpc.transform.up * 0.04f;
			if (Keyboard.current.ctrlKey.isPressed) pos -= tpc.transform.up * 0.04f;
			
			if (Keyboard.current.leftArrowKey.isPressed) rot.eulerAngles += new Vector3(0, -0.75f, 0);
			if (Keyboard.current.rightArrowKey.isPressed) rot.eulerAngles += new Vector3(0, 0.75f, 0);
			if (Keyboard.current.downArrowKey.isPressed) rot.eulerAngles += new Vector3(0.75f, 0, 0);
			if (Keyboard.current.upArrowKey.isPressed) rot.eulerAngles += new Vector3(-0.75f, 0, 0);
		}
		void OnGUI()
		{
			if (GUI.Button(new Rect(0, 0, 120, 20), spectator ? "disable spec cam" : "enable spec cam")) spectator = !spectator;	
		}
	}
}
