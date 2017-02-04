
CREATE DATABASE breakermind;

USE breakermind;

CREATE TABLE account(id INT IDENTITY(1,1) PRIMARY KEY, time int, accountid int, balance decimal(10,2),equity decimal(10,2),margin decimal(10,2),freemargin decimal(10,2), currency varchar(20), leverage int);

CREATE TABLE OpenSignal (
  id varchar(250) NOT NULL UNIQUE,
  symbol varchar(250) DEFAULT '0',
  volume decimal DEFAULT '0',
  type varchar(250) DEFAULT '0',
  opent int NOT NULL DEFAULT '0',
  openp decimal(10,6) DEFAULT '0',
  sl decimal(10,6) DEFAULT '0',
  tp decimal(10,6) DEFAULT '0',
  time DATETIME DEFAULT CURRENT_TIMESTAMP,
  account varchar(250) DEFAULT '0'  
);

CREATE TABLE CloseSignal (
  id varchar(250) DEFAULT NULL,
  closet int NOT NULL DEFAULT '0',
  closep decimal(10,6) DEFAULT '0',
  profit decimal(10,6) DEFAULT '0',
  pips decimal(10,2) DEFAULT '0',
  time DATETIME DEFAULT CURRENT_TIMESTAMP,
  account varchar(250) DEFAULT '0'
);

CREATE TABLE GBPJPY (
  time int NOT NULL UNIQUE,
  popen decimal(10,6) DEFAULT '0',
  pclose decimal(10,6) DEFAULT '0',
  low decimal(10,6) DEFAULT '0',
  high decimal(10,6) DEFAULT '0',
  reg decimal(10,6) DEFAULT '0',
  reghigh decimal(10,6) DEFAULT '0',  
  reglow decimal(10,6) DEFAULT '0',
);
