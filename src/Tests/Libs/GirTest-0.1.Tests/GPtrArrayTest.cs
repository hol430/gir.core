using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using GLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GirTest.Tests;

[TestClass, TestCategory("BindingTest")]
public class GPtrArrayTest : Test
{
	enum Transfer
	{
		Full,
		Container,
		None
	}

	private void CheckArray<T>(PtrArray array, params T[] expected)
	{
		Assert.AreEqual((uint)expected.Length, array.Len);
		T[] actual = array.Pdata<T>();
		Assert.AreEqual(expected.Length, actual.Length);
		for (int i = 0; i < expected.Length; i++)
			Assert.AreEqual(expected[i], actual[i]);
	}

	private PtrArray CreateArray(Transfer transfer, bool ownElements, int x, int y)
	{
		PtrArray arr = transfer switch
		{
			Transfer.Full => GPtrArrayTester.CreateArrayTransferFull(x, y, ownElements),
			Transfer.Container => GPtrArrayTester.CreateArrayTransferContainer(x, y, ownElements),
			Transfer.None => GPtrArrayTester.CreateArrayTransferNone(x, y, ownElements),
			_ => throw new Exception($"Unknown transfer type {transfer}")
		};
		return arr;
	}

	private PtrArray CreateAndCheck(Transfer transfer, bool ownElements, int x, int y)
	{
		PtrArray arr = CreateArray(transfer, ownElements, x, y);
		CheckArray(arr, x, y);
		return arr;
	}

	/// <summary>
	/// Create a GPtrArray with transfer:full and ensure that it can be read.
	/// </summary>
    [TestMethod]
    public void CheckReturnGPtrArray()
    {
		CreateAndCheck(Transfer.Full, true, 213, 666);
    }

	[TestMethod]
	public void CheckTransferFullDisposal()
	{
		WeakReference arrayReference = new WeakReference(null);
		WeakReference pdataReference = new WeakReference(null);
		CollectAfter(() =>
		{
			PtrArray arr = CreateAndCheck(Transfer.Full, true, 12, 23);
			arrayReference.Target = arr;
			pdataReference.Target = GetPdataPtr(arr);

			// Transfer full, so array and pdata should both be freed.
			// We also set the element_free_func to g_free, so the elements
			// should also have been freed.
		});
		// todo: find a way to ensure the native objects were freed. (This only
		// tests the managed objects.)
		arrayReference.IsAlive.Should().BeFalse();
		pdataReference.IsAlive.Should().BeFalse();
	}

	[TestMethod]
	public void CheckTransferContainerDisposal()
	{
		WeakReference arrayReference = new WeakReference(null);
		IntPtr pdata = IntPtr.Zero;

		CollectAfter(() =>
		{
			PtrArray arr = CreateAndCheck(Transfer.Container, true, 12, 23);
			arrayReference.Target = arr;

			// Grab pdata pointer. This will not be freed when the GPtrArray is
			// freed because we used transfer:container.
			pdata = GetPdataPtr(arr);
		});

		// Managed objects should have been GC'd.
		arrayReference.IsAlive.Should().BeFalse();

		// The pdata pointer should still be valid and readable at this point,
		// as the array should have been freed with free_seg set to false. We
		// can test this by creating a new PtrArray around the same pointer.
		//
		// Side note: could this still succeed even if pdata was freed? Reading
		// unallocated memory is undefined behaviour (?).

		GLib.Internal.PtrArrayData struc = new GLib.Internal.PtrArrayData();
		struc.Pdata = pdata;
		struc.Len = 2;

		// fixme - pdata is leaking
		var hnd = GLib.Internal.PtrArrayManagedHandle.Create(struc);
		PtrArray managed = new PtrArray(hnd);

		// Read array contents from pdata, ensure correctness.
		CheckArray(managed, 12, 23);
		GLib.Internal.Functions.Free(pdata);
	}

	[TestMethod]
	public void TestTransferNoneDisposal()
	{
		IntPtr ptr = IntPtr.Zero;

		CollectAfter(() =>
		{
			PtrArray arr = CreateAndCheck(Transfer.None, true, 42, -123);

			// Grab array pointer. We don't own the array, so this won't be
			// freed when the managed object is GC'd.
			ptr = arr.Handle.DangerousGetHandle();
		});

		// Ensure the GPtrArray was not freed.

		// Create a new handle with full ownership to ensure the memory is
		// freed after this test finishes. (Even though technically we don't own
		// this, the native code behind this test assumes we'll clean everything
		// up.)
		var hnd = new GLib.Internal.PtrArraySafeHandle(ptr, true, true);
		PtrArray arr = new PtrArray(hnd);
		CheckArray(arr, 42, -123);
	}

	[TestMethod]
	public void TestPassGPtrArrayTransferFull()
	{
		PtrArray arr = CreateAndCheck(Transfer.Full, true, 12345, -2);
		int res = GPtrArrayTester.GetElemTransferFull(arr, 0);
		Assert.AreEqual(12345, res);
		CheckArray(arr, 12345, -2); // This should fail.
	}

	/// <summary>
	/// Get the pdata pointer field for the given GPtrArray.
	/// </summary>
	private nint GetPdataPtr(PtrArray array)
	{
		nint hnd = array.Handle.DangerousGetHandle();
		return Marshal.PtrToStructure<GLib.Internal.PtrArrayData>(hnd).Pdata;
	}
}
