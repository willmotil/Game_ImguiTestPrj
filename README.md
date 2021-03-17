# Game_ImguiTestPrj
 ImgUi Monogame quick example reference project Using the Imgui.net nuget 
 Copyed over from the imgui.net nuget xna sample using monogame.
 
 In this example project 

'use unsafe code' in the project properties box is checked on.

 the Imgui.net nuget is installed.
 
 the namesspaces are changed and thats pretty much it.


 .
 Quirks found...
 .

 When attempting to add a menubar

                 if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("Menu primary"))
                    {
                        ImGui.MenuItem("menubar primary menu item");
                        ImGui.EndMenu();
                    }

Beware that in order for it to work the window call itself has to have the additional parameters.

            if (ImGui.Begin("Menubar window")) {              // pretty much everything else will work but the menubar.

            if (ImGui.Begin("Menubar window", ref windowIsVisible, ImGuiWindowFlags.MenuBar)) {               // do this instead.

