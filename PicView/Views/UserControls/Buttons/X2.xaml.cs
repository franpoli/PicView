﻿using PicView.Animations;
using PicView.PicGallery;
using PicView.UILogic;
using System.Windows;
using System.Windows.Controls;

namespace PicView.Views.UserControls
{
    /// <summary>
    /// Cool shady close button!
    /// </summary>
    public partial class X2 : UserControl
    {
        public X2()
        {
            InitializeComponent();
            MouseLeftButtonUp += (_, _) =>
            {
                if (GalleryFunctions.IsVerticalFullscreenOpen || GalleryFunctions.IsHorizontalFullscreenOpen)
                {
                    SystemCommands.CloseWindow(ConfigureWindows.GetMainWindow);
                }
                else if (GalleryFunctions.IsHorizontalOpen)
                {
                    PicView.PicGallery.GalleryToggle.CloseHorizontalGallery();
                }
                else if (Properties.Settings.Default.ShowInterface == false || Properties.Settings.Default.Fullscreen)
                {
                    if (UC.GetPicGallery is null || UC.GetPicGallery.IsVisible == false)
                    {
                        SystemCommands.CloseWindow(ConfigureWindows.GetMainWindow);
                    }
                }
            };

            MouseEnter += (_, _) =>
            {
                MouseOverAnimations.AltInterfaceMouseOver(PolyFill, CanvasBGcolor, BorderBrushKey);
            };

            MouseLeave += (_, _) =>
            {
                MouseOverAnimations.AltInterfaceMouseLeave(PolyFill, CanvasBGcolor, BorderBrushKey);
            };


        }
    }
}