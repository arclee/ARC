using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
//http://stackoverflow.com/questions/78536/deep-cloning-objects-in-c-sharp

public static class arcUtility
{
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
}