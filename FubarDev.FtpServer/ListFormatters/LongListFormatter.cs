﻿//-----------------------------------------------------------------------
// <copyright file="LongListFormatter.cs" company="Fubar Development Junker">
//     Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>
// <author>Mark Junker</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using FubarDev.FtpServer.FileSystem;

namespace FubarDev.FtpServer.ListFormatters
{
    public class LongListFormatter : IListFormatter
    {
        private static readonly CultureInfo _cultureUs = new CultureInfo("en-US");

        public IEnumerable<string> GetPrefix(IUnixDirectoryEntry directoryEntry)
        {
            var result = new List<string>
            {
                BuildLine(directoryEntry, null, "."),
            };
            if (!string.IsNullOrEmpty(directoryEntry.Name))
            {
                result.Add(BuildLine(directoryEntry, null, ".."));
            }
            return result;
        }

        public IEnumerable<string> GetSuffix(IUnixDirectoryEntry directoryEntry)
        {
            return new string[0];
        }

        public string Format(IUnixFileSystemEntry entry)
        {
            var fileEntry = entry as IUnixFileEntry;
            return BuildLine(entry, fileEntry, entry.Name);
        }

        private static string BuildLine(IUnixFileSystemEntry entry, IUnixFileEntry fileEntry, string name)
        {
            return string.Format(
                _cultureUs,
                "{0}{1}{2}{3} {4} {5} {6} {7:D13} {8:MMM dd HH:mm} {9}",
                fileEntry == null ? "d" : "-",
                BuildAccessMode(entry.Permissions.User),
                BuildAccessMode(entry.Permissions.Group),
                BuildAccessMode(entry.Permissions.Owner),
                entry.NumberOfLinks,
                entry.Owner,
                entry.Group,
                fileEntry?.Size ?? 0,
                entry.LastWriteTime,
                name);
        }

        private static string BuildAccessMode(IAccessMode accessMode)
        {
            return $"{(accessMode.Read ? "r" : "-")}{(accessMode.Write ? "w" : "-")}{(accessMode.Execute ? "x" : "-")}";
        }
    }
}