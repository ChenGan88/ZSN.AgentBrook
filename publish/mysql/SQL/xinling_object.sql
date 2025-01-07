/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_object

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:19:20
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_object;

-- ----------------------------
-- Table structure for tb_files_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_files_info`;
CREATE TABLE `tb_files_info` (
  `FileCode` varchar(64) NOT NULL COMMENT '文件编号',
  `fFilePath` varchar(256) DEFAULT NULL COMMENT '文件存放地址',
  `fName` varchar(128) DEFAULT NULL COMMENT '文件名',
  `fOriginName` varchar(128) DEFAULT NULL COMMENT '原文件名',
  `fType` varchar(128) DEFAULT NULL COMMENT '文件类型',
  `fAppendTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`FileCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='文件信息';

-- ----------------------------
-- Records of tb_files_info
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
