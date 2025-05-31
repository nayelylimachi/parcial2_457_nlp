CREATE DATABASE Parcial2Nlp;
GO

USE [master]
GO
CREATE LOGIN [usrparcial2Nlp] WITH PASSWORD = N'12345678',
	DEFAULT_DATABASE = [Parcial2Nlp],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON
GO
USE [Parcial2Nlp]
GO
CREATE USER [usrparcial2Nlp] FOR LOGIN [usrparcial2Nlp]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2Nlp]
GO

DROP TABLE Serie;

CREATE TABLE Serie (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    titulo VARCHAR(250) NOT NULL,
    sinopsis VARCHAR(5000) NOT NULL,
    director VARCHAR(100) NOT NULL,
    episodios INT,
    fechaEstreno DATE,
);
GO

ALTER TABLE Serie ADD estado SMALLINT NOT NULL DEFAULT 1;  -- -1ELIMINADO 0= INACTIVI  1= ACTIVO
GO

CREATE PROC paSerieListar @parametro VARCHAR(250)
AS
BEGIN
    SELECT *
    FROM Serie
    WHERE estado <> -1
      AND (titulo + director) LIKE '%' + @parametro + '%' + REPLACE(@parametro, ' ', '%') + '%'
    ORDER BY estado DESC, titulo ASC;
END
GO

-- Prueba de inserción
INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno)
VALUES ('Breaking Bad', 'Un profesor de química se convierte en fabricante de metanfetaminas.', 'Vince Gilligan', 62, '2008-01-20');

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno)
VALUES ('Stranger Things', 'Un grupo de niños descubre una serie de eventos paranormales en su pequeño pueblo.', 'Duffer Brothers', 34, '2016-07-15');

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno)
VALUES ('The Crown', 'Dramatización de la vida de la Reina Isabel II desde su juventud hasta la actualidad.', 'Peter Morgan', 60, '2016-11-04');

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno)
VALUES ('The Mandalorian', 'Un cazarrecompensas solitario navega por los confines de la galaxia, lejos de la autoridad de la Nueva República.', 'Jon Favreau', 24, '2019-11-12');
--('Game of Thrones', 'Nobles familias luchan por el control del Trono de Hierro en Westeros.', 'David Benioff y D.B. Weiss', 73, '2011-04-17'),

-- Ejemplo de actualización
UPDATE Serie SET episodios = 63 WHERE titulo = 'Breaking Bad';

-- Eliminación lógica
UPDATE Serie SET estado = -1 WHERE titulo = 'Breaking Bad';

-- Consultar registros
EXEC paSerieListar '';

SELECT * FROM Serie;