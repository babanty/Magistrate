#include <Word.au3> ; либра для взаимодействия с вордом

#Region Глобальная инструкция скрипта
$PathToIniFile = @ScriptDir & "\PropertiesForAutoitScript.ini" ; Путь до ini файла с данными для генерации ворда

$PathToSampleFile = IniRead($PathToIniFile, "PropertiesForGenerate", "PathToSamples", "null"); Название шаблона ворда
If($PathToSampleFile = "null") Then appError("Ненайдены необходимые настройки в ini-файле")


;Dim $ValueKey[100][2] ; []-номер в секции с ключами-значениями; [][0]- Key; [][1] - Value
$ValueKey = IniReadSection($PathToIniFile, "ValueForGenerate") ; Записывает все пары ключ-значения в массив

$oWord = _Word_Create() ; Создание сом объекта ворда
$oDoc = _Word_DocOpen($oWord,$PathToSampleFile , Default, Default, True) ; открытие шаблона файла ворда
Sleep(5000)
For $i = 1 to UBound($ValueKey) - 1 ; Перебор всех ключей в секции с ключами-значениями для word
	_Word_DocFindReplace($oDoc, $ValueKey[$i][0], $ValueKey[$i][1]) ; замена ключей на значения
Next
#EndRegion Глобальная инструкция скрипта

#Region Функции
Func msg($msg)
	MsgBox(0, "Бот генерирующий word файл", $msg)
EndFunc

Func appError($msg)
	msg($msg)
	Exit
EndFunc
#EndRegion Функции

