Set Schema LINQ2DB;

--Create Schema LINQ2DB;

DROP TABLE Doctor;
DROP TABLE Patient;
DROP TABLE Person;
DROP TABLE InheritanceParent;
DROP TABLE InheritanceChild;
DROP TABLE MasterTable;
DROP TABLE SlaveTable;
DROP TABLE Parent;
DROP TABLE Child;
DROP TABLE GrandChild;
DROP TABLE LinqDataTypes;
DROP TABLE TestIdentity;
DROP TABLE TestMerge1;
DROP TABLE TestMerge2;
DROP TABLE KeepIdentityTest;
DROP TABLE AllTypes;

CREATE TABLE InheritanceParent(
  InheritanceParentId INTEGER       PRIMARY KEY NOT NULL
, TypeDiscriminator   INTEGER                       
, Name                VARCHAR(50)                   
);



CREATE TABLE InheritanceChild(
  InheritanceChildId  INTEGER      PRIMARY KEY NOT NULL
, InheritanceParentId INTEGER                  NOT NULL
, TypeDiscriminator   INTEGER                      
, Name                VARCHAR(50)                  
);

CREATE TABLE Person( 
  PersonID   INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
  FirstName  VARCHAR(50) NOT NULL,
  LastName   VARCHAR(50) NOT NULL,
  MiddleName VARCHAR(50),
  Gender     CHAR(1)     NOT NULL
);

INSERT INTO Person (FirstName, LastName, Gender) VALUES ('John',   'Pupkin',    'M');
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Tester', 'Testerson', 'M');
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Jane',   'Doe',       'F');
INSERT INTO Person (FirstName, LastName, MiddleName, Gender) VALUES ('Jürgen', 'König', 'Ko', 'M');

-- Doctor Table Extension

CREATE TABLE Doctor(
  PersonID INTEGER     PRIMARY KEY NOT NULL,
  Taxonomy VARCHAR(50) NOT NULL,

  FOREIGN KEY FK_Doctor_Person (PersonID) REFERENCES Person
);


INSERT INTO Doctor (PersonID, Taxonomy) VALUES (1, 'Psychiatry');


CREATE TABLE MasterTable(
  ID1 INTEGER NOT NULL,
  ID2 INTEGER NOT NULL,
  PRIMARY KEY (ID1,ID2)
);

CREATE TABLE SlaveTable(
  ID1    INTEGER NOT NULL,
  "ID 2222222222222222222222  22" INTEGER NOT NULL,
  "ID 2222222222222222"           INTEGER NOT NULL,
  FOREIGN KEY FK_SlaveTable_MasterTable ("ID 2222222222222222222222  22", ID1)
  REFERENCES MasterTable
);

-- Patient Table Extension

CREATE TABLE Patient(
  PersonID  INTEGER      PRIMARY KEY NOT NULL,
  Diagnosis VARCHAR(256) NOT NULL,

  FOREIGN KEY FK_Patient_Person (PersonID) REFERENCES Person
);

INSERT INTO Patient (PersonID, Diagnosis) VALUES (2, 'Hallucination with Paranoid Bugs'' Delirium of Persecution');


CREATE TABLE Parent      (ParentID int, Value1 int);
CREATE TABLE Child       (ParentID int, ChildID int);
CREATE TABLE GrandChild  (ParentID int, ChildID int, GrandChildID int);



CREATE TABLE LinqDataTypes(
  ID             int,
  MoneyValue     decimal(10,4),
  DateTimeValue  timestamp,
  DateTimeValue2 timestamp,
  BoolValue      smallint,
  GuidValue      char(16) for bit DATA,
  BinaryValue    blob(5000)  ,
  SmallIntValue  smallint,
  IntValue       int         ,
  BigIntValue    bigint      ,
  StringValue    VARCHAR(50) 
);


CREATE TABLE TestIdentity (
  ID   INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL
);

