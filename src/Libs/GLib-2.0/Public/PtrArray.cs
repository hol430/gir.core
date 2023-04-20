using GLib.Internal;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace GLib;

public partial class PtrArray
{
	public uint Len
	{
		get
		{
			return GetData().Len;
		}
	}

	public T[] Pdata<T>()
	{
		nint[] pointers = GetPdataRaw();
		return pointers.MarshalToStructure<T>();
	}

	private nint[] GetPdataRaw()
	{
		PtrArrayData data = GetData();
		IntPtr ptr = data.Pdata;
		IntPtr[] result = new IntPtr[data.Len];
		for (uint i = 0; i < data.Len; i++)
			result[i] = Marshal.ReadIntPtr(new IntPtr(ptr.ToInt64() + i * Marshal.SizeOf(typeof(IntPtr))));
		return result;
	}

	private PtrArrayData GetData()
	{
		return Marshal.PtrToStructure<PtrArrayData>(_handle.DangerousGetHandle());
	}
}
