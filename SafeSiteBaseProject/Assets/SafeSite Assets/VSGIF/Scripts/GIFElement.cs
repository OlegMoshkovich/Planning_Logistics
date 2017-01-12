#pragma warning disable 0162 // code unreached.
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used
#pragma warning disable 0429 //never used

/*
 * Copyright (c) 2015 Thomas Hourdel
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 *    1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 
 *    2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 
 *    3. This notice may not be removed or altered from any source
 *    distribution.
 */




using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Reflection;
using ThreadPriority = System.Threading.ThreadPriority;

using UnityEngine.Networking;

#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif

namespace AppAdvisory.VSGIF
{
	using UnityObject = UnityEngine.Object;

	public class GIFElement : MonoBehaviour 
	{
		public GameObject GIF_CANVAS;
		public Image imageGIF;

		public Queue<RenderTexture> m_Frames;

		[NonSerialized] public List<Texture2D> m_listText = new List<Texture2D>();
		[NonSerialized] public List<Sprite> m_sprite = new List<Sprite>();

		#if UNITY_IPHONE
		[DllImport ("__Internal")]	
		private static extern void presentActivitySheetWithImageAndString(string message,byte[] imgData,int _length);

		[DllImport ("__Internal")]	
		private static extern void presentActivitySheetForFacebook(string message,string gifURL);

		[DllImport ("__Internal")]	
		private static extern void presentActivitySheetForTwitter(string message,byte[] imgData,int _length);

		[DllImport ("__Internal")]	
		private static extern void presentActivitySheetForWhatsapp(string message,byte[] imgData,int _length);
//		private static extern void presentActivitySheetForTwitter(string message,string gifURL);
		#endif

		#if APPADVISORY_ADS

		#if UNITY_ANDROID

		#if !VS_UI
		bool isAmazon
		{
		get
		{
		#if ANDROID_AMAZON
		return AdsManager.instance.adIds.isAmazon;
		#else
		return false;
		#endif
		}
		}
		#else
		bool isAmazon
		{
		get
		{
		return FindObjectOfType<UIController>().isAmazon;
		}
		}
		#endif

		#else
		bool isAmazon = false;
		#endif



		#else
		#if !VS_UI
		public bool isAmazon = false;
		#else
		bool isAmazon
		{
		get
		{
		return FindObjectOfType<UIController>().isAmazon;
		}
		}
		#endif
		#endif

		#if !VS_UI
		public string shareTextBeforeUrl = "Get it here for free: "; 
		public string shareTextAfterUrl = " #appadvisory"; 
		#else
		public string shareTextBeforeUrl
		{
		get
		{
		return FindObjectOfType<UIController>().shareTextBeforeUrl;
		}
		}
		public string shareTextAfterUrl
		{
		get
		{
		return FindObjectOfType<UIController>().shareTextAfterUrl;
		}
		}
		#endif

		#if !VS_UI
		/// <summary>
		/// URL of the iOS game. Find it on iTunes Connect.
		/// </summary>
		public string iD_iOS = "1166227384";
		public string url_ios
		{
			get
			{
				return "https://itunes.apple.com/us/app/" + iD_iOS; //1134939249
			}
		}
		/// <summary>
		/// URL of the Android game. Find it on Google Play.
		/// </summary>
		public string bundleIdAndroid = "com.appadvisory.insidethetube";
		public string url_android
		{
			get
			{
				return "https://play.google.com/store/apps/details?id=" + bundleIdAndroid; //1134939249
			}
		}
		/// <summary>
		/// URL of the Amazon game. Find it on the Amazon Developer Console.
		/// </summary>
		public string amazonID = "B01DPBSF2A";
		public string url_amazon
		{
			get
			{
				return "https://www.amazon.fr/dp/" + amazonID; //1134939249
			}
		}

		public string URL_STORE
		{
			get
			{
				string URL = "";

		#if UNITY_IOS
				URL = url_ios;
		#else
		URL = url_android;
		if(isAmazon)
		URL = url_amazon;
		#endif

				return URL;
			}
		}

		#endif

