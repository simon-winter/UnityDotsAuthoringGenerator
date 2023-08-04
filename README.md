# UnityDotsAuthoringGenerator
Can generate Authorings/Bakers from source files and can post configurable ECS templates.
If anything is not working, you need support for special cases, or have an idea how to improve, jsut open an issue, i will ahve a look ;)

# Automatic Authoring+Baker generation from source file
This Visual Studio extension allows easy generation of Authorings with bakers for your Unity DOTS (ECS) ComponentData.

It adds a right click context menu to files, to generate an authoring component (including baker) for selected file.
The path where the generated file is saved to can be configured, there is an additional right click context menu entry for the settings.

This extension assumes, that you right click a clean IComponentData|ISharedComponentData|IBufferElementData containing file.
One struct per file is supported.
All fields from the component get mapped to the authoring, Entities are translated to GameObjects, BufferElementData does not copy the fields.

- right click a file containing a single ComponentData or BufferElementData
- select 'Generate DOTS Authoring/Baker' command
- file gets generated nexto source file (configurable in settings)

# Template posting
Also it is possible to generate prefilled files or code snippets. Some defaults are included, but you can add/modify just by altering the example code file or add new ones.
Its all folder based and works with whatever it finds.

The generate template command can be also reached via a right click context menu on folders or files. 

- right click any file or folder
- select 'Create from template...' command
(- accept default installation)
- use 'Create from template...' command to post templates

## File tempaltes
The generated file will be inside the selected folder or nexto selected file.
A 'choose name' dialog will popup, which gets also posted into the template for every 'TEMPLATENAME_' string found inside (see examples ;)).
Files are generated with provided file name and posted into location and are added to the project.
All files inside the template file path folder are treated as template. The filename is used as name on the button.
'TEMPLATENAME_' can be added anywhere inside the file and will be replaced with the user choosen name.

## Snippet templates
Snippets are stored to clipboard when the button is pressed, ready to be pasted wherever you like.
You can have one or more files in the snippets path which get searched for snippets to be posted, whenever the 'Create from template command'
is opened.
Everything between the lines `// snippet MyName start` and `// snippet stop` will be added as snippet with given name.

## example generation
When there is no path set, eihter for files or snippets, the extension will offer to generate example files into a given folder.
This folder is then set in the settigns aswell as source for your files or snippets.
Having neither a snippets path nor a file path will trigger a installation dialog, which creates all examples in a project root based directory structure.

# Keyboard shortcuts
It is possible to use Visual Stduios built in shortcut manager to change/set shortcuts for the commands provided by this extension.
The defaults are:
- Generate Authoring: `ALT + G`
  
The shortcut manager is under `Tools > Options` search for `keyboard`.
All commands start with `DOTS` and can be found over that keyword easily:

![image](https://github.com/simon-winter/UnityDotsAuthoringGenerator/assets/34577718/bb3bcd1f-10ad-42e4-b094-7a3598439f02)

All template folders can be freely added/expanded for custom content.
