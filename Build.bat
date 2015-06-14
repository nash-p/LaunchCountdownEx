rem params: 1 - solutiondir,2 - outdir

del "%KspPath%GameData\LaunchCountDownEx\*.dll" /s /q /f
del "%KspPath%GameData\LaunchCountDownEx\*.mdb" /s /q /f

call "c:\Program Files (x86)\Unity\Editor\Data\MonoBleedingEdge\bin\cli.bat" "c:\Program Files (x86)\Unity\Editor\Data\MonoBleedingEdge\lib\mono\4.0\pdb2mdb.exe" "e:\Mods\LaunchCountdownEx\bin\Debug\LaunchCountDownEx.dll"

xcopy "%2*dll" "%KspPath%GameData\LaunchCountDownEx\" /Y
xcopy "%2*mdb" "%KspPath%GameData\LaunchCountDownEx\" /Y
xcopy "%1\Sounds" "%KspPath%GameData\LaunchCountDownEx\Sounds" /E /I /Y
xcopy "%1\Images" "%KspPath%GameData\LaunchCountDownEx\Images" /E /I /Y
xcopy "%1\Icons" "%KspPath%GameData\LaunchCountDownEx\Icons" /E /I /Y

rem Unity 3d v.4.6.6f1 must be installed by default path

