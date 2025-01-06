[简体中文](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/README.md) | [English](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/README-en.md)

**🚀 基于.NET 8使用Semantic Kernel为核心开发，支持国产信创，高效构建AI Agent 应用**

**🔧 功能概述**
AgentBrook 是一款面向 AI 应用软件开发者的快速开发框架系统，它拥有以下核心功能：
* **多数据库支持**: 内置对 MySQL、PostgreSQL、SQL Server 等主流数据库的支持，无需担心数据库兼容性问题。
* **框架模板化**: 允许开发者自定义生成框架模板，提高代码可读性和可维护性，减少重复代码编写。
* **高效无代码开发**: 通过无代码操作界面，开发者可以快速定义 AI Agent 的工作流，无需关注基础代码，提高开发效率。
* **模块化设计**: 系统采用模块化设计，易于维护和升级，提升开发效率。
* **大模型接入**: 支持对接 Ollama 等离线大模型平台，以及 OpenAI、智谱、通义千问 等线上大模型平台。
* **工作流编排**: 提供丰富的功能卡片，方便开发者灵活定义应用和智能体工作流，满足各类业务需求。
* **插件管理**: 开发者可以根据业务需求开发插件，为大模型提供外部感知与操控能力。
* **系统助手**: 提供示例系统助手，方便开发者参考配置，快速构建功能完善的 AI 应用。
* **开发日志管理**: 方便开发者预埋日志提取点位，查看运行日志信息，进行调试和监控。

**🖥️ 部署环境**
* 操作系统: Windows、Linux、Mac

**📦 使用说明**
1. **系统部署**: 根据部署环境选择发布版本类型，运行发布后的文件即可。
2. **系统操作**: 登录系统后，可以通过系统提供的各个模块进行功能配置和使用，例如：
    * **字典管理**: 维护系统开发使用的数据字典。
    * **系统菜单管理**: 管理系统菜单的映射关系。
    * **系统用户管理**: 管理系统账号和权限。
    * **模型管理**: 配置本地或线上大模型。
    * **知识库管理**: 上传和处理知识库文件，为大模型问答提供数据支持。
    * **插件管理**: 开发和管理插件，为大模型提供外部能力。
    * **应用管理**: 创建和管理 AI Agent 应用，定义应用工作流。
    * **智能体管理**: 创建和管理智能体，定义智能体工作流。
    * **系统助手**: 参考示例系统助手，快速构建 AI 应用。
    * **开发日志管理**: 预埋日志提取点位，查看运行日志信息，进行调试和监控。
      
**🚧 注意事项**
* 确保服务端系统版本符合要求，避免影响使用体验。
* 使用在线大模型时，请注意调用费用，并确保预存足够余额。
* 当选择的数据表数量较多时，系统可能会出现短暂卡顿现象，请耐心等待。
  
***
**💡从一个最简单的AI助手开始**
[应用工作流](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/pic_001.png)
[Agent任务工作流](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/pic_002.png)
[Chat效果](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/pic_003.png)

***
**💡开启AgentBrook之旅**
1. **准备工作**
    * **数据库**：系统默认使用了MySQL:8，Postgres:16.6+pgvector，Redis:5.0.10，可以修改项目appsettings.json文件的DbConnectionStrings。
    可能看到DbConnectionStrings你会吓一跳，怎么会有那么多数据库，在意的话可以把MySQL的数据库合并到一起。其它参数根据自己实际情况修改调整即可。
    * **大模型**：应为使用的是Semantic Kernel，极大程度方便了大模型接入，理论上支持各类兼容OpenAI接口的大模型。
    * **账号密码**：使用SQL建库会自动添加好一个管理员账号：admin，密码：1q2w3e

**💕 感谢**
    感谢[Semantic Kernel](https://github.com/microsoft/semantic-kernel),提供了一个强大好用的框架。
    感谢[AntSK](https://github.com/AIDotNet/AntSK)，项目初期学习参考了AntSK对Semantic Kernel的使用。


**🌟 加入我们**
AgentBrook 旨在帮助开发者快速构建、部署和维护 AI 应用程序，降低开发门槛，提高开发效率。欢迎广大开发者加入我们，共同推动 AI 产业发展！
更多内容请访问[AgentBrook.com](https://agentbrook.com/)
