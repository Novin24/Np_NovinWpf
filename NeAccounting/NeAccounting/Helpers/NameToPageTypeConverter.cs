﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;

namespace NeAccounting.Helpers
{
    internal class NameToPageTypeConverter
    {
        private static readonly Type[] PageTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace?.StartsWith("NeAccounting.Views.Pages") ?? false)
            .ToArray();

        public static Type? Convert(string pageName)
        {
            pageName = pageName.Trim().ToLower() + "page";

            return PageTypes.FirstOrDefault(
                singlePageType => singlePageType.Name.ToLower() == pageName
            );
        }
    }
}
