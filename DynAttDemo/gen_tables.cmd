@echo off
set root=%userprofile%\.nuget\packages\sqexpress

for /F "tokens=*" %%a in ('dir "%root%" /b /a:d /o:n') do set "lib=%root%\%%a"

set lib=%lib%\tools\codegen\SqExpress.CodeGenUtil.dll

dotnet "%lib%" gentables mssql "Data Source=(local);Initial Catalog=DynAttDemo;Integrated security=SSPI" --table-class-prefix "Tbl" -o ".\Tables" -n "DynAttDemo.Tables"