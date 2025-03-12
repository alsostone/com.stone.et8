#!/bin/bash

WORKSPACE=../..

LUBAN_DLL=$WORKSPACE/Tools/Luban/LubanRelease/Luban.dll
CONF_ROOT=$WORKSPACE/Unity/Assets/Config/Excel

echo ======================= export excel client_server start ==========================
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -c cs-bin \
    -d bin \
    -d json \
    --conf $CONF_ROOT/__luban__.conf \
    -x outputCodeDir=$WORKSPACE/Unity/Assets/Scripts/Model/Generate/ClientServer/Config \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/cs \
    -x json.outputDataDir=$WORKSPACE/Config/Json/cs \
    -x lineEnding=LF 
    
echo ======================= export excel client_server done ==========================

if [ $? -ne 0 ]; then
    exit 1
fi