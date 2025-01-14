**🚀 Developed with .NET 8 and powered by Semantic Kernel, supporting domestic innovation, efficiently build AI Agent applications**
**🔧 Overview of Functions**
AgentBrook is a rapid development framework system for AI application software developers. It has the following core functions:
* **Multi-database support**: Built-in support for mainstream databases such as MySQL, PostgreSQL, SQL Server, etc., without worrying about database compatibility issues.
* **Framework templating**: Allows developers to customize the generation of framework templates to improve code readability and maintainability, reducing the need for repetitive code writing.
* **Efficient no-code development**: Through a no-code operation interface, developers can quickly define the workflow of AI Agents without having to worry about basic code, improving development efficiency.
* **Modular design**: The system adopts a modular design, which is easy to maintain and upgrade, improving development efficiency.
* **Large model access**: Supports connection to offline large model platforms such as Ollama, as well as online large model platforms such as OpenAI, Zhipu, and Tongyi Qianwen.
* **Workflow orchestration**: Provides a rich set of functional cards, making it easy for developers to flexibly define application and intelligent agent workflows to meet various business needs.
* **Plugin management**: Developers can develop plugins according to business needs to provide external perception and control capabilities for large models.
* **System assistant**: Provides sample system assistants for developers to reference and configure, quickly building functional AI applications.
* **Development log management**: It is convenient for developers to pre-embed log extraction points, view runtime log information, and debug and monitor.
**💡 Starting Your AgentBrook Journey**
1. **Preparation**: Choose the release version type according to the deployment environment and run the released file. You can refer to the specific [**help documentation**](https://agentbrook.com/docs/examples/index.html)
2. **Prerequisites**
    * **Database**: The system defaults to MySQL:8, Postgres:16.6+pgvector, Redis:5.0.10, you can modify the DbConnectionStrings in the project appsettings.json file. You may be surprised to see DbConnectionStrings, why are there so many databases? If you are concerned, you can merge the MySQL databases together. Adjust other parameters according to your actual situation.
    * **Large model**: Because it uses Semantic Kernel, it greatly facilitates the access to large models, and theoretically supports various large models compatible with the OpenAI interface.
    * **Account password**: Using [**`SQL`**](https://github.com/ChenGan88/ZSN.AgentBrook/tree/main/DB_SQL/mysql) to build the library will automatically add an administrator account: **`admin`**, password: **`1q2w3e`**
**🐳 Docker**
**Docker Compose**
1. Enter the [`publish`](https://github.com/ChenGan88/ZSN.AgentBrook/tree/main/publish) folder
2. Run `docker-compose up --build`
3. Open [http://localhost:5002](http://localhost:5002)
4. Account: **`admin`**, password: **`1q2w3e`**
5. You can stop the container with `docker compose stop`
**📦 Instructions for Use**
1. **System operation**: After logging in to the system, you can configure and use various functions provided by the system, such as:
    * **Dictionary management**: Maintain data dictionaries used in the system.
    * **System menu management**: Manage the mapping relationship of system menus.
    * **System user management**: Manage system accounts and permissions.
    * **Model management**: Configure local or online large models.
    * **Knowledge base management**: Upload and process knowledge base files to provide data support for large model Q&A.
    * **Plugin management**: Develop and manage plugins to provide external capabilities for large models.
    * **Application management**: Create and manage AI Agent applications and define application workflows.
    * **Intelligent agent management**: Create and manage intelligent agents and define intelligent agent workflows.
    * **System assistant**: Refer to the example system assistant to quickly build AI applications.
    * **Development log management**: Pre-embed log extraction points, view runtime log information, and debug and monitor.
**🚧 Notes**
* Ensure that the server system version meets the requirements to avoid affecting the user experience.
* When using online large models, please pay attention to the call fees and ensure that sufficient balance is prepaid.
* When choosing a large number of data tables, the system may experience a brief lag, please be patient and wait.
**💡 Start with the simplest AI assistant**
![Application Workflow](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/pic_001.png)
![Agent Task Workflow](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/pic_002.png)
![Chat Effect](https://github.com/ChenGan88/ZSN.AgentBrook/blob/main/README/pic_003.png)
**💕 Acknowledgements**
   Thank you [**Semantic Kernel**](https://github.com/microsoft/semantic-kernel) for providing a powerful and easy-to-use framework.
   Thank you [**AntSK**](https://github.com/AIDotNet/AntSK) for the reference and learning of AntSK's use of Semantic Kernel in the early stages of the project.
**🌟 Join Us**
AgentBrook aims to help developers quickly build, deploy, and maintain AI applications, reduce the threshold for development, and improve development efficiency. We welcome developers from all over the world to join us and work together to promote the development of the AI industry!
For more information, please visit [AgentBrook.com](https://agentbrook.com/)
