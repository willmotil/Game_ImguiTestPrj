
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Num = System.Numerics;

namespace Game_ImguiTestPrj
{
    using gui = ImGui;

    /// <summary>
    /// Simple FNA + ImGui example
    /// </summary>
    public class Game2 : Game
    {
        private GraphicsDeviceManager graphics;
        private ImGuiRenderer imGuiRenderer;

        private Texture2D xnaTexture;
        private IntPtr imGuiTexture;

        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            Window.AllowUserResizing = true;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 1200;
            graphics.ApplyChanges();
            imGuiRenderer = new ImGuiRenderer(this);
            imGuiRenderer.RebuildFontAtlas();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Texture loading example

            // First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
            xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
            {
                var red = (pixel % 300) / 2;
                return new Color(red, 1, 1);
            });

            // Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
            imGuiTexture = imGuiRenderer.BindTexture(xnaTexture);

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(clear_color.X, clear_color.Y, clear_color.Z));

            imGuiRenderer.BeforeLayout(gameTime); // Call BeforeLayout first to set things up
            ImGuiLayout(); // Draw our UI
            imGuiRenderer.AfterLayout(); // Call AfterLayout now to finish up and draw all the things

            base.Draw(gameTime);
        }

        // Direct port of the example at https://github.com/ocornut/imgui/blob/master/examples/sdl_opengl2_example/main.cpp

        private bool windowIsVisible = false;
        private float sliderFloat = 0.0f;
        string sliderFloatString = "";
        private bool show_test_window = false;
        private bool show_another_window = false;
        private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
        private byte[] textBuffer = new byte[100];

        int currentSelectedItem = 0;
        string[] itemList = new string[] { "listBoxItemA", "listBoxItemB", "listBoxItemC" };
        bool[] isCurrentSelectedItems = new bool[] { false, false, false };
        string comboBoxMultiSelectedTextPreviewValue = "Multi Selectable Combo #of selected items: 0";

        int currentSelectedRadioButton = 0;

        bool isCheckBoxToggledOn = false;

        //
        // https://pthom.github.io/imgui_manual_online/manual/imgui_manual.html
        //
        protected virtual void ImGuiLayout()
        {
            // 1. Show a simple window
            // Tip: if we don't call ImGui.Begin()/ImGui.End() the widgets appears in a window automatically called "Debug"
            //if (ImGui.Begin("Window"))  {

            // ~~~~~~~~~~~~~~~~~~~~~~~
            // Apllication Menu ....
            // ~~~~~~~~~~~~~~~~~~~~~~~
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open", "Ctrl+O"))
                    {
                        //openmodal = true;
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            // ~~~~~~~~~~~~~~~~~~~~~~~
            // New Window ....
            // https://github.com/ocornut/imgui/blob/a1a39c632aaf2a1ab7fe09961748a2f25816fb6e/imgui_demo.cpp#L339
            // ~~~~~~~~~~~~~~~~~~~~~~~
            ImGui.StyleColorsLight();
            if (ImGui.Begin("Menubar window",  ImGuiWindowFlags.MenuBar))  // the boolean places a exit window button on the corner.
            {

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // Menu Bar ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                //ImGui.PushID(0);
                ImGui.PushStyleColor(0, new Num.Vector4(1f, .5f, .5f, .9f));  // 0 is text.
                ImGui.PushStyleColor(ImGuiCol.Border, new Num.Vector4(.5f, 1f, .5f, .9f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Num.Vector4(.75f, 1f, .75f, .99f));
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new Num.Vector4(.95f, 25f, .99f, .99f));
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("Menu primary"))
                    {
                        ImGui.MenuItem("menubar primary menu itemA");
                        ImGui.EndMenu();
                    }
                    if (ImGui.BeginMenu("Examples"))
                    {
                        ImGui.MenuItem("menubar Examples menu itemA");
                        ImGui.MenuItem("menubar Examples menu itemB");
                        ImGui.EndMenu();
                    }
                    if (ImGui.BeginMenu("Tools"))
                    {
                        ImGui.MenuItem("menubar tools menu itemA");
                        ImGui.MenuItem("menubar tools menu itemB");
                        ImGui.MenuItem("menubar tools menu itemC");
                        ImGui.EndMenu();
                    }

                    ImGui.EndMenuBar();
                }
                ImGui.PopStyleColor();
                ImGui.PopStyleColor();
                ImGui.PopStyleColor();
                ImGui.PopStyleColor();

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // text label ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                ImGui.Text("Hello, world!");
                ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // button ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                if (ImGui.Button("Button"))
                {

                }

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // text input ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                ImGui.InputText("Text input", textBuffer, 100);

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // slider ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                if (ImGui.SliderFloat("float", ref sliderFloat, 0.0f, 1.0f, string.Empty))
                { // the bool return means that this block is only entered when the user is actively changing it.
                    sliderFloatString = "Slider Float: " + sliderFloat.ToString();
                }
                ImGui.Text(sliderFloatString);

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // color edit ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                ImGui.ColorEdit3("clear color", ref clear_color);

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // checkbox / toggle box ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                ImGui.Checkbox("CheckBox", ref isCheckBoxToggledOn);


                // ~~~~~~~~~~~~~~~~~~~~~~~
                // Radio box ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                if (ImGui.RadioButton("Radio button label", ref currentSelectedRadioButton, 0))
                {

                }

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // combo item select....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                ImGui.Combo("combolabel", ref currentSelectedItem, itemList, itemList.Length, 10);

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // multiple selectable combo items ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                int tmpcounter = 0;
                if (ImGui.BeginCombo("Multi Selectable Combo", comboBoxMultiSelectedTextPreviewValue))
                {
                    for (int i = 0; i < itemList.Length; i++)
                    {
                        ImGui.Selectable(itemList[i], ref isCurrentSelectedItems[i], ImGuiSelectableFlags.DontClosePopups);

                        if (isCurrentSelectedItems[i] == true)
                            tmpcounter++;
                        comboBoxMultiSelectedTextPreviewValue = "Multi Selectable Combo #of selected items: " + tmpcounter.ToString();
                    }
                    ImGui.EndCombo();
                }

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // Tab items ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                if (ImGui.BeginTabBar("table bar string id", ImGuiTabBarFlags.None))
                {
                    if (ImGui.BeginTabItem("tabItemA"))
                    {
                        ImGui.Text("Tab item A text label0");
                        ImGui.Text("Tab item A text label2");

                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("tabItemB"))
                    {
                        ImGui.Text("Tab item B text label0");
                        ImGui.Text("Tab item B text label2");
                        ImGui.Text("Tab item B text label3");
                        ImGui.Text("Tab item B text label4");

                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("tabItemC"))
                    {
                        ImGui.Text("Tab item B text label0");

                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // list box headers and stuff ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                //ImGui.ListBoxHeader("ListBoxHeader", 2, 4);
                //ImGui.ListBox("ListBox", ref currentSelectedItem, itemList, itemList.Length);
                //ImGui.ListBoxFooter();

                // ~~~~~~~~~~~~~~~~~~~~~~~
                // texture box ....
                // ~~~~~~~~~~~~~~~~~~~~~~~
                ImGui.Text("Texture sample");
                ImGui.Image(imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One, Num.Vector4.One, Num.Vector4.One); // Here, the previously loaded texture is used

                
                
                
                // ~~~~~~~~~~~~~~~~~~~~~~~
                // Default example stuff ....
                // ~~~~~~~~~~~~~~~~~~~~~~~

                // 2.
                if (ImGui.Button("Another Window"))
                    show_another_window = !show_another_window;

                // 3.
                if (ImGui.Button("Test Window"))
                    show_test_window = !show_test_window;

                ImGui.End();
            }

            // 2. Show another simple window, this time using an explicit Begin/End pair
            if (show_another_window)
            {
                ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
                ImGui.Begin("Another Window", ref show_another_window);
                ImGui.Text("Hello");
                ImGui.End();
            }

            // 3.Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
            if (show_test_window)
            {
                ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref show_test_window);
            }
        }

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            var texture = new Texture2D(device, width, height);
            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (var pixel = 0; pixel < data.Length; pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }
            //set the color
            texture.SetData(data);
            return texture;
        }
    }
}