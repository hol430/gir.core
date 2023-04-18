using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GirTest.Tests;

[TestClass, TestCategory("BindingTest")]
public class GPtrArrayTest : Test
{
    [TestMethod]
    public void CheckReturnGPtrArray()
    {
		GirTest.GPtrArrayTester.ReturnGptrarray(213, 666);
		IntPtr raw = GirTest.Internal.GPtrArrayTester.ReturnGptrarray(213, 666);
		// GLib.Internal.PtrArrayManagedHandle arr = new GLib.Internal.PtrArrayManagedHandle(raw);
		// nint ptr = arr.DangerousGetHandle();
		GLib.Internal.PtrArrayData data = Marshal.PtrToStructure<GLib.Internal.PtrArrayData>(raw);
		Assert.AreEqual(2u, data.Len);

		GLib.PtrArray arr = new GLib.PtrArray(new GLib.Internal.PtrArrayOwnedHandle(raw));
		

		// Assert.AreEqual(2, ptrs.Length);
		
        // GPtrArrayTester.
    }
}
