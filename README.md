# FiapGrupo57Fase2

## Descrição
Este projeto é uma aplicação WebAPI desenvolvida pelo grupo 57 da FIAP. O objetivo é implementar uma arquitetura robusta e escalável, seguindo as melhores práticas de desenvolvimento de software.

## Estrutura do Projeto
O projeto está organizado em seis camadas para garantir o isolamento da lógica de domínio, desacoplamento e flexibilidade de adaptação. As principais camadas incluem:

- **Domain**: Inclui as entidades e interfaces de domínio.
- **DTO**: Contém os objetos de transferência de dados utilizados para comunicação entre as camadas.
- **Infrastructure**: Implementa os repositórios e outras dependências externas.
- **Repository**: Implementa os padrões de repositório para acesso a dados.
- **Service**: Contém a lógica de negócios e serviços da aplicação.
- **WebAPI**: Contém os controladores e configurações da WebAPI.

## Tecnologias Utilizadas
- .NET Core 8
- Swagger
- Dapper
- Mysql


## Passo a passo de como instalar prometheus e grafana

### Prometheus
1. Baixar prometheus https://github.com/prometheus/prometheus/releases/download/v3.0.1/prometheus-3.0.1.windows-amd64.zip
2. Extrair na pasta C:\Prometheus\prometheus-3.0.1.windows-amd64
3. Executar o comando PS C:\Prometheus\prometheus-3.0.1.windows-amd64> .\prometheus.exe
4. Abrir o navegador e executar http://localhost:9090/

### Prometheus
1. Baixar grafana https://dl.grafana.com/enterprise/release/grafana-enterprise-11.4.0.windows-amd64.zip
2. Extrair na pasta C:\Grafana\grafana-v11.4.0
3. Executar o comando PS C:\Grafana\grafana-v11.4.0\bin> .\grafana-server.exe
4. Abrir o navegador e executar http://localhost:3000/login

### Windows Exporter
1. Baixar promethues windows exporter https://github.com/prometheus-community/windows_exporter/releases/download/v0.30.0-rc.2/windows_exporter-0.30.0-rc.2-amd64.exe
2. Gravar na pasta C:\Prometheus\windows_exporter\windows_exporter-0.30.0-rc.2-amd64.exe
