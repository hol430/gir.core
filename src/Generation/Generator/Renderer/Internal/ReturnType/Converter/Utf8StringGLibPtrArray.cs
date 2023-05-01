﻿namespace Generator.Renderer.Internal.ReturnType;

internal class Utf8StringGLibPtrArray : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.IsGLibPtrArray<GirModel.Utf8String>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        var nullableTypeName = returnType switch
        {
            { Nullable: true, Transfer: GirModel.Transfer.None } => Model.Utf8String.GetInternalNullableUnownedHandleName(),
            { Nullable: false, Transfer: GirModel.Transfer.None } => Model.Utf8String.GetInternalNonNullableUnownedHandleName(),
            { Nullable: true, Transfer: GirModel.Transfer.Full } => Model.Utf8String.GetInternalNullableOwnedHandleName(),
            _ => Model.Utf8String.GetInternalNonNullableOwnedHandleName(),
        };

        // fixme
        // return new RenderableReturnType($"{nullableTypeName}[]");
        return new RenderableReturnType(Model.Type.Pointer);
    }
}
