@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set DGRAPH_EXECUTABLE="G:\DGraph\v1.1.1\dgraph.exe"
set DGRAPH_ZERO_WAL_DIRECTORY="G:\DGraph\data\zw"

%DGRAPH_EXECUTABLE% zero --wal %DGRAPH_ZERO_WAL_DIRECTORY%

pause