/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_app

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:16:32
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_app;

-- ----------------------------
-- Table structure for tb_apisettings_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_apisettings_info`;
CREATE TABLE `tb_apisettings_info` (
  `ApiID` int(11) NOT NULL AUTO_INCREMENT COMMENT 'API编号',
  `MemberID` varchar(64) NOT NULL COMMENT '会员编号',
  `AppID` varchar(64) NOT NULL COMMENT '对接用AppID',
  `SecretKey` varchar(64) NOT NULL COMMENT '秘钥',
  `SettingName` varchar(50) NOT NULL COMMENT '配置名称',
  `Remark` varchar(512) DEFAULT NULL COMMENT '配置备注信息',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL COMMENT '更新时间',
  PRIMARY KEY (`ApiID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_apisettings_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_apisettings_info` (`ApiID`, `MemberID`, `AppID`, `SecretKey`, `SettingName`, `Remark`, `CreateTime`, `UpdateTime`) VALUES (1, '1', '15c5fb785f4ce8a99717ea85f29690c9', '', 'test', '', '2024-10-23 14:15:12', '2025-01-04 08:09:57');
COMMIT;

-- ----------------------------
-- Table structure for tb_app_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_app_info`;
CREATE TABLE `tb_app_info` (
  `AppID` varchar(64) NOT NULL COMMENT '系统编号',
  `Name` varchar(128) DEFAULT NULL COMMENT '名称',
  `AICON` varchar(64) DEFAULT NULL COMMENT ' 图标',
  `MemberID` varchar(64) DEFAULT NULL COMMENT '所属会员编号',
  `MemberName` varchar(128) DEFAULT NULL COMMENT '会员名称',
  `DicIDList` varchar(512) DEFAULT NULL COMMENT '类型标签ID',
  `DicNameList` varchar(1024) DEFAULT NULL COMMENT '类型标签名称',
  `Description` varchar(512) DEFAULT NULL COMMENT '说明',
  `SystemStatus` int(11) DEFAULT NULL COMMENT '系统状态',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `SessionModelID` int(11) DEFAULT NULL COMMENT '综合能力大模型，用于主导App',
  `SessionModelName` varchar(50) DEFAULT NULL,
  `Prompt` text COMMENT '系统级提示词',
  `TemperatureCoefficient` int(11) DEFAULT NULL COMMENT '温度系数',
  `TopPCoefficient` int(11) DEFAULT NULL COMMENT '采用系数',
  PRIMARY KEY (`AppID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_app_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_app_info` (`AppID`, `Name`, `AICON`, `MemberID`, `MemberName`, `DicIDList`, `DicNameList`, `Description`, `SystemStatus`, `CreateTime`, `LastUpdateTime`, `SessionModelID`, `SessionModelName`, `Prompt`, `TemperatureCoefficient`, `TopPCoefficient`) VALUES ('15c5fb785f4ce8a99717ea85f29690c9', '系统助手', '2dda310095491fb79abed13d57566fdd', '', '', '20', '企业知识库', '系统助手是管理后台辅助工具，其作为系统级的AI智能体，承担着维护后台正常运行的关键职责。\n\n请注意，切勿对其进行删除操作，因为一旦删除，将导致无法在后台正常启用系统助手，进而影响整个系统的稳定性和功能性。\n\n因此，请妥善保留该系统级AI，以确保后台管理工作的顺畅进行。', 2, '2024-07-22 12:57:25', '2024-07-22 12:57:02', 6, '千问2.5-14B', '你的名字叫：AgentBrook助手。\n你是一个系统使用助手，是一个热情又专业的助手。\n你的主要工作是向正在使用AgentBrook系统的用户提供帮助。\n\n#比如：用户会向你提问（关于系统的具体功能的使用），你可以调用知识库的方式取得答案，并组织反馈给用户。如果没有找到相应的内容请不要随意回答，你可以直接回答（这个问题知识库中还没找到，我将记录下来。）\n\n#注意：你的回答仅限于本系统的功能说明，操作步骤与解释。超过该范围的问题，都不能回答。\n#注意：每次都需要快速思考，可以从对话上下文中寻找已经回答的内容；回答内容需要简洁、专业。', 80, 80);
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
