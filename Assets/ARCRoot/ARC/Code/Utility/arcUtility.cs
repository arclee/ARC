using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

public static class arcUtility
{
	//http://stackoverflow.com/questions/78536/deep-cloning-objects-in-c-sharp
	public static T Clone<T>(T source)
	{
		if (!typeof(T).IsSerializable)
		{
			
			StackTrace stackTrace = new StackTrace(true);           // get call stack
			StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)
			StackFrame frame = stackFrames[1];

			arcErrCollector.Add("The type must be serializable: " + frame.GetMethod().DeclaringType.ToString()+ "." + frame.GetMethod().Name, null, frame.GetFileName());
			return default(T);
		}
		
		// Don't serialize a null object, simply return the default for that object
		if (System.Object.ReferenceEquals(source, null))
		{
			return default(T);
		}
		
		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();
		using (stream)
		{
			formatter.Serialize(stream, source);
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

	
	//由 X 當 forward 的 lookat.
	static public Matrix4x4 LookAtFromX(Vector3 eye, Vector3 target, Vector3 up)
	{
		//http://games.greggman.com/game/webgl-3d-cameras/
		//http://3dgep.com/?p=1700#The_View_Matrix
		//http://msdn.microsoft.com/en-us/library/windows/desktop/bb281710(v=vs.85).aspx

		Vector3 vx = eye - target; // The "forward".
		vx.Normalize();
		Vector3 vz = Vector3.Cross(up, vx);// The "right" .
		// vy doesn't need to be normalized because it's a cross
		// product of 2 normalized vectors
		Vector3 vy = Vector3.Cross(vz, vx);// The "up".
		Matrix4x4 inverseViewMatrix = new Matrix4x4();
		
		inverseViewMatrix.SetColumn(0, new Vector4(vx.x, vx.y, vx.z , 0));//
		inverseViewMatrix.SetColumn(1, new Vector4(vy.x, vy.y, vy.z , 0));//
		inverseViewMatrix.SetColumn(2, new Vector4(vz.x, vz.y, vz.z , 0));//用 right 當 forward.
		//inverseViewMatrix.SetColumn(3, new Vector4(-Vector3.Dot(vx, eye), -Vector3.Dot(vy, eye), -Vector3.Dot(vz, eye) , 1));
		inverseViewMatrix.SetColumn(3, new Vector4(eye.x, eye.y, eye.z , 1));
		
		return inverseViewMatrix.inverse;
	}

	//由 matrix 中 取出 quaternion.
	static public Quaternion QuaternionFromMatrix(Matrix4x4 m)
	{
		// Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
		Quaternion q = new Quaternion();
		q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] + m[1,1] + m[2,2] ) ) / 2; 
		q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] - m[1,1] - m[2,2] ) ) / 2; 
		q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] + m[1,1] - m[2,2] ) ) / 2; 
		q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] - m[1,1] + m[2,2] ) ) / 2; 
		q.x *= Mathf.Sign( q.x * ( m[2,1] - m[1,2] ) );
		q.y *= Mathf.Sign( q.y * ( m[0,2] - m[2,0] ) );
		q.z *= Mathf.Sign( q.z * ( m[1,0] - m[0,1] ) );
		return q;
	}

	//設定 GUI 用多少大小的畫面顯示, 再依目前畫面大小自動縮放.
	static public void GUIMatrixAutoScale(float fromScreenX, float fromScreenY)
	{
		Matrix4x4 _matrix = GUI.matrix;
		_matrix.m00 = (float)Screen.width / fromScreenX;
		_matrix.m11 = (float)Screen.height / fromScreenY;
		GUI.matrix = _matrix;
	}
}