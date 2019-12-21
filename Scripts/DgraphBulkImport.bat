@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set DGRAPH_EXECUTABLE="G:\DGraph\v1.0.17\dgraph.exe"
set FILENAME_RDF="G:\aviation_2014.rdf"
set FILENAME_SCHEMA="D:\github\DGraphSample\Scripts\res\schema.txt"

%DGRAPH_EXECUTABLE% bulk -f %FILENAME_RDF% -s %FILENAME_SCHEMA% --reduce_shards=1 --http localhost:8000 --zero=localhost:5080
