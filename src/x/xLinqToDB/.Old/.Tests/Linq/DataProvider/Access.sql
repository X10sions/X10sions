DROP TABLE InheritanceParent;
DROP TABLE InheritanceChild;
DROP TABLE Person;
DROP TABLE Doctor;
DROP TABLE Patient;
DROP TABLE Parent;
DROP TABLE Child;
DROP TABLE GrandChild;
DROP TABLE LinqDataTypes;
DROP TABLE TestIdentity;
DROP TABLE AllTypes;
DROP TABLE TestMerge1;
DROP TABLE TestMerge2;
DROP TABLE RelationsTable;

DROP Procedure AddIssue792Record;
DROP Procedure ThisProcedureNotVisibleFromODBC;
DROP Procedure Person_SelectByKey;
DROP Procedure Person_SelectAll;
DROP Procedure Person_SelectByName;
DROP Procedure Person_SelectListByName;
DROP Procedure Person_Insert;
DROP Procedure Person_Update;
DROP Procedure Person_Delete;
DROP Procedure Patient_SelectAll;
DROP Procedure Patient_SelectByName;
DROP Procedure Scalar_DataReader;




CREATE TABLE InheritanceParent(
	InheritanceParentId Int      NOT NULL CONSTRAINT PK_InheritanceParent PRIMARY KEY,
	TypeDiscriminator   Int          NULL,
	Name                Text(50)     NULL
);

CREATE TABLE InheritanceChild(
	InheritanceChildId  Int      NOT NULL CONSTRAINT PK_InheritanceChild PRIMARY KEY,
	InheritanceParentId Int      NOT NULL,
	TypeDiscriminator   Int          NULL,
	Name                Text(50)     NULL
);

CREATE TABLE Person(
	PersonID   Int IDENTITY,
	FirstName  Text(50) NOT NULL,
	LastName   Text(50) NOT NULL,
	MiddleName Text(50),
	Gender     Text(1) NOT NULL,
	CONSTRAINT PK_Peson PRIMARY KEY (PersonID)
);

CREATE TABLE Doctor(
	PersonID Int NOT NULL,
	Taxonomy Text(50) NOT NULL,
	CONSTRAINT PK_Doctor PRIMARY KEY (PersonID)
);

CREATE TABLE Patient(
	PersonID  Int NOT NULL,
	Diagnosis Text(255) NOT NULL,
	CONSTRAINT PK_Patient PRIMARY KEY (PersonID)
);

ALTER TABLE Doctor	ADD CONSTRAINT PersonDoctor FOREIGN KEY (PersonID) REFERENCES Person ON UPDATE CASCADE ON DELETE CASCADE;
ALTER TABLE Patient	ADD CONSTRAINT PersonPatient FOREIGN KEY (PersonID) REFERENCES Person ON UPDATE CASCADE ON DELETE CASCADE;

INSERT INTO Person (FirstName, LastName, Gender) VALUES ('John',   'Pupkin',    'M');
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Tester', 'Testerson', 'M');
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Jane',   'Doe',       'F');
INSERT INTO Person (FirstName, LastName, MiddleName, Gender) VALUES ('Jürgen', 'König', 'Ko', 'M');

INSERT INTO Doctor (PersonID, Taxonomy)   VALUES (1, 'Psychiatry');
INSERT INTO Patient (PersonID, Diagnosis) VALUES (2, 'Hallucination with Paranoid Bugs'' Delirium of Persecution');

CREATE TABLE Parent     (ParentID int, Value1 int NULL);
CREATE TABLE Child      (ParentID int, ChildID int);
CREATE TABLE GrandChild (ParentID int, ChildID int, GrandChildID int);

CREATE Procedure Person_SelectByKey([@id] Long) AS	SELECT * FROM Person WHERE PersonID = [@id];

CREATE Procedure Person_SelectAll AS 	SELECT * FROM Person;

CREATE Procedure Person_SelectByName(
	[@firstName] Text(50),
	[@lastName]  Text(50))
AS
SELECT
	*
FROM
	Person
WHERE
	FirstName = [@firstName] AND LastName = [@lastName];
GO

CREATE Procedure Person_SelectListByName(
	[@firstName] Text(50),
	[@lastName]  Text(50))
AS
SELECT
	*
FROM
	Person
WHERE
	FirstName like [@firstName] AND LastName like [@lastName];
GO

CREATE Procedure Person_Insert(
	[@FirstName]  Text(50),
	[@MiddleName] Text(50),
	[@LastName]   Text(50),
	[@Gender]     Text(1))
AS
INSERT INTO Person
	(FirstName, MiddleName, LastName, Gender)
VALUES
	([@FirstName], [@MiddleName], [@LastName], [@Gender]);
GO

CREATE Procedure Person_Update(
	[@id]         Long,
	[@FirstName]  Text(50),
	[@MiddleName] Text(50),
	[@LastName]   Text(50),
	[@Gender]     Text(1))
AS
UPDATE
	Person
SET
	LastName   = [@LastName],
	FirstName  = [@FirstName],
	MiddleName = [@MiddleName],
	Gender     = [@Gender]
WHERE
	PersonID = [@id];
GO

CREATE Procedure Person_Delete(
	[@PersonID] Long)
AS
DELETE FROM Person WHERE PersonID = [@PersonID];
GO

CREATE Procedure Patient_SelectAll
AS
SELECT
	Person.*, Patient.Diagnosis
FROM
	Patient, Person
WHERE
	Patient.PersonID = Person.PersonID;
GO

CREATE Procedure Patient_SelectByName(
	[@firstName] Text(50),
	[@lastName]  Text(50))
