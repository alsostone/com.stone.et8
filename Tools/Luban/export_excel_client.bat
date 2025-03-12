set WORKSPACE=..\..

set LUBAN_DLL=%WORKSPACE%\Tools\Luban\LubanRelease\Luban.dll
set CONF_ROOT=%WORKSPACE%\Unity\Assets\Config\Excel

echo ======================= export excel client start ==========================
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t client ^
    -c cs-bin ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\__luban__.conf ^
    -x outputCodeDir=%WORKSPACE%\Unity\Assets\Scripts\Model\Generate\Client\Config ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\c ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\c ^
    -x lineEnding=LF ^
    
echo ======================= export excel client done ==========================

if %ERRORLEVEL% NEQ 0 exit
