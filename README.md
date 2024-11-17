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
- .NET Core
- Swagger
- Entity Framework Core
- Mysql