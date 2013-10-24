using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;


public class BookHandler : MonoBehaviour
{
	private Camera _targetCamera;

	private int _currentFrameIndex;

	private int _currentContentIndex;

	private int _currentChapterIndex;

	private int _currentPageIndex;


	public List<GameObject> Contents = new List<GameObject>();

	public float ContentDistance = 0.03125f;

	// Use this for initialization
	void Start()
	{
		_targetCamera = Camera.main;
		
		SetUpPage();
	}

	void SetUpPage()
	{
		var pageView = Camera.main.transform.FindChild("PageView");

		if (pageView)
		{
			foreach (var obj in Contents)
				Destroy(obj);

			Contents.Clear();

			for (int c = 0; c < pageView.transform.childCount; c++)
			{
				var child = pageView.transform.GetChild(c);
				// child.parent = null;
				Destroy(child.gameObject);
			}

			int frame = 0; // frame = row

			// FRAME:0 - TRIANGLES
			{
				var display1 = CreateContentDisplay(ContentBorderType.Triangle, ContentBorderAlignment.Left, 0, 0);
				display1.transform.parent = pageView;
				display1.transform.localScale = new Vector3(1f, 1f, 1f);
				display1.transform.localPosition = new Vector3(-ContentDistance, GetRowY(frame), 0f);
				Contents.Add(display1);

				var display2 = CreateContentDisplay(ContentBorderType.Triangle, ContentBorderAlignment.Right, 0, 1);
				display2.transform.parent = pageView;
				display2.transform.localScale = new Vector3(1f, 1f, 1f);
				display2.transform.localPosition = new Vector3(ContentDistance, -GetRowY(frame), 0f);
				Contents.Add(display2);

				frame++;
			}

			// FRAME:1 - RECTANGLE
			{
				var display3 = CreateContentDisplay(ContentBorderType.Rectangle, ContentBorderAlignment.Default, 1, 0);
				display3.transform.parent = pageView;
				display3.transform.localScale = new Vector3(1f, 1f, 1f);
				display3.transform.localPosition = new Vector3(-ContentDistance, -GetRowY(frame), 0f);
				Contents.Add(display3);
				frame++;
			}

			// FRAME:2 - BOXES
			{
				// FILL
				var display4 = CreateContentDisplay(ContentBorderType.Square, ContentBorderAlignment.Default, 1, 1);
				display4.transform.parent = pageView;
				display4.transform.localScale = new Vector3(1f, 1f, 1f);
				display4.transform.localPosition = new Vector3(-ContentDistance, -GetRowY(frame), 0f);
				Contents.Add(display4);

				var display5 = CreateContentDisplay(ContentBorderType.Square, ContentBorderAlignment.Default, 2, 0);
				display5.transform.parent = pageView;
				display5.transform.localScale = new Vector3(1f, 1f, 1f);
				display5.transform.localPosition = new Vector3(1f + ContentDistance, -GetRowY(frame), 0f);
				//display5.transform.position = new Vector3(1f + ContentDistance, -GetRowY(frame), 0f);
				Contents.Add(display5);
			}
		}
	}

	float GetRowY(int frame)
	{
		return (frame * ContentDistance) + frame;
	}


	GameObject CreateContentDisplay(ContentBorderType type, ContentBorderAlignment align, int frameIndex, int contentIndex)
	{
		var contentDisplay = new GameObject("ContentDisplay_" + frameIndex + "_" + contentIndex, typeof(MeshRenderer), typeof(MeshFilter));

		var size = new Size(1, 1);
		if (type == ContentBorderType.Triangle)
			size = new Size(2, 1);
		else if (type == ContentBorderType.Rectangle)
			size = new Size(2 + (ContentDistance * 2f), 1);

		contentDisplay.GetComponent<MeshFilter>().mesh = CreateContentMesh(size, type, align);

		// Set texture
		var tex = (Texture)Resources.Load("ContentTexture_" + ((frameIndex * 2) + contentIndex));
		contentDisplay.renderer.material.mainTexture = tex;

		// Set shader for this sprite; unlit supporting transparency
		// If we dont do this the sprite seems 'dark' when drawn. 
		var shader = Shader.Find("Diffuse");

		contentDisplay.renderer.material.shader = shader;

		contentDisplay.transform.renderer.material.mainTextureOffset = new Vector2(0, 1); // +0.1x
		contentDisplay.transform.renderer.material.mainTextureScale = new Vector2(1, -1);

		// contentDisplay.transform.rotation = new Quaternion(0, 0, 180, 0);

		// contentDisplay.AddComponent<Material>()

		return contentDisplay;
	}