CREATE TABLE TestMerge1(
  Id       INTEGER            PRIMARY KEY NOT NULL,
  Field1   INTEGER                            ,
  Field2   INTEGER                            ,
  Field3   INTEGER                            ,
  Field4   INTEGER                            ,
  Field5   INTEGER                            ,

  FieldInt64      BIGINT                      ,
  FieldBoolean    SMALLINT                    ,
  FieldString     VARCHAR(20)                 ,
--  FieldNString    NVARCHAR(20)                ,
  FieldChar       CHAR(1)                     ,
--  FieldNChar      NCHAR(1)                    ,
  FieldFloat      REAL                        ,
  FieldDouble     DOUBLE                      ,
  FieldDateTime   TIMESTAMP, --(3)                ,
  FieldBinary     VARCHAR(20)  FOR BIT DATA       ,
  FieldGuid       CHAR(16)     FOR BIT DATA       ,
  FieldDecimal    DECIMAL(24, 10)             ,
  FieldDate       DATE                        ,
  FieldTime       TIME                        ,
  FieldEnumString VARCHAR(20)                 ,
  FieldEnumNumber INT                         
);

CREATE TABLE TestMerge2(
  Id       INTEGER            PRIMARY KEY NOT NULL,
  Field1   INTEGER                            ,
  Field2   INTEGER                            ,
  Field3   INTEGER                            ,
  Field4   INTEGER                            ,
  Field5   INTEGER                            ,

  FieldInt64      BIGINT                      ,
  FieldBoolean    SMALLINT                    ,
  FieldString     VARCHAR(20)                 ,
--  FieldNString    NVARCHAR(20)                ,
  FieldChar       CHAR(1)                     ,
--  FieldNChar      NCHAR(1)                    ,
  FieldFloat      REAL                        ,
  FieldDouble     DOUBLE                      
,  FieldDateTime   TIMESTAMP--(3)                
,  FieldBinary     VARCHAR(20)  FOR BIT DATA       ,
  FieldGuid       CHAR(16)     FOR BIT DATA       ,
  FieldDecimal    DECIMAL(24, 10)             ,
  FieldDate       DATE                        ,
  FieldTime       TIME                        ,
  FieldEnumString VARCHAR(20)                 ,
  FieldEnumNumber INT                         
);

CREATE TABLE KeepIdentityTest (
  ID    INTEGER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY NOT NULL,
  Value INTEGER                                                  
);

CREATE TABLE AllTypes(
  ID INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,

  bigintDataType           bigint                ,
  intDataType              int                   ,
  smallintDataType         smallint              ,
  decimalDataType          decimal(30)           ,
--  decfloatDataType         decfloat              ,
  realDataType             real                  ,
  doubleDataType           double                ,

  charDataType             char(1)               ,
  char20DataType           char(20)              ,
  varcharDataType          varchar(20)           ,
  clobDataType             clob                  ,
  dbclobDataType           dbclob(100)           ,

  binaryDataType           char(5) for bit data,
  varbinaryDataType        varchar(5) for bit data,
  blobDataType             blob                  ,
  graphicDataType          graphic(10)           ,

  dateDataType             date                  ,
  timeDataType             time                  ,
  timestampDataType        timestamp             

--,  xmlDataType              xml                   
);

--INSERT INTO AllTypes (xmlDataType) VALUES (NULL);

INSERT INTO AllTypes(
  bigintDataType,
  intDataType,
  smallintDataType,
  decimalDataType,
--  decfloatDataType,
  realDataType,
  doubleDataType,

  charDataType,
  varcharDataType,
  clobDataType,
  dbclobDataType,

  binaryDataType,
  varbinaryDataType,
  blobDataType,
  graphicDataType,

  dateDataType,
  timeDataType,
  timestampDataType

--,  xmlDataType
) VALUES (
  1000000,
  7777777,
  100,
  9999999,
--  8888888,
  20.31,
  16.2,

  '1',
  '234',
  '55645',
  '6687',

  '123',
  '1234',
  Cast('234' as blob),
  '23',

  Cast('2012-12-12' as date),
  Cast('12:12:12' as time),
  Cast('2012-12-12 12:12:12.012' as timestamp)

--,  '<root><element strattr=strvalue intattr=12345/></root>'
);

CREATE OR REPLACE VIEW PersonView AS SELECT * FROM Person;

CREATE OR REPLACE Procedure Person_SelectByKey(in ID integer)
  RESULT SETS 1
  LANGUAGE SQL
  BEGIN
  DECLARE C1 CURSOR WITH RETURN TO CLIENT FOR
    SELECT * FROM Person WHERE PersonID = ID;

  OPEN C1;
END;


CREATE OR REPLACE Procedure AddIssue792Record()
  LANGUAGE SQL
  BEGIN
    INSERT INTO AllTypes(char20DataType) VALUES('issue792');
END;