		string ShareText
		{
			get
			{
				#if !VS_UI
				return shareTextBeforeUrl + URL_STORE + shareTextAfterUrl;
				#else
				return shareTextBeforeUrl + FindObjectOfType<UIController>().URL_STORE + shareTextAfterUrl;
				#endif
			}
		}


		void Awake()
		{
			var gifElements = FindObjectsOfType<GIFElement>();
//			foreach(var g in gifElements)
//			{
//				if(g != this)
//				{
//					Destroy(g.GIF_CANVAS);
//					Destroy(g.gameObject);
//				}
//			}

			if(gifElements != null && gifElements.Length > 1)
			{
				Destroy(GIF_CANVAS);
				Destroy(gameObject);
				return;
			}

//			DontDestroyOnLoad(gameObject);

			m_ReflectionUtils = new ReflectionUtils<GIFElement>(this);
			m_Frames = new Queue<RenderTexture>();

			DisplaySpriteGIF(false);

			Init();
		}

		// Used to reset internal values, called on Start(), Setup() and FlushMemory()
		void Init()
		{
			State = RecorderState.Paused;
			m_MaxFrameCount = Mathf.RoundToInt(m_BufferSize * m_FramePerSecond);
			m_TimePerFrame = 1f / m_FramePerSecond;
			m_Time = 0f;
		}

		void OnEnable()
		{
			StopAllCoroutines();
		}

		void OnDisable()
		{
			StopAllCoroutines();
		}

		public void DisplaySpriteGIF(bool _activate)
		{
			GIF_CANVAS.SetActive(_activate);
		}

		public void StopAnimtextureAndDestroySprite()
		{
			DisplaySpriteGIF(false);

			FlushMemory();

			this.m_listText = new List<Texture2D>();
			this.m_sprite = new List<Sprite>();
		}

		public void DOStart(bool _andShowButton)
		{
			StopAllCoroutines();

			if(_andShowButton)
			{
				GIF_CANVAS.SetActive(true);
				StartCoroutine(AnimListTexture());
			}
		}

		public IEnumerator AnimListTexture()
		{
			DisplaySpriteGIF(true);

			while(true)
			{
				foreach(var s in m_sprite)
				{
					this.imageGIF.sprite = s;
//					this.imageGIF.SetNativeSize();

					float waitTime = 1f/((float)m_FramePerSecond);

					yield return new WaitForSeconds(waitTime);
				}

				yield return null;
			}
		}

		[SerializeField, Min(8)]
		public int m_Width = 256;

		public int m_Height
		{
			get
			{
				return Mathf.RoundToInt(m_Width / Camera.main.aspect);
			}
		}

		[NonSerialized] public bool workerIsDone = false;

		[SerializeField, Range(1, 30)]
		int m_FramePerSecond = 15;

		[SerializeField, Min(-1)]
		int m_Repeat = 0;

		[SerializeField, Range(1, 100)]
		int m_Quality = 70;

		[SerializeField, Min(0.1f)]
		float m_BufferSize = 3f;

		public RecorderState State { get; private set; }

		public ThreadPriority WorkerPriority = ThreadPriority.Highest;

		public float EstimatedMemoryUse
		{
			get
			{
				float mem = m_FramePerSecond * m_BufferSize;
				mem *= m_Width * m_Height * 4;
				mem /= 1024 * 1024;
				return mem;
			}
		}

		#region Delegates

		public Action OnPreProcessingDone;

		public Action<int, float> OnFileSaveProgress;

		public Action<int, string> OnFileSaved;

		#endregion

		#region Internal fields

		[NonSerialized] public int m_MaxFrameCount;
		[NonSerialized] public float m_Time;
		[NonSerialized] public float m_TimePerFrame;
		[NonSerialized] public RenderTexture m_RecycledRenderTexture;
		ReflectionUtils<GIFElement> m_ReflectionUtils;

		#endregion

		#region Public API

