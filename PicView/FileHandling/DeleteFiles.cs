﻿using Microsoft.VisualBasic.FileIO;
using PicView.ChangeImage;
using PicView.UILogic;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using static PicView.ChangeImage.Navigation;
using static PicView.UILogic.Tooltip;

namespace PicView.FileHandling
{
    internal static class DeleteFiles
    {
        /// <summary>
        /// Deletes the temporary files when an archived file has been opened
        /// </summary>
        internal static void DeleteTempFiles()
        {
            if (!Directory.Exists(ArchiveExtraction.TempFilePath))
            {
                return;
            }

            try
            {
                Array.ForEach(Directory.GetFiles(ArchiveExtraction.TempFilePath), File.Delete);
#if DEBUG
                Trace.WriteLine("Temp zip files deleted");
#endif
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                Directory.Delete(ArchiveExtraction.TempFilePath);
#if DEBUG
                Trace.WriteLine("Temp zip folder " + ArchiveExtraction.TempFilePath + " deleted");
#endif
            }
            catch (Exception)
            {
                return;
            }

            ArchiveExtraction.TempZipFile = ArchiveExtraction.TempFilePath = null;
        }

        /// <summary>
        /// Deletes file or send it to recycle bin
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Recycle"></param>
        /// <returns></returns>
        internal static bool TryDeleteFile(string file, bool Recycle)
        {
            /// Need to add function to remove from PicGallery
            if (!File.Exists(file))
            {
                return false;
            }

            try
            {
                var recycle = Recycle ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently;
                FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, recycle);
            }
#if DEBUG
            catch (Exception e)
            {
                Trace.WriteLine("Delete exception \n" + e.Message);
                return false;
            }
#else
            catch (Exception) {return false; }
#endif
            return true;
        }

        /// <summary>
        /// Delete file or move it to recycle bin, navigate to next pic
        /// and display information
        /// </summary>
        /// <param name="Recyclebin"></param>
        internal static async Task DeleteFileAsync(bool Recyclebin)
        {
            if (!TryDeleteFile(Pics[FolderIndex], Recyclebin))
            {
                ShowTooltipMessage(Application.Current.Resources["AnErrorOccuredWhenDeleting"] + Environment.NewLine + Pics[FolderIndex]);
                return;
            }

            // Sync with preloader
            Preloader.Remove(FolderIndex);

            await ConfigureWindows.GetMainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, () =>
            {
                // Sync with gallery
                if (UC.GetPicGallery is not null && UC.GetPicGallery.Container.Children.Count > ChangeImage.Navigation.FolderIndex)
                {
                    UC.GetPicGallery.Container.Children.RemoveAt(ChangeImage.Navigation.FolderIndex);
                }
            });

            Pics.Remove(Pics[FolderIndex]);

            if (Pics.Count <= 0)
            {
                Error_Handling.UnexpectedError();
                return;
            }

            await NavigateToPicAsync(false).ConfigureAwait(false);

            ShowTooltipMessage(Recyclebin ? Application.Current.Resources["SentFileToRecycleBin"] : Application.Current.Resources["Deleted"]);
        }
    }
}