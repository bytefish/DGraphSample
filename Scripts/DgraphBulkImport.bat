@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set FUSEKI_SERVER_DIRECTORY="G:\Apache_Jena\apache-jena-fuseki-3.13.1"
set AVIATION_SERVICE_CONFIGURATION="D:\github\ApacheJenaSample\Scripts\config\aviation_conf.ttl"

pushd %FUSEKI_SERVER_DIRECTORY%

java -Xmx1200M -jar "%FUSEKI_SERVER_DIRECTORY%\fuseki-server.jar" --conf=%AVIATION_SERVICE_CONFIGURATION%

pause