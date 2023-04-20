using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable enable

namespace GLib.Internal;

public partial class PtrArraySafeHandle : PtrArrayHandle
{
	private readonly bool ownsHandle;
	private readonly bool ownsElements;

	public PtrArraySafeHandle(IntPtr ptr, bool ownsHandle, bool ownsElements) : base(ownsHandle)
	{
		this.ownsHandle = ownsHandle;
		this.ownsElements = ownsElements;
		SetHandle(ptr);
	}

    protected override bool ReleaseHandle()
    {
        if (ownsHandle)
		{
			Debug.Write($"Freeing PtrArrayHandle {handle}");
			if (ownsElements)
				Debug.WriteLine(" and array memory block too");
			else
				Debug.WriteLine(" but not array memory block");
			PtrArray.Free(handle, ownsElements);
		}
		return true;
    }
}
