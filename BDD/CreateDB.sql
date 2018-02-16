-- MySQL Script generated by MySQL Workbench
-- 02/16/18 13:52:40
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema EJS_PROG
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `EJS_PROG` ;

-- -----------------------------------------------------
-- Schema EJS_PROG
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `EJS_PROG` DEFAULT CHARACTER SET utf8 ;
USE `EJS_PROG` ;

-- -----------------------------------------------------
-- Table `EJS_PROG`.`User`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EJS_PROG`.`User` ;

CREATE TABLE IF NOT EXISTS `EJS_PROG`.`User` (
  `idUser` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `LastName` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`idUser`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `EJS_PROG`.`Project`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EJS_PROG`.`Project` ;

CREATE TABLE IF NOT EXISTS `EJS_PROG`.`Project` (
  `idProject` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  `File` VARCHAR(45) NULL,
  `fkUser` INT NOT NULL,
  PRIMARY KEY (`idProject`, `fkUser`),
  INDEX `fk_Project_User_idx` (`fkUser` ASC),
  CONSTRAINT `fk_Project_User`
    FOREIGN KEY (`fkUser`)
    REFERENCES `EJS_PROG`.`User` (`idUser`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
