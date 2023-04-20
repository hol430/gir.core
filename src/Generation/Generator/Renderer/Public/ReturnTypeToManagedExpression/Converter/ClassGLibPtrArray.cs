using GirModel;

namespace Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class ClassGLibPtrArray : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.IsGLibPtrArray<GirModel.Class>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
    {
        // fixme - correct ownership semantics
        return returnType.Transfer switch
        {
            Transfer.None => $"new GLib.PtrArray(new GLib.Internal.PtrArraySafeHandle({fromVariableName}, false, false))",
            Transfer.Container => $"new GLib.PtrArray(new GLib.Internal.PtrArraySafeHandle({fromVariableName}, true, false))",
            Transfer.Full => $"new GLib.PtrArray(new GLib.Internal.PtrArraySafeHandle({fromVariableName}, true, true))",
            _ => throw new System.Exception($"Unknown transfer type {returnType.Transfer}")
        };
        // return $"new GLib.PtrArray(new GLib.Internal.PtrArraySafeHandle({fromVariableName}))";
        // return returnType.Transfer == Transfer.None && returnType.AnyType.AsT1.Length == null
        //     ? $"GLib.Internal.StringHelper.ToStringArrayUtf8({fromVariableName})" //variableName is a pointer to a string array 
        //     : fromVariableName; //variableName is a string[]
    }
}
