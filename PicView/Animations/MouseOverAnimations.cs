﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PicView.Animations
{
    internal static class MouseOverAnimations
    {
        /*

            Adds MouseOver events for the given elements with the AnimationHelper.
            Changes color depending on the users settings.

        */

        private static readonly Color mainColor = (Color)Application.Current.Resources["IconColor"];

        private static readonly Color backgroundBorderColor = (Color)Application.Current.Resources["BackgroundColorAlt"];

        #region ALtInterface hover anims

        private static readonly ColorAnimation ccAnim = new ColorAnimation { Duration = TimeSpan.FromSeconds(.32) };
        private static readonly ColorAnimation ccAnim2 = new ColorAnimation { Duration = TimeSpan.FromSeconds(.2) };
        private static readonly SolidColorBrush borderBrush = (SolidColorBrush)Application.Current.Resources["BorderBrush"];

        internal static void AltInterfacePreviewMouseOver(Brush foreground, Brush border)
        {
            var alpha = AnimationHelper.GetPrefferedColorOver();
            ccAnim.From = alpha;
            ccAnim.To = AnimationHelper.GetPrefferedColorDown();
            foreground.BeginAnimation(SolidColorBrush.ColorProperty, ccAnim);
            AnimationHelper.MouseOverColorEvent(alpha.A, alpha.R, alpha.G, alpha.B, border, true);
        }

        internal static void AltInterfaceMouseOver(Brush foreground, Brush background, Brush border)
        {
            ccAnim.From = (Color)Application.Current.Resources["IconColor"];
            ccAnim.To = AnimationHelper.GetPrefferedColorOver();

            foreground.BeginAnimation(SolidColorBrush.ColorProperty, ccAnim);

            ccAnim2.From = (Color)Application.Current.Resources["AltInterface"];
            ccAnim2.To = (Color)Application.Current.Resources["AltInterfaceW"];

            background.BeginAnimation(SolidColorBrush.ColorProperty, ccAnim2);
            AnimationHelper.MouseOverColorEvent(
                borderBrush.Color.A,
                borderBrush.Color.R,
                borderBrush.Color.G,
                borderBrush.Color.B,
                border,
                true);
        }

        internal static void AltInterfaceMouseLeave(Brush foreground, Brush background, Brush border)
        {
            ccAnim.From = AnimationHelper.GetPrefferedColorOver();
            ccAnim.To = (Color)Application.Current.Resources["IconColor"];

            foreground.BeginAnimation(SolidColorBrush.ColorProperty, ccAnim);

            ccAnim2.From = (Color)Application.Current.Resources["AltInterfaceW"];
            ccAnim2.To = (Color)Application.Current.Resources["AltInterface"];

            background.BeginAnimation(SolidColorBrush.ColorProperty, ccAnim2);
            AnimationHelper.MouseLeaveColorEvent(
                borderBrush.Color.A,
                borderBrush.Color.R,
                borderBrush.Color.G,
                borderBrush.Color.B,
                border,
                true);
        }

        #endregion ALtInterface hover anims

        #region 1x

        internal static void PreviewMouseButtonDownAnim(Brush brush, bool alpha = false)
        {
            AnimationHelper.PreviewMouseLeftButtonDownColorEvent(brush, alpha);
        }

        internal static void ButtonMouseOverAnim(Brush brush, bool transparent = false, bool alpha = false)
        {
            if (transparent)
            {
                AnimationHelper.MouseOverColorEvent(0, 0, 0, 0, brush, alpha);
            }
            else
            {
                AnimationHelper.MouseOverColorEvent(
                    mainColor.A,
                    mainColor.R,
                    mainColor.G,
                    mainColor.B,
                    brush,
                    alpha
                );
            }
        }

        internal static void ButtonMouseLeaveAnim(Brush brush, bool transparent = false, bool alpha = false)
        {
            if (transparent)
            {
                AnimationHelper.MouseLeaveColorEvent(0, 0, 0, 0, brush, alpha);
            }
            else
            {
                AnimationHelper.MouseLeaveColorEvent(
                    mainColor.A,
                    mainColor.R,
                    mainColor.G,
                    mainColor.B,
                    brush,
                    alpha
                );
            }
        }

        internal static void ButtonMouseLeaveAnimBgColor(Brush brush, bool alpha = false)
        {
            AnimationHelper.MouseLeaveColorEvent(
                backgroundBorderColor.A,
                backgroundBorderColor.R,
                backgroundBorderColor.G,
                backgroundBorderColor.B,
                brush,
                alpha
            );
        }

        #endregion 1x

        #region 2x

        internal static void PreviewMouseButtonDownAnim(Brush brush, Brush brush2, bool alpha = false)
        {
            AnimationHelper.PreviewMouseLeftButtonDownColorEvent(brush, alpha);
            AnimationHelper.PreviewMouseLeftButtonDownColorEvent(brush2, alpha);
        }

        internal static void ButtonMouseOverAnim(Brush brush, Brush brush2, bool transparent = false, bool alpha = false)
        {
            ButtonMouseOverAnim(brush, transparent, alpha);
            ButtonMouseOverAnim(brush2, transparent, alpha);
        }

        internal static void ButtonMouseLeaveAnim(Brush brush, Brush brush2, bool transparent = false, bool alpha = false)
        {
            ButtonMouseLeaveAnim(brush, transparent, alpha);
            ButtonMouseLeaveAnim(brush2, transparent, alpha);
        }

        #endregion 2x

        #region 3x

        internal static void PreviewMouseButtonDownAnim(Brush brush, Brush brush2, Brush brush3, bool alpha = false)
        {
            AnimationHelper.PreviewMouseLeftButtonDownColorEvent(brush, alpha);
            AnimationHelper.PreviewMouseLeftButtonDownColorEvent(brush2, alpha);
            AnimationHelper.PreviewMouseLeftButtonDownColorEvent(brush3, alpha);
        }

        internal static void ButtonMouseOverAnim(Brush brush, Brush brush2, Brush brush3, bool transparent = false, bool alpha = false)
        {
            ButtonMouseOverAnim(brush, transparent, alpha);
            ButtonMouseOverAnim(brush2, transparent, alpha);
            ButtonMouseOverAnim(brush3, transparent, alpha);
        }

        internal static void ButtonMouseLeaveAnim(Brush brush, Brush brush2, Brush brush3, bool transparent = false, bool alpha = false)
        {
            ButtonMouseLeaveAnim(brush, transparent, alpha);
            ButtonMouseLeaveAnim(brush2, transparent, alpha);
            ButtonMouseLeaveAnim(brush3, transparent, alpha);
        }

        #endregion 3x
    }
}