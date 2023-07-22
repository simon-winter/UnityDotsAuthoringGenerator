# UnityDotsAuthoringGenerator
 
This Visual Studio extension allows easy generation of Authorings with bakers for your Unity DOTS (ECS) ComponentData.

It adds a right click context menu to files, to generate an authoring component (including baker) for selected file.
The path where the generated file is saved to can be configured, there is an additional right click context menu entry for the settings.

This extension assumes, that you right click a clean IComponentData|ISharedComponentData|IBufferElementData containing file.
One struct per file is supported.
All fields from the component get mapped to the authoring, Entities are translated to GameObjects, BufferElementData does not copy the fields.

If anything is not working, you need support for special cases, or have an idea how to improve, jsut open an issue, i will ahve a look ;)