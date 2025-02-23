﻿using PicView.ChangeImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static PicView.FileHandling.ArchiveExtraction;

namespace PicView.FileHandling
{
    internal static class FileLists
    {
        internal enum SortFilesBy
        {
            Name,
            FileSize,
            Creationtime,
            Extension,
            Lastaccesstime,
            Lastwritetime,
            Random
        }

        /// <summary>
        /// Sort and return list of supported files
        /// </summary>
        internal static List<string>? FileList(FileInfo fileInfo) => Properties.Settings.Default.SortPreference switch
        {
            0 => FileList(fileInfo, SortFilesBy.Name),
            1 => FileList(fileInfo, SortFilesBy.FileSize),
            2 => FileList(fileInfo, SortFilesBy.Creationtime),
            3 => FileList(fileInfo, SortFilesBy.Extension),
            4 => FileList(fileInfo, SortFilesBy.Lastaccesstime),
            5 => FileList(fileInfo, SortFilesBy.Lastwritetime),
            6 => FileList(fileInfo, SortFilesBy.Random),
            _ => FileList(fileInfo, SortFilesBy.Name),
        };

        /// <summary>
        /// Sort and return list of supported files
        /// </summary>
        private static List<string>? FileList(FileInfo fileInfo, SortFilesBy sortFilesBy)
        {
            if (fileInfo is null) { return null; }

            SearchOption searchOption;

            if (Properties.Settings.Default.IncludeSubDirectories && string.IsNullOrWhiteSpace(TempZipFile)) // Don't search subdirectories when zipped
            {
                searchOption = SearchOption.AllDirectories;
            }
            else
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }

            string? directory = FileFunctions.CheckIfDirectoryOrFile(fileInfo.FullName) ? fileInfo.FullName : fileInfo.DirectoryName;
            if (directory == null) { return null; }

            var items = Directory.EnumerateFiles(directory, "*.*", searchOption)
                .AsParallel()
                .Where(file => SupportedFiles.IsSupportedExt(file)
            );

            switch (sortFilesBy)
            {
                default:
                case SortFilesBy.Name: // Alphanumeric sort
                    var list = items.ToList();
                    if (Properties.Settings.Default.Ascending)
                    {
                        list.Sort((x, y) => { return SystemIntegration.NativeMethods.StrCmpLogicalW(x, y); });
                    }
                    else
                    {
                        list.Sort((x, y) => { return SystemIntegration.NativeMethods.StrCmpLogicalW(y, x); });
                    }
                    return list;

                case SortFilesBy.FileSize:
                    if (Properties.Settings.Default.Ascending)
                    {
                        return items.OrderBy(f => new FileInfo(f).Length).ToList();
                    }
                    else
                    {
                        return items.OrderByDescending(f => new FileInfo(f).Length).ToList();
                    }

                case SortFilesBy.Extension:
                    if (Properties.Settings.Default.Ascending)
                    {
                        return items.OrderBy(f => new FileInfo(f).Extension).ToList();
                    }
                    else
                    {
                        return items.OrderByDescending(f => new FileInfo(f).Extension).ToList();
                    }

                case SortFilesBy.Creationtime:
                    if (Properties.Settings.Default.Ascending)
                    {
                        return items.OrderBy(f => new FileInfo(f).CreationTime).ToList();
                    }
                    else
                    {
                        return items.OrderByDescending(f => new FileInfo(f).CreationTime).ToList();
                    }

                case SortFilesBy.Lastaccesstime:
                    if (Properties.Settings.Default.Ascending)
                    {
                        return items.OrderBy(f => new FileInfo(f).LastAccessTime).ToList();
                    }
                    else
                    {
                        return items.OrderByDescending(f => new FileInfo(f).LastAccessTime).ToList();
                    }

                case SortFilesBy.Lastwritetime:
                    if (Properties.Settings.Default.Ascending)
                    {
                        return items.OrderBy(f => new FileInfo(f).LastWriteTime).ToList();
                    }
                    else
                    {
                        return items.OrderByDescending(f => new FileInfo(f).LastWriteTime).ToList();
                    }

                case SortFilesBy.Random:
                    return items.OrderBy(f => Guid.NewGuid()).ToList();
            }
        }

        /// <summary>
        /// Gets values and extracts archives
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Task RetrieveFilelistAsync(FileInfo fileInfo) => Task.Run(async () =>
        {
            if (fileInfo is null)
            {
                await Error_Handling.ReloadAsync(true).ConfigureAwait(false);
                return;
            }
            // Check if to load from archive
            if (SupportedFiles.IsSupportedArchives(fileInfo.FullName))
            {
                if (!Extract(fileInfo.FullName)) // Start extracting logic
                {
                    if (Error_Handling.CheckOutOfRange() == false)
                    {
                        Navigation.BackupPath = Navigation.Pics[Navigation.FolderIndex];
                    }

                    await Error_Handling.ReloadAsync(true).ConfigureAwait(false);
                }
                return;
            }

            // Set files to Pics and get index
            Navigation.Pics = FileList(fileInfo);
            if (Navigation.Pics == null)
            {
                await Error_Handling.ReloadAsync(true).ConfigureAwait(false);
                return;
            }
#if DEBUG
            Trace.WriteLine("Getvalues completed ");
#endif
        });
    }
}