namespace Generator.Renderer.Internal.ReturnType;

internal class ClassGLibPtrArray : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.IsGLibPtrArray<GirModel.Class>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        return new RenderableReturnType(Model.Type.Pointer);
    }
}
