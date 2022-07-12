--Server name: DESKTOP-EQC5ME2\SQLEXPRESS
--DB Name:     DeliveryServiceDB


--DROP TABLE  dbo.OrderItems;
--DROP TABLE  dbo.ProductDefinitions;
--DROP TABLE  dbo.Orders;
--DROP TABLE  dbo.Products;
--DROP TABLE  dbo.Ingredients;
--DROP TABLE  dbo.Users;
--DROP TABLE  dbo.IngredientsTmp;
--DROP TABLE  dbo.ProductDefinitionsTmp;
--DROP TABLE  dbo.ProductsTmp;

DELETE FROM dbo.OrderItems
DELETE FROM dbo.ProductDefinitions
DELETE FROM dbo.Orders
DELETE FROM dbo.Products
DELETE FROM dbo.Ingredients
DELETE FROM dbo.Users

SET IDENTITY_INSERT dbo.Ingredients OFF;
SET IDENTITY_INSERT dbo.Products OFF;
SET IDENTITY_INSERT dbo.Orders OFF;

CREATE TABLE IngredientsTmp(Id int, Name varchar(35));
INSERT INTO dbo.IngredientsTmp VALUES (0, 'Beef');
INSERT INTO dbo.IngredientsTmp VALUES (1, 'Pork');
INSERT INTO dbo.IngredientsTmp VALUES (2,'Chicken');
INSERT INTO dbo.IngredientsTmp VALUES (3,'Ham');
INSERT INTO dbo.IngredientsTmp VALUES (4,'Bacon');
INSERT INTO dbo.IngredientsTmp VALUES (5,'Turkey');
INSERT INTO dbo.IngredientsTmp VALUES (6,'Tuna');
INSERT INTO dbo.IngredientsTmp VALUES (7,'Fries');
INSERT INTO dbo.IngredientsTmp VALUES (8,'Tomato');
INSERT INTO dbo.IngredientsTmp VALUES (9,'Peppers');
INSERT INTO dbo.IngredientsTmp VALUES (10,'Lettuce');
INSERT INTO dbo.IngredientsTmp VALUES (11,'Bread');
INSERT INTO dbo.IngredientsTmp VALUES (12,'Garlic Bread');
INSERT INTO dbo.IngredientsTmp VALUES (13,'Onions');
INSERT INTO dbo.IngredientsTmp VALUES (14,'Mayo');
INSERT INTO dbo.IngredientsTmp VALUES (15,'Ketchup');
INSERT INTO dbo.IngredientsTmp VALUES (16,'Sour Cream');
INSERT INTO dbo.IngredientsTmp VALUES (17,'Salt');
INSERT INTO dbo.IngredientsTmp VALUES (18,'Pepper');
INSERT INTO dbo.IngredientsTmp VALUES (19,'Cury');
INSERT INTO dbo.IngredientsTmp VALUES (20,'Soya Sauce');
INSERT INTO dbo.IngredientsTmp VALUES (21,'Olive Oil');


SET IDENTITY_INSERT dbo.Ingredients OFF;

SET IDENTITY_INSERT dbo.Ingredients ON;
INSERT INTO dbo.Ingredients ([Id], [Name]) 
SELECT [Id], [Name] FROM dbo.IngredientsTmp;
SET IDENTITY_INSERT dbo.Ingredients OFF;
-----------------------------------------------------------------------------------------------
CREATE TABLE ProductsTmp(Id int, Name varchar(50), Price float);
CREATE TABLE ProductDefinitionsTmp(ProductId int, IngredientId int);