		/// <summary>
		/// Initializes the component. Use this if you need to change the recorder settings in a script.
		/// This will flush the previously saved frames as settings can't be changed while recording.
		/// </summary>
		/// <param name="autoAspect">Automatically compute height from the current aspect ratio</param>
		/// <param name="width">Width in pixels</param>
		/// <param name="height">Height in pixels</param>
		/// <param name="fps">Recording FPS</param>
		/// <param name="bufferSize">Maximum amount of seconds to record to memory</param>
		/// <param name="repeat">-1: no repeat, 0: infinite, >0: repeat count</param>
		/// <param name="quality">Quality of color quantization (conversion of images to the maximum
		/// 256 colors allowed by the GIF specification). Lower values (minimum = 1) produce better
		/// colors, but slow processing significantly. Higher values will speed up the quantization
		/// pass at the cost of lower image quality (maximum = 100).</param>
		public void Setup(bool autoAspect, int width, int height, int fps, float bufferSize, int repeat, int quality)
		{
			if (State == RecorderState.PreProcessing)
			{
				Debug.LogWarning("Attempting to setup the component during the pre-processing step.");
				return;
			}

			// Start fresh
			FlushMemory();

			m_ReflectionUtils.ConstrainMin(x => x.m_Width, width);

			if (autoAspect)
				m_ReflectionUtils.ConstrainMin(x => x.m_Height, height);

			m_ReflectionUtils.ConstrainRange(x => x.m_FramePerSecond, fps);
			m_ReflectionUtils.ConstrainMin(x => x.m_BufferSize, bufferSize);
			m_ReflectionUtils.ConstrainMin(x => x.m_Repeat, repeat);
			m_ReflectionUtils.ConstrainRange(x => x.m_Quality, quality);

			// Ready to go
			Init();
		}

		/// <summary>
		/// Pauses recording.
		/// </summary>
		public RecorderState Pause()
		{
			if (State == RecorderState.PreProcessing)
			{
				Debug.LogWarning("Attempting to pause recording during the pre-processing step. The recorder is automatically paused when pre-processing.");
				return State;
			}

			State = RecorderState.Paused;

			return State;
		}

		/// <summary>
		/// Starts or resumes recording. You can't resume while it's pre-processing data to be saved.
		/// </summary>
		public RecorderState Record()
		{
			if (State == RecorderState.PreProcessing)
			{
				Debug.LogWarning("Attempting to resume recording during the pre-processing step.");
				return State;
			}

			State = RecorderState.Recording;

			return State;
		}

		/// <summary>
		/// Clears all saved frames from memory and starts fresh.
		/// </summary>
		public void FlushMemory()
		{
			if (State == RecorderState.PreProcessing)
			{
				Debug.LogWarning("Attempting to flush memory during the pre-processing step.");
				return;
			}

			Init();

			if (m_RecycledRenderTexture != null)
				Flush(m_RecycledRenderTexture);

			if (m_Frames == null)
				return;

			foreach (RenderTexture rt in m_Frames)
				Flush(rt);

			m_Frames.Clear();
		}

		/// <summary>
		/// Saves the stored frames to a gif file. The filename will automatically be generated.
		/// Recording will be paused and won't resume automatically. You can use the 
		/// <code>OnPreProcessingDone</code> callback to be notified when the pre-processing
		/// step has finished.
		/// </summary>
		public void Save()
		{
			Save(GenerateFileName());
		}

		/// <summary>
		/// Saves the stored frames to a gif file. If the filename is null or empty, an unique one
		/// will be generated. You don't need to add the .gif extension to the name. Recording will
		/// be paused and won't resume automatically. You can use the <code>OnPreProcessingDone</code>
		/// callback to be notified when the pre-processing step has finished.
		/// </summary>
		/// <param name="filename">File name without extension</param>
		public void Save(string filename)
		{
			if (State == RecorderState.PreProcessing)
			{
				Debug.LogWarning("Attempting to save during the pre-processing step.");
				return;
			}

			if (m_Frames.Count == 0)
			{
				Debug.LogWarning("Nothing to save. Maybe you forgot to start the recorder ?");
				return;
			}

			State = RecorderState.PreProcessing;

			if (string.IsNullOrEmpty(filename))
				filename = GenerateFileName();

			StartCoroutine(PreProcess(filename));
		}

		#endregion

		#region Methods
		void Flush(UnityObject obj)
		{
			#if UNITY_EDITOR
			if (Application.isPlaying)
				Destroy(obj);
			else
				DestroyImmediate(obj);
			#else
			UnityObject.Destroy(obj);
			#endif
		}

