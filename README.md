# Casos de uso

## CU1 - Recepción de un fichero para transcribir

Tipo request: [POST] 

Ruta: api/Transcripciones

Parámetros: 

* Login (Requerido) : se envia como una clave en el header del request con Key="Login" y Value="Usuario"
  
* Fichero MP3 (Requerido): se envia como una clave en el body, form-data de tipo file con Key="mp3" y Value=fichero.mp3
  
## CU2 - Consulta del estado de las transcripciones para un usuario

Tipo request: [GET]

Ruta: api/Transcripciones?desde={yyyy-MM-ddTHH:mm}&hasta={yyyy-MM-ddTHH:mm}

Parámetros:

* Login (Requerido) : se envia como una clave en el header del request con Key="Login" y Value="Usuario"

* Fecha desde (opcional): Fecha desde que se envia como parámetro de la url con el formato "desde=yyyy-MM-ddTHH:mm"

* Fecha hasta (opcional): Fecha hasta que se envia como parámetro de la url con el formato "hasta=yyyy-MM-ddTHH:mm"
 
~~~
Ejemplos: 
  
  [GET] api/Transcripciones
  
  [GET] api/Transcripciones?desde=2018-01-01T08:30
  
  [GET] api/Transcripciones?hasta=2018-12-31T23:59
  
  [GET] api/Transcripciones?desde=2018-01-01T08:30&hasta=2018-12-31T23:59
~~~
  
## CU3 - Envío del resultado de una transcripción

Tipo request: [GET]

Ruta: api/Transcripciones/{id}

Parámetros:

* Login (Requerido) : se envia como una clave en el header del request con Key="Login" y Value="Usuario"

* id (Requerido) : Id de la transcripción se envia como parámetro de la url

~~~
Ejemplos: 
  
  [GET] api/Transcripciones/1
  
  [GET] api/Transcripciones/33
~
