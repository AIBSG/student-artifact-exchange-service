@echo off

set file=%~1
set directories=%*
shift
shift

for %%d in (%directories%) do (
	if exist "%%d\%file%" (
		echo Файл %file% найден в каталоге %%d
		fc "C:\%file%" "%%d\%file%" > null && (
			echo Файлы индентичны
		) || (
			echo Отличия файлов:
			fc "C:\%file%" "%%d\%file%"
		)
		goto :end
	)
)

echo Файл %file% не найден в указанных каталогах
:end
pause