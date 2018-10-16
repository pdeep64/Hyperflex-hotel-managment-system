/*
SQLyog Ultimate v9.51 
MySQL - 5.7.17-log : Database - hyperfle_hms
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`hyperfle_hms` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `hyperfle_hms`;

/*Table structure for table `account` */

DROP TABLE IF EXISTS `account`;

CREATE TABLE `account` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `income_type` enum('INCOME','EXPENCES') DEFAULT NULL,
  `payment_type` varchar(25) DEFAULT NULL,
  `payment` double(30,2) DEFAULT '0.00',
  `added_date` date DEFAULT NULL,
  `added_time` time DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `card_details` varchar(255) DEFAULT 'N/A',
  `cheque_no` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

/*Data for the table `account` */

insert  into `account`(`id`,`income_type`,`payment_type`,`payment`,`added_date`,`added_time`,`note`,`card_details`,`cheque_no`) values (1,'EXPENCES','CASH',6200.00,'2018-08-12','20:41:00','PAYMENT FOR GRN: GR-00003','N/A',NULL),(2,'INCOME','CASH,',1200.00,'2018-08-12','23:19:55','AMOUNT RECEIVED FOR: RS-0000002','CARD NO: 0 OWNER NAME: ',NULL),(3,'INCOME','CASH,',300.00,'2018-08-14','18:25:23','AMOUNT RECEIVED FOR: RS-0000005','CARD NO: 0 OWNER NAME: ',NULL),(4,'INCOME','CASH,CHEQUE,',1200.00,'2018-08-14','18:26:55','AMOUNT RECEIVED FOR: RS-0000006','CARD NO:  OWNER NAME: ',NULL),(5,'INCOME','CASH,',480.00,'2018-08-14','18:27:45','AMOUNT RECEIVED FOR: RS-0000003','CARD NO:  OWNER NAME: ',NULL),(6,'INCOME','CASH,',654.00,'2018-08-14','20:48:28','AMOUNT RECEIVED FOR: RS-0000007','CARD NO: 0 OWNER NAME: ',NULL),(7,'INCOME','CASH,',410.00,'2018-08-14','20:56:08','AMOUNT RECEIVED FOR: RS-0000009','CARD NO: 0 OWNER NAME: ',NULL),(8,'INCOME','CASH,',383.00,'2018-08-14','21:42:04','AMOUNT RECEIVED FOR: RS-0000014','CARD NO: 0 OWNER NAME: ',NULL),(9,'INCOME','CASH,',400.00,'2018-08-14','22:02:59','AMOUNT RECEIVED FOR: RS-0000015','CARD NO: 0 OWNER NAME: ',NULL),(10,'INCOME','CASH,',1109.00,'2018-08-18','12:48:22','AMOUNT RECEIVED FOR: RS-0000017','CARD NO: 0 OWNER NAME: ',NULL),(11,'EXPENCES','CASH',2.00,'2018-08-21','20:06:46','PAYMENT FOR GRN: GR-00005','N/A',NULL),(12,'INCOME','CASH,',750.00,'2018-08-27','12:04:28','AMOUNT RECEIVED FOR: RS-0000020','CARD NO: 0 OWNER NAME: ',NULL),(13,'INCOME','CASH,',350.00,'2018-09-25','10:43:45','AMOUNT RECEIVED FOR: RS-0000023','CARD NO: 0 OWNER NAME: ',NULL);

/*Table structure for table `additional_service` */

DROP TABLE IF EXISTS `additional_service`;

CREATE TABLE `additional_service` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `reservation_id` varchar(15) DEFAULT NULL,
  `additional_serivice_id` int(11) DEFAULT NULL,
  `additional_serivice_qty` double(30,3) DEFAULT NULL,
  `additional_serivice_price` double(30,2) DEFAULT NULL COMMENT 'TOTAL PRICE (additional_serivice_qty*additional_serivice_price)',
  `room_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `additional_serivice_id` (`additional_serivice_id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

/*Data for the table `additional_service` */

insert  into `additional_service`(`id`,`reservation_id`,`additional_serivice_id`,`additional_serivice_qty`,`additional_serivice_price`,`room_id`) values (1,'RS-0000002',1,1.000,62.52,6),(2,'RS-0000003',1,1.000,63.02,3),(3,'RS-0000004',1,1.000,50.00,1),(4,'RS-0000006',1,1.000,63.02,3),(5,'RS-0000007',1,1.000,63.02,6),(6,'RS-0000009',1,1.000,63.02,1),(7,'RS-0000011',1,1.000,63.02,2),(8,'RS-0000015',1,1.000,63.02,4),(9,'RS-0000017',1,1.000,63.02,3),(10,'RS-0000016',2,1.000,12.60,2),(11,'RS-0000019',2,1.000,12.60,6),(12,'RS-0000020',1,1.000,63.02,2),(13,'RS-0000020',2,1.000,12.60,2);

/*Table structure for table `additional_service_list` */

DROP TABLE IF EXISTS `additional_service_list`;

CREATE TABLE `additional_service_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `service_name` varchar(50) NOT NULL,
  `service_price` double(30,2) NOT NULL DEFAULT '0.00',
  `description` text,
  `service_status` enum('1','0') NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `service_name` (`service_name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `additional_service_list` */

insert  into `additional_service_list`(`id`,`service_name`,`service_price`,`description`,`service_status`) values (1,'EXTRA BEAD',50.00,'N/A','1'),(2,'EXTRA TABLE',10.00,'N/A','1');

/*Table structure for table `agent` */

DROP TABLE IF EXISTS `agent`;