		string GenerateFileName()
		{
			string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
			return "GifCapture-" + timestamp;
		}

		void SetListTexture2D(Texture2D temp)
		{ 
			Texture2D n = new Texture2D(m_Width, m_Height, TextureFormat.RGB24, true);
			n.SetPixels32(temp.GetPixels32());
			n.Apply();
			Sprite _s = Sprite.Create(n, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f)); 

			this.m_listText.Add(n);
			this.m_sprite.Add(_s);
		}

		private Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) 
		{
			Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
			Color[] rpixels=result.GetPixels(0);
			float incX=((float)1/source.width)*((float)source.width/targetWidth);
			float incY=((float)1/source.height)*((float)source.height/targetHeight);
			for(int px=0; px<rpixels.Length; px++) {
				rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth),
					incY*((float)Mathf.Floor(px/targetWidth)));
			}
			result.SetPixels(rpixels,0);
			result.Apply();
			return result;
		}

		Texture2D RoundCrop (Texture2D sourceTexture) 
		{
			int width = sourceTexture.width;
			int height = sourceTexture.height;
			float radius = (width < height) ? (width/2f) : (height/2f);
			float centerX = width/2f;
			float centerY = height/2f;
			Vector2 centerVector = new Vector2(centerX, centerY);

			// pixels are laid out left to right, bottom to top (i.e. row after row)
			Color[] colorArray = sourceTexture.GetPixels(0, 0, width, height);
			Color[] croppedColorArray = new Color[width*height]; 

			for (int row = 0; row < height; row++) {
				for (int column = 0; column < width; column++) {
					int colorIndex = (row * width) + column;
					float pointDistance = Vector2.Distance(new Vector2(column, row), centerVector);

					if (pointDistance < radius) {
						croppedColorArray[colorIndex] = colorArray[colorIndex];
					}
					else {
						croppedColorArray[colorIndex] = Color.clear;
					}
				}
			}

			Texture2D croppedTexture = new Texture2D(width, height);
			croppedTexture.SetPixels(croppedColorArray);
			croppedTexture.Apply();
			return croppedTexture; 
		}

		Texture2D SquareCrop (Texture2D sourceTexture) 
		{
			int width = sourceTexture.width;
			int height = sourceTexture.height;
			float centerX = width/2f;
			float centerY = height/2f;
			// pixels are laid out left to right, bottom to top (i.e. row after row)
			Color[] colorArray = sourceTexture.GetPixels(0, 0, width, height);
			Color[] croppedColorArray = new Color[width*height]; 

			for (int row = 0; row < height; row++) 
			{
				for (int column = 0; column < width; column++) 
				{
					int colorIndex = (row * width) + column;

					if (column > height / 2 || row > width / 2) 
					{
						croppedColorArray[colorIndex] = colorArray[colorIndex];
					}
					else 
					{
						croppedColorArray[colorIndex] = Color.clear;
					}
				}
			}

			Texture2D croppedTexture = new Texture2D(width, height);
			croppedTexture.SetPixels(croppedColorArray);
			croppedTexture.Apply();
			return croppedTexture; 
		}
			
		private string streamingGif = "";

		// Pre-processing coroutine to extract frame data and send everything to a separate worker thread
		IEnumerator PreProcess(string filename)
		{
			workerIsDone = false;

			streamingGif = appDataPath + "/" + filename + ".gif";

			//			streamingGif = 	System.IO.Path.Combine(appDataPath, "Raw") + "/" + filename + ".gif";



			print("appDataPath = " + appDataPath);
			print("filename = " + filename);
			print("streamingGif = appDataPath + \"/\" + filename + \".gif\" = " + streamingGif);

			List<GifFrame> frames = new List<GifFrame>(m_Frames.Count);

			// Get a temporary texture to read RenderTexture data
			Texture2D temp = new Texture2D(m_Width, m_Height, TextureFormat.RGB24, true);
			temp.hideFlags = HideFlags.HideAndDontSave;
			temp.wrapMode = TextureWrapMode.Clamp;
			temp.filterMode = FilterMode.Bilinear;
			temp.anisoLevel = 0;

			// Process the frame queue
			while (m_Frames.Count > 0)
			{
				var m_f = m_Frames.Dequeue();
				GifFrame frame = ToGifFrame(m_f, temp);
				frames.Add(frame);

				SetListTexture2D(temp);

				yield return null;
			}

			// Dispose the temporary texture
			Flush(temp);

			// Switch the state to pause, let the user choose to keep recording or not
			State = RecorderState.Paused;

			// Callback
			if (OnPreProcessingDone != null)
				OnPreProcessingDone();

			// Setup a worker thread and let it do its magic
			GifEncoder encoder = new GifEncoder(m_Repeat, m_Quality);
			encoder.SetDelay(Mathf.RoundToInt(m_TimePerFrame * 1000f));

			GIFMaker worker = new GIFMaker(WorkerPriority)
			{
				m_Encoder = encoder,
				m_Frames = frames,
				m_FilePath = streamingGif,
				m_OnFileSaved = OnFileSaved,
				m_OnFileSaveProgress = OnFileSaveProgress
			};

			worker.Start();

			this.DOStart(true);
		}

		public void ShareGIF(ShareType shareType)
		{
			print("GIFElement - ShareGIF");

			#if UNITY_IPHONE
			StartCoroutine(loadGifFromFile("file://localhost" + streamingGif, shareType));
			#endif

			#if UNITY_ANDROID
			StartCoroutine(loadGifFromFile("file://" + streamingGif, shareType));
			#endif

		}

