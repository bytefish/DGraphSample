@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: Executable & Temp Dir:
set DGRAPH_EXECUTABLE="G:\DGraph\v1.1.1\dgraph.exe"
set DGRAPH_BULK_TMP_DIRECTORY="G:\DGraph\tmp"
set DGRAPH_BULK_OUT_DIRECTORY="G:\DGraph\out"

:: Schema and Data
set FILENAME_RDF="D:\aviation_2014.rdf.gz"
set FILENAME_SCHEMA="D:\github\DGraphSample\Scripts\res\schema.txt"

%DGRAPH_EXECUTABLE% bulk -f %FILENAME_RDF% -s %FILENAME_SCHEMA% --replace_out --reduce_shards=1 --tmp %DGRAPH_BULK_TMP_DIRECTORY% --out %DGRAPH_BULK_OUT_DIRECTORY% --http localhost:8000 --zero=localhost:5080

pause