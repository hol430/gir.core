namespace Generator.Renderer.Internal.ReturnType;

internal class PointerGLibPtrArray : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.IsGLibPtrArray<GirModel.Pointer>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        return new RenderableReturnType(Model.Type.Pointer);
    }
}