INSERT INTO dbo.ProductsTmp VALUES (0, 'Black Forest Ham', 440);
INSERT INTO dbo.ProductDefinitionsTmp VALUES (0, (SELECT Id FROM Ingredients WHERE Name = 'Garlic Bread'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (0, (SELECT Id FROM Ingredients WHERE Name = 'Ham'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (0, (SELECT Id FROM Ingredients WHERE Name = 'Tomato'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (0, (SELECT Id FROM Ingredients WHERE Name = 'Mayo'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (0, (SELECT Id FROM Ingredients WHERE Name = 'Onions'));
   
INSERT INTO dbo.ProductsTmp VALUES (1, 'Mediterranean Wave', 580);
INSERT INTO dbo.ProductDefinitionsTmp VALUES (1, (SELECT Id FROM Ingredients WHERE Name = 'Bread'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (1, (SELECT Id FROM Ingredients WHERE Name = 'Tuna'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (1, (SELECT Id FROM Ingredients WHERE Name = 'Lettuce'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (1, (SELECT Id FROM Ingredients WHERE Name = 'Tomato'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (1, (SELECT Id FROM Ingredients WHERE Name = 'Onions'));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (1, (SELECT Id FROM Ingredients WHERE Name = 'Olive Oil' ));

INSERT INTO dbo.ProductsTmp VALUES (2, 'Balkan XXL Experience', 1250);
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Garlic Bread' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Beef' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Pork' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Chicken' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Ham' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Bacon' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Fries' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Tomato' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Peppers' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Onions' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Mayo' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Sour Cream' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Salt' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (2, (SELECT Id FROM Ingredients WHERE Name = 'Pepper' ));

INSERT INTO dbo.ProductsTmp VALUES (3, 'Cold Combo', 420);
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Bread' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Ham' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Turkey' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Lettuce' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Tomato' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Onions' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Sour Cream' ));
INSERT INTO dbo.ProductDefinitionsTmp VALUES (3, (SELECT Id FROM Ingredients WHERE Name = 'Olive Oil'  ));

SET IDENTITY_INSERT dbo.Ingredients OFF;
SET IDENTITY_INSERT dbo.Products OFF;

SET IDENTITY_INSERT dbo.Products ON;
INSERT INTO dbo.Products (Id, Name, Price) 
SELECT Id, Name, Price FROM dbo.ProductsTmp;
SET IDENTITY_INSERT dbo.Products OFF;

INSERT INTO dbo.ProductDefinitions (ProductId, IngredientId)
SELECT ProductId, IngredientId FROM dbo.ProductDefinitionsTmp;
-----------------------------------------------------------------------------------------------
INSERT INTO dbo.Users VALUES('ugljesa_dummy1_admin',    'dummy_mail_1@gmail.com',	'dae7ac9a262d8b6577823250295f42c2d57ecf46e009184dcc03af2337293d59',
							 'Ugljesa_dummy1',		    'Todorovic_dummy1',					'1999-11-17',						
							 'Bul. Despota stefana 7',  'a',
                             'dummy_img.png', 1);

INSERT INTO dbo.Users VALUES('ugljesa_dummy2_admin',    'dummy_mail_2@gmail.com',    'dae7ac9a262d8b6577823250295f42c2d57ecf46e009184dcc03af2337293d59',
							 'Ugljesa_dummy2',          'Todorovic_dummy2',                 '1999-11-18',
							 'Bul. Despota stefana 8',  'a',
							 '', 1);
      
INSERT INTO dbo.Users VALUES('ugljesa_dummy3_delivery', 'dummy_mail_3@gmail.com',	'dae7ac9a262d8b6577823250295f42c2d57ecf46e009184dcc03af2337293d59',
							 'Ugljesa_dummy3',			'Todorovic_dummy3',					'1999-11-19',
							 'Bul. Despota stefana 9',	'd',
							 'dummy_img.png',1);

INSERT INTO dbo.Users VALUES('ugljesa_dummy4_delivery', 'dummy_mail_4@gmail.com',	'dae7ac9a262d8b6577823250295f42c2d57ecf46e009184dcc03af2337293d59',
							 'Ugljesa_dummy4',			'Todorovic_dummy4',					'1999-11-20',
							 'Bul. Despota stefana 10',	'd',
							 '',1);
     
INSERT INTO dbo.Users VALUES('ugljesa_dummy5_consumer', 'dummy_mail_5@gmail.com',	'dae7ac9a262d8b6577823250295f42c2d57ecf46e009184dcc03af2337293d59',
							 'Ugljesa_dummy5',			'Todorovic_dummy5',					'1999-11-21',
							 'Bul. Despota stefana 12',	'c',
							 'dummy_img.png',1);

INSERT INTO dbo.Users VALUES('ugljesa_dummy6_consumer', 'dummy_mail_6@gmail.com',		'dae7ac9a262d8b6577823250295f42c2d57ecf46e009184dcc03af2337293d59',
							 'Ugljesa_dummy6',			'Todorovic_dummy6',					    '1999-11-22',
							 'Bul. Despota stefana 13',	'c',
							 '',1);
-----------------------------------------------------------------------------------------------
CREATE TABLE dbo.OrdersTmp(Id int, Consumer varchar(35), Deliveryman varchar(35), Address varchar(35), Comment varchar(50), Status varchar(1), TimeExpected varchar(20));
INSERT INTO dbo.OrdersTmp VALUES (0, 'ugljesa_dummy5_consumer', '',						    'Dummy Address1',  'Dummy Comment 1', 'a', '');
INSERT INTO dbo.OrdersTmp VALUES (2, 'ugljesa_dummy5_consumer', 'ugljesa_dummy3_delivery',  'Dummy Address3',  'Dummy Comment 3', 'c', '10/07/2022 08:45');
INSERT INTO dbo.OrdersTmp VALUES (3, 'ugljesa_dummy5_consumer', 'ugljesa_dummy4_delivery',  'Dummy Address4',  'Dummy Comment 4', 'c', '09/07/2022 09:49');
INSERT INTO dbo.OrdersTmp VALUES (4, 'ugljesa_dummy5_consumer', '',                         'Dummy Address5',  'Dummy Comment 5', 'a', '');
INSERT INTO dbo.OrdersTmp VALUES (5, 'ugljesa_dummy6_consumer', '',                         'Dummy Address6',  'Dummy Comment 6', 'a', '');
INSERT INTO dbo.OrdersTmp VALUES (7, 'ugljesa_dummy6_consumer', 'ugljesa_dummy3_delivery',  'Dummy Address8',  'Dummy Comment 8', 'c', '08/07/2022 11:22');
INSERT INTO dbo.OrdersTmp VALUES (8, 'ugljesa_dummy6_consumer', 'ugljesa_dummy4_delivery',  'Dummy Address9',  'Dummy Comment 9', 'c', '07/07/2022 22:35');
INSERT INTO dbo.OrdersTmp VALUES (9, 'ugljesa_dummy6_consumer', '',                         'Dummy Address10', 'Dummy Comment 10','a', '');

SET IDENTITY_INSERT dbo.Ingredients OFF;
SET IDENTITY_INSERT dbo.Products OFF;
SET IDENTITY_INSERT dbo.Orders OFF;

SET IDENTITY_INSERT dbo.Orders ON;
INSERT INTO dbo.Orders (Id, Consumer, Deliveryman, Address, Comment, Status, TimeExpected)
SELECT Id, Consumer, Deliveryman, Address, Comment, Status, TimeExpected FROM dbo.OrdersTmp;
SET IDENTITY_INSERT dbo.Orders OFF;
-----------------------------------------------------------------------------------------------
CREATE TABLE dbo.OrderItemsTmp(OrderId int, ProductId int, Quantity int);

INSERT INTO dbo.OrderItemsTmp VALUES (0, (SELECT Id FROM Products WHERE Name = 'Black Forest Ham'), 1);
INSERT INTO dbo.OrderItemsTmp VALUES (0, (SELECT Id FROM Products WHERE Name = 'Mediterranean Wave'), 1);
INSERT INTO dbo.OrderItemsTmp VALUES (0, (SELECT Id FROM Products WHERE Name = 'Cold Combo'), 2);

INSERT INTO dbo.OrderItemsTmp VALUES (2, (SELECT Id FROM Products WHERE Name = 'Cold Combo'), 3);
INSERT INTO dbo.OrderItemsTmp VALUES (2, (SELECT Id FROM Products WHERE Name = 'Mediterranean Wave'), 2);

INSERT INTO dbo.OrderItemsTmp VALUES (3, (SELECT Id FROM Products WHERE Name = 'Mediterranean Wave'), 1);

INSERT INTO dbo.OrderItemsTmp VALUES (4, (SELECT Id FROM Products WHERE Name = 'Balkan XXL Experience'), 3);

INSERT INTO dbo.OrderItemsTmp VALUES (5, (SELECT Id FROM Products WHERE Name = 'Black Forest Ham'), 2);
INSERT INTO dbo.OrderItemsTmp VALUES (5, (SELECT Id FROM Products WHERE Name = 'Mediterranean Wave'), 3);
INSERT INTO dbo.OrderItemsTmp VALUES (5, (SELECT Id FROM Products WHERE Name = 'Cold Combo'), 2);
INSERT INTO dbo.OrderItemsTmp VALUES (5, (SELECT Id FROM Products WHERE Name = 'Balkan XXL Experience'), 2);

INSERT INTO dbo.OrderItemsTmp VALUES (7, (SELECT Id FROM Products WHERE Name = 'Black Forest Ham'), 4);
INSERT INTO dbo.OrderItemsTmp VALUES (7, (SELECT Id FROM Products WHERE Name = 'Mediterranean Wave'), 3);

INSERT INTO dbo.OrderItemsTmp VALUES (8, (SELECT Id FROM Products WHERE Name = 'Black Forest Ham'), 1);
INSERT INTO dbo.OrderItemsTmp VALUES (8, (SELECT Id FROM Products WHERE Name = 'Cold Combo'), 1);

INSERT INTO dbo.OrderItemsTmp VALUES (9, (SELECT Id FROM Products WHERE Name = 'Cold Combo'), 4);

SET IDENTITY_INSERT dbo.Ingredients OFF;
SET IDENTITY_INSERT dbo.Products OFF;
SET IDENTITY_INSERT dbo.Orders OFF;
SET IDENTITY_INSERT dbo.OrderItems OFF;

SET IDENTITY_INSERT dbo.OrderItems ON;
INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity)
SELECT OrderId, ProductId, Quantity FROM dbo.OrderItemsTmp;
SET IDENTITY_INSERT dbo.OrderItems OFF;

-----------------------------------------------------------------------------------------------

DROP TABLE dbo.OrdersTmp
DROP TABLE ProductsTmp;
DROP TABLE ProductDefinitionsTmp;
DROP TABLE dbo.IngredientsTmp;
DROP TABLE dbo.OrderItemsTmp;