IEnumerator UploadContribution(Byte[] bytes, Action<string> callbackGiphyURL)
{
	// url giphy : @"http://i.giphy.com/%@.gif"
	WWWForm form = new WWWForm();
	form.AddField("api_key", "dc6zaTOxFJmzC");
	form.AddBinaryData("file", bytes, "vsgif.gif", "image/gif");

	WWW w = new WWW("https://upload.giphy.com/v1/gifs", form);

	yield return w;


	print("check url = " + CheckWWW(w));
	print("url = " + w.text);
	print("error = " + w.error);
	print("url = " + w.url);
	print("url = " + w.ToString());

//	var giphyJson = GiphyJson.CreateFromJSON(w.url);
	var giphyJson = JsonUtility.FromJson<GiphyJson>(w.text);
	string urlGiphy = "http://i.giphy.com/" + giphyJson.data.id + ".gif";

	if(callbackGiphyURL != null)
		callbackGiphyURL(urlGiphy);

	/////
//	UnityWebRequest request =  UnityWebRequest.Post("https://upload.giphy.com/v1/gifs",form );
//	request.SetRequestHeader("api_key","dc6zaTOxFJmzC");
//
//	yield return request.Send();
//
//	var giphyJson = JsonUtility.FromJson<GiphyJson>(request.downloadHandler.text);
//	string urlGiphy = "http://i.giphy.com/" + giphyJson.data.id + ".gif";
	/////


//	Application.OpenURL(urlGiphy);
}

//void LogsRequest(UnityWebRequest request)
//{
//	print("request response = "  + request.responseCode);
//	print("request url = "  + request.url);
//	print("request tostring = "  + request.ToString());
//	print("request downloadhandler text = "  + request.downloadHandler.text);
//	print("request uploadHandler progress = "  + request.uploadHandler.progress);
//	print("request error = "  + request.error);
//
//	Dictionary<string, string> ccc =  request.GetResponseHeaders();
//
//	print("request Count = "  + request.GetResponseHeaders().Count);
//
//	foreach(var c in ccc)
//	{
//		print("" + c.Key + " ****** " + c.Value);
//	}
//}



