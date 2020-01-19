-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: localhost    Database: survey
-- ------------------------------------------------------
-- Server version	8.0.18

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
-- Table structure for table `mulit_survey_item`
--

DROP TABLE IF EXISTS `mulit_survey_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mulit_survey_item` (
  `top_idx` int(11) NOT NULL,
  `type` char(8) DEFAULT NULL,
  `view` char(3) DEFAULT NULL,
  `question` varchar(100) DEFAULT NULL,
  `total` int(11) DEFAULT NULL,
  `name` varchar(10) DEFAULT NULL,
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`idx`),
  KEY `top_idx` (`top_idx`)
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mulit_survey_item`
--

LOCK TABLES `mulit_survey_item` WRITE;
/*!40000 ALTER TABLE `mulit_survey_item` DISABLE KEYS */;
INSERT INTO `mulit_survey_item` VALUES (29,'Radio','ON','남성',0,'A',26),(29,'Radio','ON','여성',0,'A',27),(30,'Radio','ON','10대',0,'1',28),(30,'Radio','ON','20대',0,'1',29),(30,'Radio','ON','30대',0,'1',30),(32,'Check','ON','JSP',0,'1',31),(32,'Check','ON','PHP',0,'1',32),(32,'Check','ON','JS',0,'1',33),(32,'Check','ON','C#',0,'1',34),(33,'Check','ON','JSP',0,'1',35),(33,'Check','ON','PHP',0,'1',36),(33,'Check','ON','JS',0,'1',37),(33,'Check','ON','C#',0,'1',38),(34,'Radio','ON','Sublime Text',0,'1',39),(34,'Radio','ON','Atom',0,'1',40),(34,'Radio','ON','Brackets',0,'1',41),(34,'Radio','ON','Visual Studio Code',0,'1',42),(35,'Radio','ON','매우 만족',0,'1',43),(35,'Radio','ON','만족',0,'1',44),(35,'Radio','ON','보통',0,'1',45),(35,'Radio','ON','불만족',0,'1',46),(35,'Radio','ON','매우 불만족',0,'1',47),(37,'Check','ON','고기',0,'1',48),(37,'Check','ON','야채',0,'1',49),(37,'Check','ON','국',0,'1',50),(37,'Check','ON','기타',0,'1',51);
/*!40000 ALTER TABLE `mulit_survey_item` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-20  1:11:43
