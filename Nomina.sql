CREATE DATABASE Nomina;
 -- DROP DATABASE Nomina;
USE Nomina;
CREATE TABLE Categorias
(
Id int not null primary key auto_increment,
Nombre varchar(50),
TotalEmpleados int default 0,
SueldoMaximo decimal(6,2)
);

INSERT INTO Categorias(Nombre,SueldoMaximo) VALUES('Logistica', 1000),('Ayudantes sin titulación', 2000),('Gerente',3000),('Almacenista',4000),('Ingenieros y Licenciados',7000);

CREATE TABLE Empleados
(
Id int not null primary key auto_increment,
Nombre varchar(100) not null,
Sueldo decimal(6,2),
Activo tinyint(1) default 1,
CategoriaId int not null,
constraint fk_Empleados_Categoria foreign key(CategoriaId) references Categorias(Id) on delete cascade on update cascade
);
INSERT INTO Empleados(Nombre,Sueldo,Activo,CategoriaId) VALUES('PEPE', 5500,1,2);
-- DROP TRIGGER AgregarSueldoMaximo_AE;
DELIMITER //
CREATE TRIGGER AgregarSueldoMaximo_AE BEFORE INSERT ON  Empleados FOR EACH ROW
BEGIN
	SELECT SueldoMaximo INTO @sueldomaximo FROM Categorias WHERE Id = NEW.CategoriaId;
    SELECT COUNT(*) INTO @existe FROM Categorias WHERE Id = NEW.CategoriaId;
    IF(@existe = 1) THEN
    BEGIN
		IF(NEW.Sueldo > @sueldomaximo) THEN
				SET NEW.Sueldo =  @sueldomaximo;
			END IF; 
		UPDATE Categorias SET TotalEmpleados = TotalEmpleados + 1 WHERE Id = NEW.CategoriaId;
    END;
    END IF;
END;
//
SELECT * FROM Empleados;
SELECT * FROM Categorias;
-- TRUNCATE TABLE Empleados;
-- TRUNCATE TABLE Categorias;

-- DROP TRIGGER   AL_Actualizar;

DELIMITER //
CREATE TRIGGER AL_Actualizar BEFORE UPDATE ON  Empleados FOR EACH ROW
BEGIN
	SELECT SueldoMaximo INTO @sueldomaxviejo FROM Categorias WHERE Id = OLD.CategoriaId;
	SELECT SueldoMaximo INTO @sueldomaxnuevo FROM Categorias WHERE Id = NEW.CategoriaId;
    IF(NEW.Activo = 1) THEN
    BEGIN
		IF(NEW.CategoriaId != OLD.CategoriaId) THEN
			IF(NEW.Sueldo > @sueldomaxnuevo)THEN
            BEGIN
				SET NEW.Sueldo = @sueldomaxnuevo;
            END;
            END IF;
			UPDATE Categorias SET TotalEmpleados = TotalEmpleados -1 WHERE Id = OLD.CategoriaId;
			UPDATE Categorias SET TotalEmpleados = TotalEmpleados +1 WHERE Id = NEW.CategoriaId;
		END IF;
        IF(OLD.Sueldo != NEW.Sueldo)THEN
        BEGIN
			IF(NEW.Sueldo > @sueldomaxviejo) THEN
			BEGIN
				SET NEW.Sueldo = @sueldomaxviejo;
            END;
            END IF;
        END;
        END IF;
    END;
    END IF;
END;
//
-- DELETE FROM EMPLEADOS;
SELECT * FROM EMPLEADOS;
UPDATE EMPLEADOS SET Sueldo = 5000 WHERE Id = 2;
SELECT * FROM CATEGORIAS;

-- SET FOREIGN_KEY_CHECKS = 0; 
-- TRUNCATE TABLE Categorias; 
-- SET FOREIGN_KEY_CHECKS = 1;