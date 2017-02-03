SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

SET NAMES "utf8";
SET NAMES 'utf8' COLLATE 'utf8_general_ci';
SET CHARSET "utf8";
SET CHARACTER SET "utf8";

CREATE DATABASE IF NOT EXISTS `breakermind` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;

USE breakermind;

CREATE TABLE `account`(id INT NOT NULL AUTO_INCREMENT,time int, accountid int, balance float(10,2),equity float(10,2),margin float(10,2),freemargin float(10,2), currency varchar(20), leverage int, PRIMARY KEY(id));
CREATE TABLE IF NOT EXISTS `OpenSignal` (  `id` varchar(250) DEFAULT NULL,  `symbol` varchar(250) DEFAULT '0',  `volume` float DEFAULT '0',  `type` varchar(250) DEFAULT '0',  `opent` bigint(21) NOT NULL DEFAULT '0',  `openp` float(10,6) DEFAULT '0',  `sl` float(10,6) DEFAULT '0',  `tp` float(10,6) DEFAULT '0',  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,  `account` varchar(250) DEFAULT '0',  UNIQUE KEY `id` (`id`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE TABLE IF NOT EXISTS `CloseSignal` (  `id` varchar(250) DEFAULT NULL,  `closet` bigint(21) NOT NULL DEFAULT '0',  `closep` float(10,6) DEFAULT '0',  `profit` float(10,6) DEFAULT '0',  `pips` float(10,2) DEFAULT '0',  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,  `account` varchar(250) DEFAULT '0',  UNIQUE KEY `id` (`id`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE TABLE IF NOT EXISTS `GBPJPY` (  `time` bigint DEFAULT NULL,  `open` float(10,6) DEFAULT '0',  `close` float(10,6) DEFAULT '0',  `low` float(10,6) DEFAULT '0',  `high` float(10,6) DEFAULT '0',  `reg` float(10,6) DEFAULT '0',  `reghigh` float(10,6) DEFAULT '0',    `reglow` float(10,6) DEFAULT '0',  UNIQUE KEY `time` (`time`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE TABLE IF NOT EXISTS `EURUSD` (  `time` bigint DEFAULT NULL,  `open` float(10,6) DEFAULT '0',  `close` float(10,6) DEFAULT '0',  `low` float(10,6) DEFAULT '0',  `high` float(10,6) DEFAULT '0',  `reg` float(10,6) DEFAULT '0',  `reghigh` float(10,6) DEFAULT '0',    `reglow` float(10,6) DEFAULT '0',  UNIQUE KEY `time` (`time`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;