AS
SELECT
	Person.*, Patient.Diagnosis
FROM
	Patient, Person
WHERE
	Patient.PersonID = Person.PersonID
	AND FirstName = [@firstName] AND LastName = [@lastName];
GO

CREATE Procedure Scalar_DataReader
AS
	SELECT 12345 AS intField, '54321' AS stringField;
GO





CREATE TABLE LinqDataTypes(
	ID             int,
	MoneyValue     decimal(10,4),
	DateTimeValue  datetime,
	DateTimeValue2 datetime,
	BoolValue      bit,
	GuidValue      uniqueidentifier,
	BinaryValue    OleObject NULL,
	SmallIntValue  smallint,
	IntValue       int       NULL,
	BigIntValue    long      NULL,
	StringValue    Text(50)  NULL
);

CREATE TABLE TestIdentity(
	ID Int IDENTITY,
	CONSTRAINT PK_TestIdentity PRIMARY KEY (ID)
);


CREATE TABLE AllTypes(
	ID                       counter      NOT NULL,
	bitDataType              bit              NULL,
	smallintDataType         smallint         NULL,
	decimalDataType          decimal          NULL,
	intDataType              int              NULL,
	tinyintDataType          tinyint          NULL,
	moneyDataType            money            NULL,
	floatDataType            float            NULL,
	realDataType             real             NULL,

	datetimeDataType         datetime         NULL,

	charDataType             char(1)          NULL,
	char20DataType           char(20)         NULL,
	varcharDataType          varchar(20)      NULL,
	textDataType             text             NULL,
	ncharDataType            nchar(20)        NULL,
	nvarcharDataType         nvarchar(20)     NULL,
	ntextDataType            ntext            NULL,

	binaryDataType           binary(10)       NULL,
	varbinaryDataType        varbinary        NULL,
	imageDataType            image            NULL,
	oleObjectDataType        oleobject        NULL,

	uniqueidentifierDataType uniqueidentifier NULL
);

INSERT INTO AllTypes (binaryDataType) VALUES (NULL);


CREATE TABLE TestMerge1(
	Id       Int      NOT NULL CONSTRAINT PK_TestMerge1 PRIMARY KEY,
	Field1   Int          NULL,
	Field2   Int          NULL,
	Field3   Int          NULL,
	Field4   Int          NULL,
	Field5   Int          NULL,

	FieldBoolean    BIT               NULL,
	FieldString     VARCHAR(20)       NULL,
	FieldNString    NVARCHAR(20)      NULL,
	FieldChar       CHAR(1)           NULL,
	FieldNChar      NCHAR(1)          NULL,
	FieldFloat      REAL              NULL,
	FieldDouble     FLOAT             NULL,
	FieldDateTime   DATETIME          NULL,
	FieldBinary     VARBINARY(20)     NULL,
	FieldGuid       UNIQUEIDENTIFIER  NULL,
	FieldDecimal    DECIMAL(24, 10)   NULL,
	FieldDate       DATE              NULL,
	FieldTime       TIME              NULL,
	FieldEnumString VARCHAR(20)       NULL,
	FieldEnumNumber INT               NULL
);

CREATE TABLE TestMerge2(
	Id       Int      NOT NULL CONSTRAINT PK_TestMerge2 PRIMARY KEY,
	Field1   Int          NULL,
	Field2   Int          NULL,
	Field3   Int          NULL,
	Field4   Int          NULL,
	Field5   Int          NULL,

	FieldBoolean    BIT               NULL,
	FieldString     VARCHAR(20)       NULL,
	FieldNString    NVARCHAR(20)      NULL,
	FieldChar       CHAR(1)           NULL,
	FieldNChar      NCHAR(1)          NULL,
	FieldFloat      REAL              NULL,
	FieldDouble     FLOAT             NULL,
	FieldDateTime   DATETIME          NULL,
	FieldBinary     VARBINARY(20)     NULL,
	FieldGuid       UNIQUEIDENTIFIER  NULL,
	FieldDecimal    DECIMAL(24, 10)   NULL,
	FieldDate       DATE              NULL,
	FieldTime       TIME              NULL,
	FieldEnumString VARCHAR(20)       NULL,
	FieldEnumNumber INT               NULL
);

CREATE Procedure AddIssue792Record(@id INT) AS 	INSERT INTO AllTypes(char20DataType) VALUES('issue792');

CREATE Procedure ThisProcedureNotVisibleFromODBC AS 	INSERT INTO AllTypes(char20DataType) VALUES('issue792');

CREATE TABLE RelationsTable(
	ID1		INT NOT NULL,
	ID2		INT NOT NULL,
	Int1	INT NOT NULL,
	Int2	INT NOT NULL,
	IntN1	INT NULL,
	IntN2	INT NULL,
	FK		INT NOT NULL,
	FKN		INT NULL
);

CREATE INDEX PK_RelationsTable ON RelationsTable(ID1, ID2) WITH PRIMARY;
CREATE INDEX IX_Index ON RelationsTable(Int1, IntN1);
CREATE UNIQUE INDEX UX_Index1 ON RelationsTable(Int1);
CREATE UNIQUE INDEX UX_Index2 ON RelationsTable(IntN1);
ALTER TABLE RelationsTable ADD CONSTRAINT FK_Nullable FOREIGN KEY (IntN1, IntN2) REFERENCES RelationsTable(ID1, ID2);
ALTER TABLE RelationsTable ADD CONSTRAINT FK_NotNullable FOREIGN KEY (Int1, Int2) REFERENCES RelationsTable(ID1, ID2);