CREATE TABLE `agent` (
  `agent_id` int(11) NOT NULL AUTO_INCREMENT,
  `agent_name` varchar(25) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `contact_no` varchar(25) DEFAULT NULL,
  `company_name` varchar(60) DEFAULT NULL,
  `company_contact_no` varchar(15) DEFAULT NULL,
  `total_guest_count` int(10) DEFAULT '0',
  `commison_rate` double(10,2) DEFAULT '0.00',
  `description` text,
  `total_paid` double(30,2) NOT NULL DEFAULT '0.00',
  `total_due` double(30,2) NOT NULL DEFAULT '0.00',
  `added_by` int(11) DEFAULT NULL,
  `added_date` date DEFAULT NULL,
  `added_time` time DEFAULT NULL,
  `agent_status` enum('0','1') DEFAULT NULL,
  PRIMARY KEY (`agent_id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Data for the table `agent` */

insert  into `agent`(`agent_id`,`agent_name`,`address`,`contact_no`,`company_name`,`company_contact_no`,`total_guest_count`,`commison_rate`,`description`,`total_paid`,`total_due`,`added_by`,`added_date`,`added_time`,`agent_status`) values (1,'A.M. GAYAN THARAKA','-','1684684','-','546846',NULL,10.00,'DNWEJKNDJEK',0.00,0.00,1,'2018-08-12','20:26:11','1'),(2,'MR.PEREPA',NULL,NULL,NULL,NULL,NULL,NULL,NULL,0.00,0.00,NULL,NULL,NULL,NULL),(3,'MRS. KANCHANA',NULL,NULL,NULL,NULL,NULL,NULL,NULL,0.00,0.00,NULL,NULL,NULL,NULL),(4,'MR.NUWAN',NULL,NULL,NULL,NULL,NULL,NULL,NULL,0.00,0.00,NULL,NULL,NULL,NULL);

/*Table structure for table `agent_acc` */

DROP TABLE IF EXISTS `agent_acc`;

CREATE TABLE `agent_acc` (
  `account_no` int(10) NOT NULL AUTO_INCREMENT,
  `reservation_no` varchar(15) NOT NULL,
  `agent_id` int(15) DEFAULT NULL,
  `commition` double(20,2) DEFAULT '0.00',
  `paid` double(20,2) DEFAULT '0.00',
  `due` double(20,2) DEFAULT '0.00',
  PRIMARY KEY (`account_no`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

/*Data for the table `agent_acc` */

insert  into `agent_acc`(`account_no`,`reservation_no`,`agent_id`,`commition`,`paid`,`due`) values (8,'RS-0000014',1,38.32,0.00,0.00),(9,'RS-0000015',0,0.00,0.00,0.00);

/*Table structure for table `bank` */

DROP TABLE IF EXISTS `bank`;

CREATE TABLE `bank` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `bank_name` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

/*Data for the table `bank` */

insert  into `bank`(`id`,`bank_name`) values (1,'COM'),(2,'BOC'),(3,'HNB'),(4,'NDB'),(5,'PEOPLES'),(6,'NTB'),(7,'SAMPATH'),(8,'AMANA'),(9,'RBD');

/*Table structure for table `category` */

DROP TABLE IF EXISTS `category`;

CREATE TABLE `category` (
  `categry_id` int(11) NOT NULL AUTO_INCREMENT,
  `caegory_name` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`categry_id`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

/*Data for the table `category` */

insert  into `category`(`categry_id`,`caegory_name`) values (1,'NON-VEG FOOD'),(2,'VEG FOOD'),(3,'DESSERT'),(4,'BEVERAGE'),(5,'FAST FOOD'),(6,'INGRADIENTS');

/*Table structure for table `company` */

DROP TABLE IF EXISTS `company`;

CREATE TABLE `company` (
  `cmp_id` varchar(10) NOT NULL,
  `cmp_name` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL,
  `contact_no_01` varchar(15) DEFAULT NULL,
  `contact_no_02` varchar(15) DEFAULT NULL,
  `mac_id` varchar(200) DEFAULT NULL,
  `pro_id` varchar(300) DEFAULT NULL,
  `reg` varchar(10) DEFAULT NULL,
  `business_reg_no` varchar(255) DEFAULT NULL,
  `website` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`cmp_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `company` */

insert  into `company`(`cmp_id`,`cmp_name`,`address`,`contact_no_01`,`contact_no_02`,`mac_id`,`pro_id`,`reg`,`business_reg_no`,`website`,`email`) values ('CM-01','CASENDRTA VILLA','SRI DALADA VIDIYA,KANDY','081-2222231','081-2222231',NULL,NULL,NULL,NULL,'CASSENDRA.LK','OWNER.CASSENDRA.LK');

/*Table structure for table `country` */

DROP TABLE IF EXISTS `country`;

CREATE TABLE `country` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `country_name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=200 DEFAULT CHARSET=latin1;

/*Data for the table `country` */

insert  into `country`(`id`,`country_name`) values (1,'Albania'),(2,'Albania'),(3,'Algeria'),(4,'Andorra'),(5,'Angola'),(6,'Anguilla'),(7,'Antigua & Barbuda'),(8,'Argentina'),(9,'Armenia'),(10,'Australia'),(11,'Austria'),(12,'Azerbaijan'),(13,'Bahamas'),(14,'Bahrain'),(15,'Bangladesh'),(16,'Barbados'),(17,'Belarus'),(18,'Belgium'),(19,'Belize'),(20,'Benin'),(21,'Bermuda'),(22,'Bhutan'),(23,'Bolivia'),(24,'Bosnia & Herzegovina'),(25,'Botswana'),(26,'Brazil'),(27,'Brunei Darussalam'),(28,'Bulgaria'),(29,'Burkina Faso'),(30,'Myanmar/Burma'),(31,'Burundi'),(32,'Cambodia'),(33,'Cameroon'),(34,'Canada'),(35,'Cape Verde'),(36,'Cayman Islands'),(37,'Central African Republic'),(38,'Chad'),(39,'Chile'),(40,'China'),(41,'Colombia'),(42,'Comoros'),(43,'Congo'),(44,'Costa Rica'),(45,'Croatia'),(46,'Cuba'),(47,'Cyprus'),(48,'Czech Republic'),(49,'Democratic Republic of the Congo'),(50,'Denmark'),(51,'Djibouti'),(52,'Dominica'),(53,'Dominican Republic'),(54,'Ecuador'),(55,'Egypt'),(56,'El Salvador'),(57,'Equatorial Guinea'),(58,'Eritrea'),(59,'Estonia'),(60,'Ethiopia'),(61,'Fiji'),(62,'Finland'),(63,'France'),(64,'French Guiana'),(65,'Gabon'),(66,'Gambia'),(67,'Georgia'),(68,'Germany'),(69,'Ghana'),(70,'Great Britain'),(71,'Greece'),(72,'Grenada'),(73,'Guadeloupe'),(74,'Guatemala'),(75,'Guinea'),(76,'Guinea-Bissau'),(77,'Guyana'),(78,'Haiti'),(79,'Honduras'),(80,'Hungary'),(81,'Iceland'),(82,'India'),(83,'Indonesia'),(84,'Iran'),(85,'Iraq'),(86,'Israel and the Occupied Territories'),(87,'Italy'),(88,'Ivory Coast (Cote d\'Ivoire)'),(89,'Jamaica'),(90,'Japan'),(91,'Jordan'),(92,'Kazakhstan'),(93,'Kenya'),(94,'Kosovo'),(95,'Kuwait'),(96,'Kyrgyz Republic (Kyrgyzstan)'),(97,'Laos'),(98,'Latvia'),(99,'Lebanon'),(100,'Lesotho'),(101,'Liberia'),(102,'Libya'),(103,'Liechtenstein'),(104,'Lithuania'),(105,'Luxembourg'),(106,'Republic of Macedonia'),(107,'Madagascar'),(108,'Malawi'),(109,'Malaysia'),(110,'Maldives'),(111,'Mali'),(112,'Malta'),(113,'Martinique'),(114,'Mauritania'),(115,'Mauritius'),(116,'Mayotte'),(117,'Mexico'),(118,'Moldova, Republic of'),(119,'Monaco'),(120,'Mongolia'),(121,'Montenegro'),(122,'Montserrat'),(123,'Morocco'),(124,'Mozambique'),(125,'Namibia'),(126,'Nepal'),(127,'Netherlands'),(128,'New Zealand'),(129,'Nicaragua'),(130,'Niger'),(131,'Nigeria'),(132,'Korea, Democratic Republic of (North Korea)'),(133,'Norway'),(134,'Oman'),(135,'Pacific Islands'),(136,'Pakistan'),(137,'Panama'),(138,'Papua New Guinea'),(139,'Paraguay'),(140,'Peru'),(141,'Philippines'),(142,'Poland'),(143,'Portugal'),(144,'Puerto Rico'),(145,'Qatar'),(146,'Reunion'),(147,'Romania'),(148,'Russian Federation'),(149,'Rwanda'),(150,'Saint Kitts and Nevis'),(151,'Saint Lucia'),(152,'Saint Vincent\'s & Grenadines'),(153,'Samoa'),(154,'Sao Tome and Principe'),(155,'Saudi Arabia'),(156,'Senegal'),(157,'Serbia'),(158,'Seychelles'),(159,'Sierra Leone'),(160,'Singapore'),(161,'Slovak Republic (Slovakia)'),(162,'Slovenia'),(163,'Solomon Islands'),(164,'Somalia'),(165,'South Africa'),(166,'Korea, Republic of (South Korea)'),(167,'South Sudan'),(168,'Spain'),(169,'Sri Lanka'),(170,'Sudan'),(171,'Suriname'),(172,'Swaziland'),(173,'Sweden'),(174,'Switzerland'),(175,'Syria'),(176,'Tajikistan'),(177,'Tanzania'),(178,'Thailand'),(179,'Timor Leste'),(180,'Togo'),(181,'Trinidad & Tobago'),(182,'Tunisia'),(183,'Turkey'),(184,'Turkmenistan'),(185,'Turks & Caicos Islands'),(186,'Uganda'),(187,'Ukraine'),(188,'United Arab Emirates'),(189,'United States of America (USA)'),(190,'Uruguay'),(191,'Uzbekistan'),(192,'Venezuela'),(193,'Vietnam'),(194,'Virgin Islands (UK)'),(195,'Virgin Islands (US)'),(196,'Yemen'),(197,'Zambia'),(198,'Zimbabwe'),(199,'Other');

/*Table structure for table `daily_menu` */

DROP TABLE IF EXISTS `daily_menu`;

CREATE TABLE `daily_menu` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `meal_type` enum('BREAKFAST','LUNCH','DINNER') NOT NULL,
  `item_stock_id` int(10) NOT NULL,
  `menu_date` date NOT NULL,
  `added_by` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `daily_menu` */

/*Table structure for table `floor` */

DROP TABLE IF EXISTS `floor`;

CREATE TABLE `floor` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `floor_no` varchar(60) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `floor_no` (`floor_no`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Data for the table `floor` */

insert  into `floor`(`id`,`floor_no`) values (1,'1ST'),(2,'2ND'),(3,'3RD');

/*Table structure for table `grn` */

DROP TABLE IF EXISTS `grn`;

CREATE TABLE `grn` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `grn_no` varchar(10) NOT NULL,
  `payment_type` varchar(60) NOT NULL,
  `supplier_id` int(10) DEFAULT NULL,
  `grn_total` double(30,2) DEFAULT '0.00',
  `added_by` int(11) DEFAULT NULL,
  `added_date` date DEFAULT NULL,
  `added_time` time DEFAULT NULL,
  PRIMARY KEY (`id`,`grn_no`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

/*Data for the table `grn` */

insert  into `grn`(`id`,`grn_no`,`payment_type`,`supplier_id`,`grn_total`,`added_by`,`added_date`,`added_time`) values (1,'GR-00001','CASH',4,76200.00,1,'2018-08-12','20:41:00'),(2,'GR-00002','CASH',4,76200.00,1,'2018-08-12','20:41:00'),(3,'GR-00003','CASH',4,76200.00,1,'2018-08-12','20:41:00'),(4,'GR-00004','',4,1500.00,1,'2018-08-13','22:16:56'),(5,'GR-00005','CASH',2,2.00,1,'2018-08-21','20:06:46');

/*Table structure for table `guest` */

DROP TABLE IF EXISTS `guest`;

CREATE TABLE `guest` (
  `guest_id` varchar(10) NOT NULL,
  `id_no` varchar(25) DEFAULT NULL,
  `first_name` varchar(25) DEFAULT NULL,
  `last_name` varchar(25) DEFAULT NULL,
  `mobile_no` varchar(20) DEFAULT NULL,
  `gender` enum('MALE','FEMALE') DEFAULT NULL,
  `passport_no` varchar(25) DEFAULT NULL,
  `address` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `country_id` int(11) DEFAULT NULL,
  `is_temp` enum('Y','N') DEFAULT 'N',
  PRIMARY KEY (`guest_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `guest` */

insert  into `guest`(`guest_id`,`id_no`,`first_name`,`last_name`,`mobile_no`,`gender`,`passport_no`,`address`,`email`,`country_id`,`is_temp`) values ('GU-000001','912061779V','KRISHAN','CHAMARA','0716960436','MALE','N/A','N/A','sakrishan@gmail.com',199,'N'),('GU-000002','15456665V','KASUN','SANJAYA','5464864','MALE','N/A','N/A','dnjwnedb@njkd.ewk',199,'N'),('GU-000003','','PRADEEP','EKANAYAKA','456','MALE','N/A','N/A','5D',199,'N'),('GU-000004','DF','AS','SAD','23','MALE','N/A','N/A','DF',199,'N'),('GU-000005','D','DSF','DF','DSF','MALE','N/A','N/A','DF',199,'N'),('GU-000006','C','C','C','C','MALE','N/A','N/A','C',199,'N'),('GU-000007','A','A','A','A','MALE','N/A','N/A','A',199,'N'),('GU-000008','AS','S','S','S','MALE','N/A','N/A','S',199,'N'),('GU-000009','f','f','f','f','MALE','N/A','N/A','f',199,'N'),('GU-000010','x','x','x','4','MALE','N/A','N/A','rt',199,'N'),('GU-000011','123v','rrr','ttt','33','MALE','N/A','N/A','ff',199,'N'),('GU-000012','kamal','nimla','dad','sa','MALE','N/A','N/A','d',199,'N'),('GU-000013','sdasd','ad','add','34','MALE','N/A','N/A','',199,'N'),('GU-000014','aa.','a','a','a','MALE','N/A','N/A','a',199,'N');

/*Table structure for table `issue_cheque` */

DROP TABLE IF EXISTS `issue_cheque`;

CREATE TABLE `issue_cheque` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `grn_no` varchar(15) DEFAULT NULL,
  `amount` double(30,2) NOT NULL DEFAULT '0.00',
  `cheque_date` date NOT NULL,
  `issued_date` date NOT NULL,
  `supplier_id` int(3) DEFAULT NULL,
  `bank_id` int(10) DEFAULT NULL,
  `cheque_no` varchar(20) NOT NULL,
  `issued_time` time NOT NULL,
  `issued_by` int(10) NOT NULL,
  PRIMARY KEY (`cheque_no`,`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `issue_cheque` */

/*Table structure for table `item` */

DROP TABLE IF EXISTS `item`;

CREATE TABLE `item` (
  `item_id` int(11) NOT NULL AUTO_INCREMENT,
  `barcode` varchar(50) DEFAULT NULL,
  `item_category` int(11) DEFAULT NULL,
  `item_name` varchar(100) DEFAULT NULL,
  `item_type_id` int(10) NOT NULL,
  `item_status` enum('ENABLE','DISABLE') DEFAULT NULL,
  `qty_handle` enum('0','1') NOT NULL DEFAULT '1',
  PRIMARY KEY (`item_id`)
) ENGINE=MyISAM AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;

/*Data for the table `item` */

insert  into `item`(`item_id`,`barcode`,`item_category`,`item_name`,`item_type_id`,`item_status`,`qty_handle`) values (1,'1',1,'Fish Molee',1,'ENABLE','0'),(2,'2',1,'Chicken Chettinad',1,'ENABLE','0'),(3,'3',1,'Kerala Karimeen Fry',1,'ENABLE','0'),(4,'4',1,'Kerala Chicken Roast',1,'ENABLE','0'),(5,'5',2,'Double Barley Posole',1,'ENABLE','0'),(6,'6',2,'Supercrunch Tofu Tacos',1,'ENABLE','0'),(7,'7',2,'Eggs in Purgatory',1,'ENABLE','0'),(8,'8',3,'Battenberg cake',1,'ENABLE','0'),(9,'9',3,'Madeira cakes',1,'ENABLE','0'),(10,'10',3,'Ice Cream',2,'ENABLE','0'),(11,'11',5,'Waffle Fries',2,'ENABLE','0'),(12,'12',5,'Double-Double',2,'ENABLE','0'),(13,'13',5,'Fries',2,'ENABLE','0'),(14,'14',4,'Coca Cola',2,'ENABLE','0'),(15,'15',4,'Fanta',2,'ENABLE','0'),(16,'16',4,'Nestea Lemon',1,'ENABLE','0'),(17,'17',6,'Samba Rice',4,'ENABLE','1'),(18,'18',6,'EGGS',4,'ENABLE','1');

/*Table structure for table `item_type` */

DROP TABLE IF EXISTS `item_type`;

CREATE TABLE `item_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

/*Data for the table `item_type` */

insert  into `item_type`(`id`,`type`) values (1,'KOT ITEMS\r\n'),(2,'BOT ITEM'),(3,'LAUNDRY ITEMS '),(4,'KOT INGRADIENTS'),(5,'NON SELLABLE ITRMS');

/*Table structure for table `kot_order` */

DROP TABLE IF EXISTS `kot_order`;

CREATE TABLE `kot_order` (
  `order_no` varchar(10) NOT NULL,
  `reservation_no` varchar(10) DEFAULT NULL,
  `room_id` int(11) DEFAULT NULL,
  `table_id` int(11) DEFAULT NULL,
  `tax_percentage` double(10,2) NOT NULL,
  `total_price` double(30,2) NOT NULL,
  `added_by` int(11) NOT NULL,
  `added_date` date NOT NULL,
  `added_time` time NOT NULL,
  `order_status` enum('Pending','In preparation','Prepared','Served') NOT NULL DEFAULT 'Pending',
  `special_note` text,
  PRIMARY KEY (`order_no`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Data for the table `kot_order` */

insert  into `kot_order`(`order_no`,`reservation_no`,`room_id`,`table_id`,`tax_percentage`,`total_price`,`added_by`,`added_date`,`added_time`,`order_status`,`special_note`) values ('KO-00001','RS-0000003',1,1,25.04,62.00,1,'2018-08-12','23:13:32','Served','dc snmmsscmn mdscdsnm csd c smdm cdms m'),('KO-00002','RS-0000017',3,2,25.04,28.75,1,'2018-08-14','17:08:41','Served','malu abul tiyal ekak ona');

/*Table structure for table `kot_order_item` */

DROP TABLE IF EXISTS `kot_order_item`;

CREATE TABLE `kot_order_item` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `item_stock_id` int(10) NOT NULL,
  `order_qty` double(20,2) NOT NULL,
  `unit_price` double(30,2) NOT NULL,
  `total_price` double(30,2) NOT NULL,
  `order_no` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

/*Data for the table `kot_order_item` */

insert  into `kot_order_item`(`id`,`item_stock_id`,`order_qty`,`unit_price`,`total_price`,`order_no`) values (1,1,1.00,26.00,26.00,'KO-00002'),(2,2,1.00,27.00,27.00,'KO-00002'),(3,4,1.00,9.00,9.00,'KO-00002'),(4,1,1.00,20.00,20.00,'KO-00002'),(5,8,1.00,8.75,8.75,'KO-00002');

/*Table structure for table `kot_order_update_log` */

DROP TABLE IF EXISTS `kot_order_update_log`;

CREATE TABLE `kot_order_update_log` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `updated_by` int(11) NOT NULL,
  `updated_date` date NOT NULL,
  `updated_time` time NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Data for the table `kot_order_update_log` */

/*Table structure for table `meal_types` */

DROP TABLE IF EXISTS `meal_types`;

CREATE TABLE `meal_types` (
  `meal_type_id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(10) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL,
  `adult_meal_price` double(30,2) DEFAULT NULL,
  `child_meal_price` double(30,2) DEFAULT NULL,
  `meal_plan_status` enum('1','0') DEFAULT '1',
  PRIMARY KEY (`meal_type_id`),
  UNIQUE KEY `type` (`type`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Data for the table `meal_types` */

insert  into `meal_types`(`meal_type_id`,`type`,`description`,`adult_meal_price`,`child_meal_price`,`meal_plan_status`) values (1,'BB','breakfast, beverages',20.00,8.00,'1'),(2,'HB','breakfast and dinner, beverages (tea, coffee, water) are for free on breakfast, but are to be paid o',5.00,2.00,'1'),(3,'FB ','breakfast, lunch, dinner; beverages (tea, coffee, water) are for free on breakfast, but are to be pa',20.00,10.00,'1');

/*Table structure for table `purchased_mini_bar_item` */

DROP TABLE IF EXISTS `purchased_mini_bar_item`;

CREATE TABLE `purchased_mini_bar_item` (
  `reservation_no` varchar(15) DEFAULT NULL,
  `stock_id` int(11) DEFAULT NULL,
  `qty` double(5,2) DEFAULT NULL,
  `price` double(20,2) DEFAULT NULL,
  KEY `reservation_no` (`reservation_no`),
  KEY `room_id` (`stock_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `purchased_mini_bar_item` */

insert  into `purchased_mini_bar_item`(`reservation_no`,`stock_id`,`qty`,`price`) values ('RS-0000003',10,1.00,5.00),('RS-0000003',11,1.00,10.00),('RS-0000003',13,1.00,2.00),('RS-0000007',11,2.00,12.61),('RS-0000009',10,1.00,6.30),('RS-0000017',14,2.00,1.26),('RS-0000020',10,1.00,6.30),('RS-0000020',11,1.00,12.60);

/*Table structure for table `received_cheque` */

DROP TABLE IF EXISTS `received_cheque`;

CREATE TABLE `received_cheque` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `reservation_no` varchar(15) DEFAULT NULL,
  `amount` double(30,2) NOT NULL DEFAULT '0.00',
  `cheque_date` date NOT NULL,
  `issued_date` date NOT NULL,
  `guest_id` varchar(10) DEFAULT NULL,
  `bank_id` int(10) DEFAULT NULL,
  `cheque_no` varchar(20) NOT NULL,
  `issued_time` time NOT NULL,
  `issued_by` int(10) NOT NULL,
  PRIMARY KEY (`cheque_no`,`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `received_cheque` */

insert  into `received_cheque`(`id`,`reservation_no`,`amount`,`cheque_date`,`issued_date`,`guest_id`,`bank_id`,`cheque_no`,`issued_time`,`issued_by`) values (1,'RS-0000006',200.00,'2018-08-16','2018-08-14','GU-000006',1,'ER','18:26:55',1);

/*Table structure for table `recervation_price` */

DROP TABLE IF EXISTS `recervation_price`;

CREATE TABLE `recervation_price` (
  `reservation_no` varchar(15) NOT NULL,
  `total_room_charges` double(20,2) DEFAULT '0.00',
  `total_adult_food_charges` double(20,2) DEFAULT '0.00',
  `total_child_food_charges` double(20,2) DEFAULT '0.00',
  `additional_service_charge` double(20,2) DEFAULT '0.00',
  `mini_bar_item_price` double(20,2) DEFAULT '0.00',
  `kot_charges` double(20,2) DEFAULT '0.00',
  `tax_pecentage` double(10,2) DEFAULT '0.00',
  `sub_toal_with_tax` double(20,2) DEFAULT '0.00',
  `agent_commitions` double(20,2) DEFAULT '0.00',
  `paid` double(20,0) DEFAULT '0',
  `discount` double(30,2) DEFAULT '0.00',
  `status` enum('PENDING','COMPLETE') DEFAULT 'PENDING',
  PRIMARY KEY (`reservation_no`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Data for the table `recervation_price` */

insert  into `recervation_price`(`reservation_no`,`total_room_charges`,`total_adult_food_charges`,`total_child_food_charges`,`additional_service_charge`,`mini_bar_item_price`,`kot_charges`,`tax_pecentage`,`sub_toal_with_tax`,`agent_commitions`,`paid`,`discount`,`status`) values ('RS-0000002',1062.84,100.03,20.01,62.52,0.00,62.00,25.04,1245.40,0.00,1200,45.40,'COMPLETE'),('RS-0000003',312.60,25.21,10.08,63.02,17.00,62.00,26.04,482.62,0.00,480,2.62,'COMPLETE'),('RS-0000004',250.00,20.00,8.00,50.00,0.00,0.00,26.04,328.00,0.00,0,0.00,'PENDING'),('RS-0000005',312.60,25.21,10.08,0.00,0.00,0.00,26.04,347.89,0.00,300,47.89,'COMPLETE'),('RS-0000006',1156.62,25.21,20.17,63.02,0.00,0.00,26.04,1265.02,0.00,1200,65.02,'COMPLETE'),('RS-0000007',531.42,25.21,10.08,63.02,25.21,0.00,26.04,654.94,0.00,654,0.94,'COMPLETE'),('RS-0000009',312.60,25.21,10.08,63.02,6.30,0.00,26.04,417.21,0.00,410,7.21,'COMPLETE'),('RS-0000011',312.60,25.21,10.08,63.02,0.00,0.00,26.04,410.91,0.00,0,0.00,'PENDING'),('RS-0000014',312.60,50.42,20.17,0.00,0.00,0.00,26.04,383.19,0.00,383,0.19,'COMPLETE'),('RS-0000015',312.60,25.21,10.08,63.02,0.00,0.00,26.04,410.91,41.09,400,10.91,'COMPLETE'),('RS-0000017',937.80,75.62,30.25,63.02,2.52,0.00,26.04,1109.21,0.00,1109,0.21,'COMPLETE'),('RS-0000016',848.27,25.21,12.60,12.60,0.00,0.00,26.04,898.68,0.00,0,0.00,'PENDING'),('RS-0000019',2031.90,50.42,10.08,12.60,0.00,0.00,26.04,2105.00,0.00,0,0.00,'PENDING'),('RS-0000020',627.70,25.21,10.08,75.62,18.90,0.00,26.04,757.51,0.00,750,7.51,'COMPLETE'),('RS-0000023',315.10,25.21,10.08,0.00,0.00,0.00,26.04,350.39,0.00,350,0.39,'COMPLETE');

/*Table structure for table `recerved_rooms` */

DROP TABLE IF EXISTS `recerved_rooms`;

CREATE TABLE `recerved_rooms` (
  `reservation_no` varchar(15) DEFAULT NULL,
  `room_id` int(11) DEFAULT NULL,
  `room_charge` double(30,2) DEFAULT NULL,
  `house_keeping` enum('Yes','No') DEFAULT 'No'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Data for the table `recerved_rooms` */

insert  into `recerved_rooms`(`reservation_no`,`room_id`,`room_charge`,`house_keeping`) values ('RS-0000002',6,531.42,'No'),('RS-0000003',3,312.60,'No'),('RS-0000004',1,250.00,'No'),('RS-0000005',4,312.60,'No'),('RS-0000006',5,531.42,'No'),('RS-0000006',3,312.60,'No'),('RS-0000006',2,312.60,'No'),('RS-0000007',6,531.42,'No'),('RS-0000008',7,1500.48,'No'),('RS-0000009',1,312.60,'No'),('RS-0000010',1,312.60,'No'),('RS-0000011',2,312.60,'No'),('RS-0000012',1,312.60,'No'),('RS-0000013',1,312.60,'No'),('RS-0000014',3,312.60,'No'),('RS-0000015',4,312.60,'Yes'),('RS-0000016',2,312.60,'No'),('RS-0000017',3,312.60,'No'),('RS-0000017',5,535.67,'No'),('RS-0000017',3,312.60,'No'),('RS-0000019',7,1500.48,'No'),('RS-0000019',6,531.42,'No'),('RS-0000020',2,312.60,'No'),('RS-0000020',1,315.10,'Yes'),('RS-0000021',3,312.60,'Yes'),('RS-0000021',5,531.42,'Yes'),('RS-0000022',7,1500.48,'Yes'),('RS-0000022',6,531.42,'No'),('RS-0000022',4,312.60,'No'),('RS-0000023',2,315.10,'No');

/*Table structure for table `reservation` */

DROP TABLE IF EXISTS `reservation`;

CREATE TABLE `reservation` (
  `reservation_id` varchar(10) NOT NULL,
  `guest_id` varchar(10) DEFAULT NULL,
  `no_of_rooms` int(11) DEFAULT NULL,
  `no_of_adult` int(11) DEFAULT NULL,
  `no_of_child` int(11) DEFAULT NULL,
  `arrival_date` date DEFAULT NULL,
  `depature_Date` date DEFAULT NULL,
  `no_of_nights` int(11) DEFAULT NULL,
  `reserved_by` varchar(15) DEFAULT NULL,
  `agent_id` int(11) DEFAULT NULL,
  `additional_note` varchar(255) DEFAULT NULL,
  `added_date` date DEFAULT NULL,
  `added_time` time DEFAULT NULL,
  `added_by` int(11) DEFAULT NULL,
  `status` enum('PENDING','CHECKED IN','CHECKED OUT','CANCELED') DEFAULT 'PENDING',
  `meal_type_id` int(11) DEFAULT '-1',
  `tax_status` enum('0','1') DEFAULT NULL COMMENT '0- without tax 1-with tax',
  PRIMARY KEY (`reservation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Data for the table `reservation` */

insert  into `reservation`(`reservation_id`,`guest_id`,`no_of_rooms`,`no_of_adult`,`no_of_child`,`arrival_date`,`depature_Date`,`no_of_nights`,`reserved_by`,`agent_id`,`additional_note`,`added_date`,`added_time`,`added_by`,`status`,`meal_type_id`,`tax_status`) values ('RS-0000001','GU-000001',0,2,0,'2018-08-12','2018-08-14',2,'ON TIME',0,'mfkwen edjewndjkw dwjekdw','2018-08-12','22:27:03',1,'PENDING',-1,NULL),('RS-0000002','GU-000002',0,2,1,'2018-08-12','2018-08-14',2,'ON TIME',NULL,'','2018-08-12','22:42:44',1,'CHECKED OUT',1,NULL),('RS-0000003','GU-000003',1,1,1,'2018-08-13','2018-08-14',1,'CALL',NULL,'S\r\n','2018-08-13','17:19:43',1,'CHECKED OUT',1,NULL),('RS-0000004','GU-000004',1,1,1,'2018-08-14','2018-08-15',1,'AGENT',2,'','2018-08-14','11:53:16',1,'CHECKED OUT',1,'1'),('RS-0000005','GU-000005',1,1,1,'2018-08-14','2018-08-15',1,'AGENT',NULL,'D','2018-08-14','18:24:30',1,'CHECKED OUT',1,'1'),('RS-0000006','GU-000006',3,1,2,'2018-08-14','2018-08-15',1,'CALL',NULL,'CC','2018-08-14','18:26:04',1,'CHECKED OUT',1,'1'),('RS-0000007','GU-000007',1,1,1,'2018-08-14','2018-08-15',1,'AGENT',NULL,'QAQWE','2018-08-14','20:47:14',1,'CHECKED OUT',1,'1'),('RS-0000008','GU-000008',1,1,1,'2018-08-14','2018-08-15',1,'AGENT',1,'','2018-08-14','20:49:33',1,'PENDING',-1,'1'),('RS-0000009','GU-000008',1,1,1,'2018-08-15','2018-08-16',1,'AGENT',NULL,'AS','2018-08-14','20:54:50',1,'CHECKED OUT',1,'1'),('RS-0000011','GU-000007',1,1,1,'2018-08-16','2018-08-17',1,'AGENT',NULL,'d','2018-08-14','21:23:18',1,'CHECKED IN',1,'1'),('RS-0000012','GU-000009',1,1,1,'2018-08-16','2018-08-17',1,'AGENT',3,'\r\n','2018-08-14','21:25:39',1,'PENDING',-1,'1'),('RS-0000013','GU-000005',1,1,1,'2018-08-17','2019-08-17',365,'AGENT',4,'po','2018-08-14','21:28:41',1,'PENDING',-1,'1'),('RS-0000014','GU-000010',1,2,2,'2018-08-18','2018-08-19',1,'AGENT',2,'[','2018-08-14','21:29:34',1,'CHECKED OUT',1,'1'),('RS-0000015','GU-000011',1,1,1,'2018-08-17','2018-08-18',1,'AGENT',1,'fg','2018-08-14','22:02:01',1,'CHECKED OUT',1,'1'),('RS-0000016','GU-000001',2,1,1,'2018-08-22','2018-08-23',1,'CALL',NULL,'','2018-08-15','13:15:13',1,'CHECKED IN',3,'1'),('RS-0000017','GU-000012',1,1,1,'2018-08-19','2018-08-22',3,'CALL',NULL,'ui','2018-08-16','09:32:40',1,'CHECKED OUT',1,'1'),('RS-0000018','GU-000013',1,1,1,'2018-08-22','2018-08-23',1,'CALL',0,'\r\n\r\n','2018-08-21','20:03:20',1,'PENDING',-1,'1'),('RS-0000019','GU-000001',2,2,1,'2018-08-22','2018-08-23',1,'WEB',NULL,'','2018-08-21','20:03:55',1,'CHECKED IN',1,'1'),('RS-0000020','GU-000007',2,1,1,'2018-08-27','2018-08-28',1,'CALL',NULL,'s','2018-08-27','12:03:15',1,'CHECKED OUT',1,'1'),('RS-0000021','GU-000001',2,1,1,'2018-09-01','2018-09-02',1,'CALL',0,'','2018-09-06','17:44:13',1,'PENDING',-1,'1'),('RS-0000022','GU-000001',3,1,0,'2018-09-01','2018-09-02',1,'CALL',0,'','2018-09-06','17:44:45',1,'PENDING',-1,'1'),('RS-0000023','GU-000001',1,1,1,'2018-09-28','2018-09-29',1,'CALL',NULL,'d\r\n','2018-09-25','10:37:16',1,'CHECKED OUT',1,'1');

/*Table structure for table `resturant_table` */

DROP TABLE IF EXISTS `resturant_table`;

CREATE TABLE `resturant_table` (
  `table_id` int(10) NOT NULL AUTO_INCREMENT,
  `table_no` varchar(60) NOT NULL,
  PRIMARY KEY (`table_id`),
  UNIQUE KEY `table_no` (`table_no`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

/*Data for the table `resturant_table` */

insert  into `resturant_table`(`table_id`,`table_no`) values (1,'01'),(2,'02'),(3,'03'),(4,'04'),(5,'05'),(6,'06');

/*Table structure for table `room` */

DROP TABLE IF EXISTS `room`;

CREATE TABLE `room` (
  `room_id` int(11) NOT NULL AUTO_INCREMENT,
  `room_name` varchar(20) NOT NULL,
  `room_package_id` int(10) DEFAULT NULL,
  `room_floor` varchar(60) DEFAULT NULL,
  `description` text,
  `room_type` enum('AC','NON AC') NOT NULL,
  `current_status` enum('RESERVED','AVAILABLE','MAINTANANCE','HOUSE KEEPING','CHECKED IN','CHECKED OUT') DEFAULT 'AVAILABLE',
  PRIMARY KEY (`room_id`),
  UNIQUE KEY `room_name` (`room_name`),
  KEY `room_package_id` (`room_package_id`),
  CONSTRAINT `room_ibfk_1` FOREIGN KEY (`room_package_id`) REFERENCES `room_packages` (`room_package_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

/*Data for the table `room` */

insert  into `room`(`room_id`,`room_name`,`room_package_id`,`room_floor`,`description`,`room_type`,`current_status`) values (1,'RM-01',1,'1ST','N/A','AC','AVAILABLE'),(2,'RM-02',1,'1ST','N/A','AC','CHECKED OUT'),(3,'RM-03',1,'1ST','N/A','AC','AVAILABLE'),(4,'RM-04',1,'1ST','N/A','AC','RESERVED'),(5,'RM-05',2,'2ND','N/A','AC','AVAILABLE'),(6,'RM-06',2,'2ND','N/A','AC','RESERVED'),(7,'RM-07',3,'3RD','N/A','AC','AVAILABLE');

/*Table structure for table `room_packages` */

DROP TABLE IF EXISTS `room_packages`;

CREATE TABLE `room_packages` (
  `room_package_id` int(10) NOT NULL AUTO_INCREMENT,
  `package_name` varchar(50) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL,
  `condition` enum('AC','NON AC') DEFAULT NULL,
  `room_package_price` double(30,2) DEFAULT NULL,
  `package_status` enum('0','1') NOT NULL DEFAULT '1',
  `package_color` varchar(60) NOT NULL,
  PRIMARY KEY (`room_package_id`),
  UNIQUE KEY `package_name` (`package_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Data for the table `room_packages` */

insert  into `room_packages`(`room_package_id`,`package_name`,`description`,`condition`,`room_package_price`,`package_status`,`package_color`) values (1,'SINGLE ROOM WITH AC','-','AC',250.00,'1','#400080'),(2,'DOUBLE ROOM WITH AC','-','AC',425.00,'1','#004000'),(3,'SUITE','-','AC',1200.00,'1','#400000');

/*Table structure for table `spilt_invoice` */

DROP TABLE IF EXISTS `spilt_invoice`;

CREATE TABLE `spilt_invoice` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `reservation_id` varchar(25) DEFAULT NULL,
  `description` varchar(200) DEFAULT NULL,
  `price` double(10,2) DEFAULT NULL,
  `total_price` double(10,2) DEFAULT NULL,
  `type` enum('ROOM','MEAL','KOT','ADDITIONAL','MINI BAR') DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

/*Data for the table `spilt_invoice` */

insert  into `spilt_invoice`(`id`,`reservation_id`,`description`,`price`,`total_price`,`type`) values (1,'RS-0000017','TOTAL ROOM CHARGES',1160.87,1189.62,'ROOM'),(2,'RS-0000017','TOTAL MEAL CHARGES',0.00,1189.62,'MEAL'),(3,'RS-0000017','TOTAL MINI BAR CHARGES',0.00,1189.62,'MINI BAR'),(4,'RS-0000017','TOTAL ADDITIONAL SERVICE CHARGES',0.00,1189.62,'ADDITIONAL'),(5,'RS-0000017','KOT - KO-00002',28.75,1189.62,'KOT');

/*Table structure for table `stock` */

DROP TABLE IF EXISTS `stock`;

CREATE TABLE `stock` (
  `stock_id` int(10) NOT NULL AUTO_INCREMENT,
  `item_code` int(10) NOT NULL,
  `qty` double(20,3) NOT NULL,
  `cost_price` double(30,2) NOT NULL,
  `sales_price` double(30,2) NOT NULL,
  `section` varchar(10) NOT NULL DEFAULT 'SEC-01',
  PRIMARY KEY (`stock_id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;

/*Data for the table `stock` */

insert  into `stock`(`stock_id`,`item_code`,`qty`,`cost_price`,`sales_price`,`section`) values (1,1,0.000,25.00,20.00,'SEC-01'),(2,2,0.000,30.00,27.00,'SEC-01'),(3,3,0.000,24.00,19.00,'SEC-01'),(4,4,0.000,13.00,9.00,'SEC-01'),(5,5,0.000,6.00,5.00,'SEC-01'),(6,6,0.000,8.00,7.00,'SEC-01'),(7,7,0.000,9.00,6.50,'SEC-01'),(8,8,0.000,12.00,8.75,'SEC-01'),(9,9,0.000,5.00,4.50,'SEC-01'),(10,10,-3.000,10.00,5.00,'SEC-01'),(11,11,-4.000,12.00,10.00,'SEC-01'),(12,12,0.000,5.00,3.00,'SEC-01'),(13,13,-1.000,3.00,2.00,'SEC-01'),(14,14,-2.000,2.00,1.00,'SEC-01'),(15,15,0.000,2.00,1.00,'SEC-01'),(16,16,0.000,3.00,2.00,'SEC-01'),(21,17,50.000,1500.00,0.00,'SEC-01'),(22,18,100.000,12.00,0.00,'SEC-01'),(23,17,10.000,150.00,0.00,'SEC-01'),(24,11,1.000,2.00,5.00,'SEC-01');

/*Table structure for table `stock_items` */

DROP TABLE IF EXISTS `stock_items`;

CREATE TABLE `stock_items` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `item_code` int(10) NOT NULL,
  `qty` double(20,3) DEFAULT '0.000',
  `cost_price` double(30,2) DEFAULT '0.00',
  `sales_price` double(30,2) DEFAULT '0.00',
  `grn_no` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

/*Data for the table `stock_items` */

insert  into `stock_items`(`id`,`item_code`,`qty`,`cost_price`,`sales_price`,`grn_no`) values (5,17,50.000,1500.00,0.00,'GR-00003'),(6,18,100.000,12.00,0.00,'GR-00003'),(7,17,10.000,150.00,0.00,'GR-00004'),(8,11,1.000,2.00,5.00,'GR-00005');

/*Table structure for table `stock_section` */

DROP TABLE IF EXISTS `stock_section`;

CREATE TABLE `stock_section` (
  `section_no` varchar(10) NOT NULL,
  `section_name` varchar(60) NOT NULL,
  PRIMARY KEY (`section_no`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Data for the table `stock_section` */

insert  into `stock_section`(`section_no`,`section_name`) values ('SEC-01','Main Stock');

/*Table structure for table `supplier` */

DROP TABLE IF EXISTS `supplier`;

CREATE TABLE `supplier` (
  `supplier_id` int(11) NOT NULL AUTO_INCREMENT,
  `supplier_name` varchar(50) DEFAULT NULL,
  `company_name` varchar(50) DEFAULT NULL,
  `address` varchar(100) DEFAULT NULL,
  `contact1` varchar(15) DEFAULT NULL,
  `contact2` varchar(15) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `status` enum('ENABLE','DISABLE') NOT NULL DEFAULT 'ENABLE',
  PRIMARY KEY (`supplier_id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Data for the table `supplier` */

insert  into `supplier`(`supplier_id`,`supplier_name`,`company_name`,`address`,`contact1`,`contact2`,`note`,`status`) values (2,'SUPPLIER','SUPPLIER','KANDY\r\n\r\n','123','456','N/A\r\n','ENABLE'),(3,'ABC','ABC','\r\n','','','\r\nN/A','ENABLE'),(4,'KRISHAN CHAMARA','ABC COMPANY','-','15646466','4684646','NEF3EW 3JKB 3','ENABLE');

/*Table structure for table `supplier_account` */

DROP TABLE IF EXISTS `supplier_account`;

CREATE TABLE `supplier_account` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account_no` varchar(15) NOT NULL,
  `supplier_id` int(3) NOT NULL,
  `grn_no` varchar(15) NOT NULL,
  `grn_total` double(30,2) NOT NULL DEFAULT '0.00',
  `paid` double(30,2) NOT NULL DEFAULT '0.00',
  `due` double(30,2) NOT NULL DEFAULT '0.00',
  `added_date` date DEFAULT NULL,
  `added_time` time DEFAULT NULL,
  `added_by` int(11) DEFAULT NULL,
  PRIMARY KEY (`account_no`,`supplier_id`,`id`),
  KEY `account_no` (`account_no`),
  KEY `int` (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `supplier_account` */

insert  into `supplier_account`(`id`,`account_no`,`supplier_id`,`grn_no`,`grn_total`,`paid`,`due`,`added_date`,`added_time`,`added_by`) values (1,'CR-00001',4,'GR-00003',76200.00,6200.00,70000.00,'2018-08-12','20:41:00',1),(2,'CR-00002',4,'GR-00004',1500.00,0.00,1500.00,'2018-08-13','22:16:56',1);

/*Table structure for table `tax_details` */

DROP TABLE IF EXISTS `tax_details`;

CREATE TABLE `tax_details` (
  `tax_type_id` int(10) NOT NULL AUTO_INCREMENT,
  `tax_type` varchar(60) NOT NULL,
  `precentage` double(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`tax_type_id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Data for the table `tax_details` */

insert  into `tax_details`(`tax_type_id`,`tax_type`,`precentage`) values (1,'NBT',2.04),(2,'SC',10.00),(3,'VAT',12.00),(4,'TDL',1.00);

/*Table structure for table `user_login` */

DROP TABLE IF EXISTS `user_login`;

CREATE TABLE `user_login` (
  `user_id` int(10) NOT NULL AUTO_INCREMENT,
  `user_name` varchar(10) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `user_type` enum('administrator','user','Manager','system_administrator') DEFAULT NULL,
  `last_login_datetime` datetime DEFAULT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `user_login` */

insert  into `user_login`(`user_id`,`user_name`,`password`,`user_type`,`last_login_datetime`) values (1,'admin','soiPCPFb06Y=','system_administrator','2018-09-25 11:25:08'),(2,'admin','123','system_administrator','2018-08-27 16:03:16');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