public bool CheckWWW(WWW www)
{
	if (www == null)
		return false;

	if (string.IsNullOrEmpty (www.error))
		return false;

	return true;

}

		#endregion

		#region IO METHODS

		/// <summary>
		/// Gets the app streaming assets data path.
		/// </summary>
		/// <value>The app data path.</value>
		//		public static Uri UriAppDataPath {
		//			get {
		//				UriBuilder uriBuilder = new UriBuilder ();      
		//				uriBuilder.Scheme = "file";
		//				#if !UNITY_EDITOR
		//				uriBuilder.Path = System.IO.Path.Combine(appDataPath, "Raw");
		//				#else
		//				uriBuilder.Path = System.IO.Path.Combine (appDataPath, "StreamingAssets");
		//				#endif
		//				return uriBuilder.Uri;
		//			}
		//		}

		private static string appDataPath 
		{
			get
			{ 
				if(Application.isEditor)
					return Application.dataPath;
				else
					return Application.persistentDataPath; 
			}
		}

		byte[] imgBytes;
		uint imgLength;

		IEnumerator loadGifFromFile (string imagePath, ShareType shareType)
		{
			print("GIFElement - loadGifFromFile - start - imagePath = " + imagePath);

			while(!workerIsDone)
			{
				yield return null;
			}

			WWW www = new WWW (imagePath);
			yield return www;

			if (www.error == null) 
			{

				imgBytes = www.bytes;
				imgLength = (uint)imgBytes.Length;

				if(shareType == ShareType.Whatsapp)
				{
					#if UNITY_IPHONE 
					presentActivitySheetForWhatsapp("",imgBytes,imgBytes.Length);
					#endif

			#if UNITY_ANDROID
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
			intentObject.Call<AndroidJavaObject>("setType", "image/png");

			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "");

			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share");
			currentActivity.Call("startActivity", jChooser);
			#endif

				}
				else if(shareType != ShareType.Native)
				{
					StartCoroutine(UploadContribution(imgBytes, (string giphyURL) => {
						#if UNITY_IPHONE 
						if(shareType == ShareType.Facebook)
						{
//							presentActivitySheetForFacebook(ShareText,giphyURL);
							presentActivitySheetForFacebook("",giphyURL);

						}
						else if(shareType == ShareType.Twitter)
						{
//							presentActivitySheetForTwitter(ShareText,giphyURL);
							presentActivitySheetForTwitter(ShareText,imgBytes,imgBytes.Length);
//							presentActivitySheetForTwitter("","");
//							presentActivitySheetForTwitter("",giphyURL);

//							presentActivitySheetForTwitter(imagePath);
//							presentActivitySheetForTwitter(giphyURL);

//							presentActivitySheetWithImageAndString(giphyURL,imgBytes,imgBytes.Length);

							 
						}
						#endif 

						#if UNITY_ANDROID
				string ShareSubject = "GIF";
				string shareLink = giphyURL;
				string textToShare = ShareText;
				if (!Application.isEditor)
				{

					AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
					AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
					intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
					AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

					intentObject.Call<AndroidJavaObject>("setType", "text/plain");

					// intent.putExtra(Intent.EXTRA_SUBJECT, "Foo bar"); // NB: has no effect!
					intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("ACTION_SEND"), giphyURL);

					AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
					currentActivity.Call("startActivity", intentObject);
				}
						#endif
					}));
				}
				else
				{
					if (imgLength == 0) {
						Debug.Log ("Image bytes array is null");
						yield break;
					}

					print("GIFElement - loadGifFromFile - presentActivitySheetWithImageAndString");


					if(!Application.isEditor)
					{				
						#if UNITY_IPHONE
						presentActivitySheetWithImageAndString(ShareText,imgBytes,imgBytes.Length);
//						presentActivitySheetWithImageAndString("",imgBytes,imgBytes.Length);
						#endif

						#if UNITY_ANDROID

						AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
						AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

						intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
						AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

						AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
						intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
						intentObject.Call<AndroidJavaObject>("setType", "image/png");

						intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), ShareText);

						AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
						AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

						AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share");
						currentActivity.Call("startActivity", jChooser);

						#endif
					}
				}



			} else {
				print("GIFElement - loadGifFromFile - Error load image : " + www.error);
			}



			print("GIFElement - loadGifFromFile - end");
		}

		// Converts a RenderTexture to a GifFrame
		// Should be fast enough for low-res textures but will tank the framerate at higher res
		GifFrame ToGifFrame(RenderTexture source, Texture2D target)
		{
			// TODO: Experiment with Compute Shaders, it may be faster to return data from a ComputeBuffer
			// than ReadPixels

			RenderTexture.active = source;
			target.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
			target.Apply();

			this.m_listText.Add(target);

			RenderTexture.active = null;


			return new GifFrame() { Width = target.width, Height = target.height, Data = target.GetPixels32() };
		}

		#endregion

	}
}
