/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_member

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:18:31
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_member;

-- ----------------------------
-- Table structure for tb_member_auth_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_member_auth_info`;
CREATE TABLE `tb_member_auth_info` (
  `MemberAuthID` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '会员登录信息ID',
  `MemberID` varchar(64) NOT NULL COMMENT '会员编号',
  `AccessToken` varchar(256) NOT NULL COMMENT 'AccessToken',
  `RefreshToken` varchar(256) NOT NULL COMMENT 'RefreshToken',
  `maAppendTime` datetime NOT NULL COMMENT '创建时间',
  `maUpdateTime` datetime NOT NULL COMMENT '更新时间',
  PRIMARY KEY (`MemberAuthID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_member_auth_info
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for tb_member_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_member_info`;
CREATE TABLE `tb_member_info` (
  `MemberID` varchar(64) NOT NULL COMMENT '会员编号',
  `MemberCard` varchar(50) NOT NULL COMMENT '会员卡卡号',
  `mPhoneNumber` varchar(50) NOT NULL COMMENT '手机号码',
  `mNickName` varchar(50) NOT NULL COMMENT '昵称',
  `mPWD` varchar(32) NOT NULL COMMENT '密码',
  `mIcon` varchar(128) NOT NULL COMMENT '头像',
  `mBirthday` date NOT NULL COMMENT '出生年月',
  `mState` int(11) NOT NULL COMMENT '系统状态',
  `mPoints` int(11) NOT NULL COMMENT '目前积分',
  `mLevel` int(11) NOT NULL COMMENT '目前等级',
  `mIntroducer` varchar(64) DEFAULT '' COMMENT '介绍人ID',
  `mAppendTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`MemberID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_member_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_member_info` (`MemberID`, `MemberCard`, `mPhoneNumber`, `mNickName`, `mPWD`, `mIcon`, `mBirthday`, `mState`, `mPoints`, `mLevel`, `mIntroducer`, `mAppendTime`) VALUES ('9e38a46a3a04ff6c7a9a7c360edacc5d', '11', '22', '33', '550e1bafe077ff0b0b67f4e32f29d751', '44', '2024-07-18', 0, 0, 0, '66', '2024-07-18 09:01:47');
COMMIT;

-- ----------------------------
-- Table structure for tb_member_other_auth_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_member_other_auth_info`;
CREATE TABLE `tb_member_other_auth_info` (
  `MemberOtherAuthID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统编号',
  `MemberID` varchar(64) NOT NULL COMMENT '用户编号',
  `Server_Type` int(11) NOT NULL COMMENT '第三方服务类型',
  `Server_ID` int(11) DEFAULT '0' COMMENT '第三方服务ID',
  `User_Nickname` varchar(50) DEFAULT NULL COMMENT '对应服务的用户名，昵称',
  `OpenID` varchar(255) DEFAULT NULL COMMENT 'OpenID',
  `UnionID` varchar(255) DEFAULT NULL COMMENT '联合ID',
  `Session_Key` varchar(255) DEFAULT NULL COMMENT '小程序SessionKey',
  `AccessToken` varchar(255) DEFAULT NULL COMMENT 'AccessToken',
  `RefreshToken` varchar(255) DEFAULT NULL COMMENT 'RefreshToken',
  `Phone` varchar(50) DEFAULT NULL COMMENT '手机号码',
  `Sex` int(11) NOT NULL COMMENT '性别',
  `Language` varchar(50) DEFAULT 'Zh_cn' COMMENT '语言',
  `Country` varchar(100) DEFAULT NULL COMMENT '国家',
  `Province` varchar(100) DEFAULT NULL COMMENT '省份',
  `City` varchar(100) DEFAULT NULL COMMENT '城市',
  `Region` varchar(100) DEFAULT NULL COMMENT '地区',
  `Head_img` varchar(255) DEFAULT NULL COMMENT '头像',
  `Subscribe` int(11) NOT NULL COMMENT '关注类型',
  `Remake` varchar(255) DEFAULT NULL COMMENT '备注',
  `Append_Time` datetime NOT NULL COMMENT '创建时间',
  `Update_Time` datetime NOT NULL COMMENT '更新时间',
  PRIMARY KEY (`MemberOtherAuthID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_member_other_auth_info
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
