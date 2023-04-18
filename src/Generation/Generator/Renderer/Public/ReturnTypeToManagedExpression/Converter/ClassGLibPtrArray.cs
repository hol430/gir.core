using GirModel;

namespace Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class ClassGLibPtrArray : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.IsGLibPtrArray<GirModel.Class>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
    {
        // fixme - correct ownership semantics
        return $"new GLib.PtrArray(new GLib.Internal.PtrArrayOwnedHandle({fromVariableName}))";
        // return returnType.Transfer == Transfer.None && returnType.AnyType.AsT1.Length == null
        //     ? $"GLib.Internal.StringHelper.ToStringArrayUtf8({fromVariableName})" //variableName is a pointer to a string array 
        //     : fromVariableName; //variableName is a string[]
    }
}