	Mesh CreateContentMesh(Size size, ContentBorderType contentType, ContentBorderAlignment alignment = ContentBorderAlignment.Default, IndexWindingOrder windingOrder = IndexWindingOrder.Clockwise)
	{
		var mesh = new Mesh();

		var vertices = new Vector3[4];
		var uv = new Vector2[4];
		var indices = new int[vertices.Length + 2];

		if (contentType == ContentBorderType.Triangle)
		{
			vertices = new Vector3[3];
			uv = new Vector2[3];
			if (alignment == ContentBorderAlignment.Left)
			{
				SetVertices(ref vertices,
					new Vector3(0, 0, 0),
					new Vector3(0, size.Height, 0),
					new Vector3(size.Width, size.Height, 0));

				SetUVs(ref uv, new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0));

				SetIndices(ref indices, 0, 1, 2);
			}
			else
			{
				SetVertices(ref vertices,
					new Vector3(0, 0, 0),
					new Vector3(size.Width, size.Height, 0),
					new Vector3(size.Width, 0, 0));

				SetUVs(ref uv, new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1));

				SetIndices(ref indices, 0, 1, 2);
			}
		}
		else if (contentType == ContentBorderType.Rectangle || contentType == ContentBorderType.Square)
		{
			SetVertices(ref vertices,
				new Vector3(0, 0, 0),
				new Vector3(0, size.Height, 0),
				new Vector3(size.Width, size.Height, 0),
				new Vector3(size.Width, 0, 0));

			SetUVs(ref uv, new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1));

			SetIndices(ref indices, 0, 1, 2, 0, 2, 3);
		}
		if (windingOrder == IndexWindingOrder.CounterClockWise)
			indices = Reverse(indices);

		mesh.vertices = vertices;
		mesh.uv = (uv);
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		mesh.RecalculateNormals();
		return mesh;
	}

	T[] Reverse<T>(T[] array)
	{
		var reversed = new T[array.Length];
		int i = 0;
		for (int j = array.Length - 1; j > 0; j--)
		{
			reversed[i++] = array[j];
		}
		return reversed;
	}

	void SetIndices(ref int[] stack, params int[] indices)
	{
		for (int j = 0; j < indices.Length; j++)
		{
			stack[j] = indices[j];
		}
	}
	void SetVertices(ref Vector3[] stack, params Vector3[] vertices)
	{
		for (int j = 0; j < vertices.Length; j++)
		{
			stack[j] = vertices[j];
		}
	}
	void SetUVs(ref Vector2[] stack, params Vector2[] uv)
	{
		for (int j = 0; j < uv.Length; j++)
		{
			stack[j] = uv[j];
		}
	}

	public void SetCurrentFrame(int index)
	{
		_currentFrameIndex = index;
	}

	public void SetCurrentContent(int index)
	{
		_currentContentIndex = index;
	}

	public void SetCurrentPage(int index)
	{
		_currentPageIndex = index;
	}

	public void SetCurrentChapter(int index)
	{
		_currentChapterIndex = index;
	}

	public Camera GetContentCamera(int chapterIndex, int pageIndex, int frameIndex, int contentIndex)
	{
		return this.gameObject.transform
			   .GetChild(chapterIndex)
			   .GetChild(pageIndex)
			   .GetChild(frameIndex)
			   .GetChild(contentIndex).Find("Camera").gameObject.camera;
	}


	public Camera GetCurrentContentCamera()
	{
		return GetContentCamera(_currentChapterIndex, _currentPageIndex, _currentFrameIndex, _currentContentIndex);
	}

	public void RecursiveDisableCameras(Transform current)
	{
		try
		{
			var cam = current.Find("Camera").gameObject.camera;

			if (cam)
			{
				cam.enabled = false;
			}
		}
		catch (Exception)
		{
			Debug.Log("Recursive Camera Disable Error");
		}
		for (var j = 0; j < current.childCount; j++)
		{
			Debug.Log("loooop");
			var chi = current.GetChild(j);
			if (chi)
			{
				RecursiveDisableCameras(chi);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnDrawGizmos()
	{
		// Gizmos.DrawIcon(transform.position, "Light Gizmo.tiff");

		// Graphics.DrawProcedural();

	}
}

public struct Size
{
	public float Width;

	public float Height;

	public Size(float w, float h)
	{
		this.Width = w;
		this.Height = h;
	}
}
public enum IndexWindingOrder
{
	Clockwise,
	CounterClockWise
}
public enum ContentBorderAlignment
{
	Default, Left, Right
}
public enum ContentBorderType
{
	Triangle,
	Rectangle,
	Square
}
