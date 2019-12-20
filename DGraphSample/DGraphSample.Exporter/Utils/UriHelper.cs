// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DGraphSample.Exporter.Utils
{
    public static class UriHelper
    {
        public static Uri Create(Uri baseUri, string path, string fragment)
        {
            var builder = new UriBuilder(baseUri);

            builder.Path = path;
            builder.Fragment = fragment;

            return builder.Uri;
        }


        public static Uri SetFragment(Uri uri, string fragment)
        {
            var builder = new UriBuilder(uri);

            builder.Fragment = fragment;

            return builder.Uri;
        }
    }
}