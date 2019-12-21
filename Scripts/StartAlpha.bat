@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set DGRAPH_EXECUTABLE="G:\DGraph\v1.1.1\dgraph.exe"
set DGRAPH_WAL_DIRECTORY="G:\DGraph\data\w"
set DGRAPH_POSTINGS_DIRECTORY="G:\DGraph\data\p"
set DGRAPH_LOGS_DIRECTORY="G:\DGraph\data\logs"

%DGRAPH_EXECUTABLE% alpha --lru_mb 4096 --wal %DGRAPH_WAL_DIRECTORY% --postings %DGRAPH_POSTINGS_DIRECTORY% --zero localhost:5080

pause