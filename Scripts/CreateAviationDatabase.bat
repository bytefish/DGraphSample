@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set JENA_HOME="G:\Apache_Jena\apache-jena-3.13.1"
set JENA_TDBLOADER2="%JENA_HOME%\bat\tdb2_tdbloader.bat"
set DATABASE_DIR="G:\Apache_Jena\data\aviation"
set DATASET="G:\aviation_2014.ttl"

pushd %JENA_HOME%

%JENA_TDBLOADER2% --loc=%DATABASE_DIR% %DATASET%