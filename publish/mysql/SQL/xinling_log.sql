/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_log

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:18:21
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_log;

-- ----------------------------
-- Table structure for log_level
-- ----------------------------
DROP TABLE IF EXISTS `log_level`;
CREATE TABLE `log_level` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `LevelName` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `LevelRemarks` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Status` tinyint(1) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of log_level
-- ----------------------------
BEGIN;
INSERT INTO `log_level` (`Id`, `LevelName`, `LevelRemarks`, `Status`, `CreateTime`, `UpdateTime`) VALUES (1, 'ERROR', '引起系统错误的问题', 1, '2020-05-25 23:28:13', '2020-05-25 23:28:13');
INSERT INTO `log_level` (`Id`, `LevelName`, `LevelRemarks`, `Status`, `CreateTime`, `UpdateTime`) VALUES (2, 'WARN', '可疑的错误', 1, '2020-05-25 23:30:03', '2020-05-25 23:30:03');
INSERT INTO `log_level` (`Id`, `LevelName`, `LevelRemarks`, `Status`, `CreateTime`, `UpdateTime`) VALUES (3, 'INFO', '正常信息', 1, '2020-05-25 23:30:11', '2020-05-25 23:30:11');
INSERT INTO `log_level` (`Id`, `LevelName`, `LevelRemarks`, `Status`, `CreateTime`, `UpdateTime`) VALUES (4, 'DEBUG', 'Debug信息', 1, '2020-05-25 23:31:07', '2020-05-25 23:31:07');
INSERT INTO `log_level` (`Id`, `LevelName`, `LevelRemarks`, `Status`, `CreateTime`, `UpdateTime`) VALUES (5, 'CUSTOM', '自定义信息', 1, '2020-06-02 01:14:00', '2020-06-02 01:14:00');
COMMIT;

-- ----------------------------
-- Table structure for log_mark
-- ----------------------------
DROP TABLE IF EXISTS `log_mark`;
CREATE TABLE `log_mark` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MarkName` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `MarkRemarks` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `ClassId` int(11) NOT NULL,
  `LevelId` int(11) NOT NULL,
  `Status` tinyint(1) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=MyISAM AUTO_INCREMENT=304 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of log_mark
-- ----------------------------
BEGIN;
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (1, '系统错误', NULL, 1, 1, 1, '2020-06-03 23:40:00', '2020-06-03 23:40:00');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (2, '请求SQL记录', '', 1, 3, 0, '2020-06-04 00:06:54', '2020-06-04 00:06:54');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (3, '接口记录', NULL, 2, 3, 1, '2020-10-20 11:31:49', '2020-10-20 11:31:27');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (4, '接口错误记录', NULL, 2, 3, 1, '2021-07-05 16:12:50', '2021-07-05 16:12:40');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (8, '暂时日志', NULL, 1, 3, 1, '2021-07-05 16:22:41', '2021-07-05 16:22:27');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (9, '短信发送', NULL, 4, 3, 1, '2021-07-08 15:10:06', '2021-07-08 15:09:45');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (10, '定时任务错误', NULL, 4, 3, 1, '2021-07-09 15:14:19', '2021-07-09 15:14:01');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (15, '投保单文件生成错误', NULL, 6, 3, 1, '2021-07-11 14:17:53', '2021-07-11 14:17:25');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (46, '统一签章文件回传', NULL, 9, 3, 1, '2021-07-21 14:56:03', '0001-01-01 00:00:00');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (101, '远程文件下载错误', NULL, 9, 3, 1, '2021-07-22 09:53:45', '0001-01-01 00:00:00');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (102, 'FTP错误', NULL, 9, 3, 1, '2021-07-26 17:32:30', '0001-01-01 00:00:00');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (139, 'FTP传输成功', NULL, 9, 3, 1, '2021-08-06 16:37:46', '0001-01-01 00:00:00');
INSERT INTO `log_mark` (`Id`, `MarkName`, `MarkRemarks`, `ClassId`, `LevelId`, `Status`, `CreateTime`, `UpdateTime`) VALUES (303, 'API接口', NULL, 2, 3, 1, '2021-11-12 10:25:30', '2021-11-12 10:25:30');
COMMIT;

