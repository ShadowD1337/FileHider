-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: filehider
-- ------------------------------------------------------
-- Server version	8.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('00000000000000_CreateIdentitySchema','8.0.3');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL,
  `RoleId` varchar(256) NOT NULL,
  `ClaimType` varchar(256) DEFAULT NULL,
  `ClaimValue` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(256) NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `ConcurrencyStamp` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL,
  `UserId` varchar(256) NOT NULL,
  `ClaimType` varchar(256) DEFAULT NULL,
  `ClaimValue` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `ProviderKey` varchar(128) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `ProviderDisplayName` varchar(256) DEFAULT NULL,
  `UserId` varchar(256) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(256) NOT NULL,
  `RoleId` varchar(256) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(256) NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `PasswordHash` varchar(256) DEFAULT NULL,
  `SecurityStamp` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` varchar(256) DEFAULT NULL,
  `PhoneNumber` varchar(20) DEFAULT NULL,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `LockoutEnd` datetime DEFAULT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('1c2ee617-7134-44cd-920a-ee15408cff9a','abv@gmail.com','ABV@GMAIL.COM','abv@gmail.com','ABV@GMAIL.COM',_binary '\0','AQAAAAIAAYagAAAAELzg/nKOi/nzkJ1HcUNeney0y/sCbVtUpCLdPBwANOCVOrrHhIy34lTHpsHnZeY+2w==','F6U6X4NPWCO4IODCNFEMBJAMD2MQYPTG','a461ac52-c832-4b6d-9adc-67a8846ddc93',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('7807db75-37f0-4ab2-950c-ae82eed22f0d','12345@abv.bg','12345@ABV.BG','12345@abv.bg','12345@ABV.BG',_binary '\0','AQAAAAIAAYagAAAAEB8bSa2VOyXS2j42TZzQ0WMpuZw+14PpUGBCG8YumJ4D9CP3xQdodXjcxBD7Wuhl2g==','7HVFSVRHBSHMMT66O7FLALXWREYKGDBI','33dab7c3-f889-430b-9ba3-69450cdda63f',NULL,_binary '\0',_binary '\0',NULL,_binary '',0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(256) NOT NULL,
  `LoginProvider` varchar(128) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hiddeninformations`
--

DROP TABLE IF EXISTS `hiddeninformations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hiddeninformations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `size` int NOT NULL,
  `content` mediumtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hiddeninformations`
--

LOCK TABLES `hiddeninformations` WRITE;
/*!40000 ALTER TABLE `hiddeninformations` DISABLE KEYS */;
INSERT INTO `hiddeninformations` VALUES (1,4,'test'),(8,0,'test123'),(9,0,'test123'),(10,0,'test123'),(11,0,'test123'),(12,0,'test123'),(13,0,'test123'),(14,0,'test123'),(15,0,'test123'),(16,0,'test123'),(17,0,'test123'),(18,10,'popoppopop');
/*!40000 ALTER TABLE `hiddeninformations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `imagefiles`
--

DROP TABLE IF EXISTS `imagefiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imagefiles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `stego_strat_id` int DEFAULT NULL,
  `download_link` varchar(200) DEFAULT NULL,
  `hidden_info_id` int DEFAULT NULL,
  `total_byte_capacity` int DEFAULT NULL,
  `byte_capacity_left` int NOT NULL,
  `user_id` varchar(256) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `hiddeninformation_hiddeninfo_id_idx` (`hidden_info_id`),
  KEY `imagestegostrategies_stegostrategy_id_idx` (`stego_strat_id`),
  KEY `aspnetusers_user_id_idx` (`user_id`),
  CONSTRAINT `aspnetusers_user_id` FOREIGN KEY (`user_id`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `hiddeninformation_hiddeninfo_id` FOREIGN KEY (`hidden_info_id`) REFERENCES `hiddeninformations` (`id`),
  CONSTRAINT `imagestegostrategies_stegostrategy_id` FOREIGN KEY (`stego_strat_id`) REFERENCES `imagestegostrategies` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imagefiles`
--

LOCK TABLES `imagefiles` WRITE;
/*!40000 ALTER TABLE `imagefiles` DISABLE KEYS */;
INSERT INTO `imagefiles` VALUES (4,1,'test',1,999,0,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(5,5,'https://www.dropbox.com/scl/fi/9c4dqeknlftttuuiop0ih/test.jpg?rlkey=0tpg8k460gg9y2oq4k62isf3z&dl=0',10,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(6,6,'https://www.dropbox.com/scl/fi/9c4dqeknlftttuuiop0ih/test.jpg?rlkey=0tpg8k460gg9y2oq4k62isf3z&dl=0',11,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(7,7,'https://www.dropbox.com/scl/fi/6z4tdxxay2kslj3zmhyso/test.jpg?rlkey=klj51it1lbf3g52p44r8d8u3m&dl=0',12,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(8,8,'https://www.dropbox.com/scl/fi/6z4tdxxay2kslj3zmhyso/test.jpg?rlkey=klj51it1lbf3g52p44r8d8u3m&dl=0',13,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(9,9,'https://www.dropbox.com/scl/fi/6z4tdxxay2kslj3zmhyso/test.jpg?rlkey=klj51it1lbf3g52p44r8d8u3m&dl=0',14,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(10,10,'https://firebasestorage.googleapis.com/v0/b/filehider-itcareer.appspot.com/o/Files%2Ftest.jpg',15,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(11,11,'https://firebasestorage.googleapis.com/v0/b/filehider-itcareer.appspot.com/o/Files%2Ftest.jpg',16,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(12,12,'https://firebasestorage.googleapis.com/v0/b/filehider-itcareer.appspot.com/o/Files%2Ftest.jpg',17,230400,230400,'1c2ee617-7134-44cd-920a-ee15408cff9a'),(13,13,'https://firebasestorage.googleapis.com/v0/b/filehider-itcareer.appspot.com/o/Files%2Ftest.jpg?alt=media',18,230400,230390,'7807db75-37f0-4ab2-950c-ae82eed22f0d');
/*!40000 ALTER TABLE `imagefiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `imagestegostrategies`
--

DROP TABLE IF EXISTS `imagestegostrategies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imagestegostrategies` (
  `id` int NOT NULL AUTO_INCREMENT,
  `color_channels` varchar(50) DEFAULT NULL,
  `bits_per_channel` int DEFAULT NULL,
  `pixel_spacing` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imagestegostrategies`
--

LOCK TABLES `imagestegostrategies` WRITE;
/*!40000 ALTER TABLE `imagestegostrategies` DISABLE KEYS */;
INSERT INTO `imagestegostrategies` VALUES (1,'R',NULL,1),(2,'Red,Green',2,1),(3,'Red,Green,Blue',2,1),(4,'Red,Green,Blue',2,1),(5,'Red,Green,Blue',2,1),(6,'Red,Green,Blue',2,1),(7,'Red,Green,Blue',2,1),(8,'Red,Green,Blue',2,1),(9,'Red,Green,Blue',2,1),(10,'Red,Green,Blue',2,1),(11,'Red,Green,Blue',2,1),(12,'Red,Green,Blue',2,1),(13,'Red,Green,Blue',2,1);
/*!40000 ALTER TABLE `imagestegostrategies` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-24  3:03:30
