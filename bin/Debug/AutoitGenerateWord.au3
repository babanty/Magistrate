#include <Word.au3> ; либра для взаимодействия с вордом

#Region Глобальная инструкция скрипта
$PathToIniFile = @ScriptDir & "\PropertiesForAutoitScript.ini" ; Путь до ini файла с данными для генерации ворда

$PathToSampleFile = IniRead($PathToIniFile, "PropertiesForGenerate", "PathToSamples", "null"); Название шаблона ворда
If($PathToSampleFile = "null") Then appError("Ненайдены необходимые настройки в ini-файле")

$ValueKey = IniReadSection($PathToIniFile, "ValueForGenerate") ; Записывает все пары ключ-значения в массив

$oWord = _Word_Create() ; Создание сом объекта ворда
$oDoc = _Word_DocOpen($oWord,$PathToSampleFile , Default, Default, True) ; открытие шаблона файла ворда

For $i = 1 to UBound($ValueKey) - 1 ; Перебор всех ключей в секции с ключами-значениями для word

	if(StringLen($ValueKey[$i][1]) > 200) Then ; Если больше 200 символов,то надо делить на две вставки
		$numCycle = Ceiling ( (StringLen($ValueKey[$i][1])/200) ); Число циклов, каждые 200 символов +1 цикл
		Local $localStr = ""
		_Word_DocFindReplace($oDoc, $ValueKey[$i][0], "#[strDiv]") ; замена ключей на значения
		For $ii = 0 To $numCycle - 1 ; разбить на строчки
			if($ii = $numCycle - 1) Then ; если это последний цикл
				$localStr = StringMid ( $ValueKey[$i][1], 1 + 200 * $ii ) ; вырезаем строчку 200 символов
				_Word_DocFindReplace($oDoc, "#[strDiv]", $localStr) ; замена ключей на значения
			Else
				$localStr = StringMid ( $ValueKey[$i][1], 1 + 200 * $ii, 200 + 200 * $ii ) ; вырезаем строчку 200 символов
				$localStr = $localStr & "#[strDiv]" ; добавление заменяемой строчки
				_Word_DocFindReplace($oDoc, "#[strDiv]", $localStr) ; замена ключей на значения
			EndIf
		Next

	Else
		_Word_DocFindReplace($oDoc, $ValueKey[$i][0], $ValueKey[$i][1]) ; замена ключей на значения
	EndIf

;~ 	if(StringLen($ValueKey[$i][1]) > 200) Then ; Если больше 200 символов,то надо делить на две вставки
;~ 		if(StringLen($ValueKey[$i][1]) > 400) Then
;~ 			msg('Превышено максимальное количество символов для вставки в один ключ в 400 символов. Ключ: "' + $ValueKey[$i][0] + '" заменен не будет')
;~ 		EndIf
;~ 		Local $stringOne = StringLeft($ValueKey[$i][1], 200) & "#[1]" ; Разбиение строки на две, тут первая строка
;~ 		_Word_DocFindReplace($oDoc, $ValueKey[$i][0], $stringOne) ; замена ключей на значения
;~ 		Local $stringTwo = StringRight($ValueKey[$i][1], StringLen($ValueKey[$i][1]) - 200) ; Разбиение строки на две, тут вторая строка
;~ 		_Word_DocFindReplace($oDoc, "#[1]", $stringTwo) ; замена ключей на значения
;~ 	Else
;~ 		_Word_DocFindReplace($oDoc, $ValueKey[$i][0], $ValueKey[$i][1]) ; замена ключей на значения
;~ 	EndIf


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

