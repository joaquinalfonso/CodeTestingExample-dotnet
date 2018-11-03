CREATE TABLE [dbo].[Transcripciones]
(
	[Id] VARCHAR(30) NOT NULL PRIMARY KEY, 
    [FechaHoraRecepcion] DATETIME NOT NULL, 
    [LoginUsuario] VARCHAR(50) NOT NULL, 
    [NombreArchivo] VARCHAR(200) NOT NULL, 
    [Estado] VARCHAR(20) NOT NULL, 
    [FechaHoraTranscripcion] DATETIME NULL, 
    [MensajeError] VARCHAR(200) NOT NULL
)
