# Wenli.Config.ApolloClient

[![NuGet version (Wenli.Data.Es)](https://img.shields.io/nuget/v/Wenli.Data.Es.svg?style=flat-square)](https://www.nuget.org/packages?q=Wenli.Config.ApolloClient)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)


Wenli.Config.ApolloClient 是根据了解官方驱动流程后，结合项目实际情况重构的apollo配置中心客户端，主要解决以下几个问题：
 1.无需在c盘配置目录 
 2.可以连接多个apollo服务中心、每一个中心可以使用多个appid的配置 
 3.不再使用onchange事件来通知更新，而是增加实体类生成工具，用类的属性来实现自动更新，以方便于项目中使用 
 4.增加更加详细的配置，以适应不同项目环境的需求，比如长轮询时间、次数等 5.根据实际情况修改配置读取顺序为：优先使用appsetting中的配置、其次使用apolloclient本地配置，最后使用apolloservice的配置