-- ----------------------------
-- Table structure for log_mark_class
-- ----------------------------
DROP TABLE IF EXISTS `log_mark_class`;
CREATE TABLE `log_mark_class` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClassName` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `ClassRemarks` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `ParentId` int(11) NOT NULL,
  `RootId` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=MyISAM AUTO_INCREMENT=23 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of log_mark_class
-- ----------------------------
BEGIN;
INSERT INTO `log_mark_class` (`Id`, `ClassName`, `ClassRemarks`, `ParentId`, `RootId`, `CreateTime`, `UpdateTime`) VALUES (1, '系统默认', NULL, 0, 1, '2020-06-03 23:38:50', '2020-06-03 23:38:50');
INSERT INTO `log_mark_class` (`Id`, `ClassName`, `ClassRemarks`, `ParentId`, `RootId`, `CreateTime`, `UpdateTime`) VALUES (2, '接口日志', NULL, 0, 0, '2020-10-20 11:31:14', '2020-10-20 11:31:14');
INSERT INTO `log_mark_class` (`Id`, `ClassName`, `ClassRemarks`, `ParentId`, `RootId`, `CreateTime`, `UpdateTime`) VALUES (4, '其他任务', NULL, 0, 0, '2021-07-08 15:09:43', '2021-07-08 15:09:43');
INSERT INTO `log_mark_class` (`Id`, `ClassName`, `ClassRemarks`, `ParentId`, `RootId`, `CreateTime`, `UpdateTime`) VALUES (6, '自动生成文件操作', NULL, 0, 0, '2021-07-11 14:17:04', '2021-07-11 14:17:04');
INSERT INTO `log_mark_class` (`Id`, `ClassName`, `ClassRemarks`, `ParentId`, `RootId`, `CreateTime`, `UpdateTime`) VALUES (9, '文件相关', NULL, 0, 0, '2021-07-21 14:55:51', '0001-01-01 00:00:00');
COMMIT;

-- ----------------------------
-- Table structure for log_record
-- ----------------------------
DROP TABLE IF EXISTS `log_record`;
CREATE TABLE `log_record` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `MarkId` int(11) NOT NULL,
  `LevelId` int(11) NOT NULL,
  `LogDetail` longtext COLLATE utf8_unicode_ci,
  `LogRemarks` varchar(5000) COLLATE utf8_unicode_ci DEFAULT NULL,
  `LogUrl` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `LogCreatorId` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `LogCreatorIP` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `OperateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DateCode` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=MyISAM AUTO_INCREMENT=202914 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of log_record
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Procedure structure for CommonPagenation
-- ----------------------------
DROP PROCEDURE IF EXISTS `CommonPagenation`;
delimiter ;;
CREATE PROCEDURE `CommonPagenation`(IN tableName VARCHAR ( 255 ),
	IN showFName VARCHAR ( 5000 ),
	IN selectWhere VARCHAR ( 5000 ),
	IN selectOrder VARCHAR ( 200 ),
	IN pageNo INT,
	IN pageSize INT)
BEGIN
	
	SET @startrow = ( pageNo - 1 ) * pageSize;
	
	SET @pagesize = pageSize;
	
	SET @rowindex = 0;
	
	SET @strsql = CONCAT(
		'select SQL_CALC_FOUND_ROWS ',
		showFName,
		' from ',
		tableName,
		CASE
				IFNULL( selectWhere, '' ) 
				WHEN '' THEN
				'' ELSE CONCAT ( ' where ', selectWhere ) 
			END,
		CASE
				IFNULL( selectOrder, '' ) 
				WHEN '' THEN
				'' ELSE CONCAT( ' order by ', selectOrder ) 
			END,
			' limit ',
			@startRow,
			',',
			@pagesize 
		);
	
	SET @countsql = "SELECT FOUND_ROWS()";
	PREPARE strsql 
	FROM
		@strsql;
	EXECUTE strsql;
	PREPARE countsql 
	FROM
		@countsql;
	EXECUTE countsql;

END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
