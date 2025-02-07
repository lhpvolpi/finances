# Fluxo de Caixa - Documentação

## Visão Geral

Este projeto tem como objetivo desenvolver um sistema escalável e resiliente para controle de fluxo de caixa. Ele permite o cadastro de usuários, login, criação de transações (créditos e débitos) e a geração de um saldo consolidado diário.

A solução foca em escalabilidade, resiliência e desempenho, utilizando tecnologias como Redis para cache e fila de processamento para consolidação dos dados.

## Tecnologias Utilizadas

- **Linguagem:** C#
- **Banco de Dados:** SQL Server
- **Cache e Mensageria:** Redis
- **Framework Web:** ASP.NET Core 8
- **Autenticação:** JWT
- **Arquitetura:** Clean Architecture e Microsserviços

## Arquitetura

A aplicação é composta por dois principais serviços:

### 1. Serviço de Lançamentos

- Responsável por cadastrar transações financeiras (créditos e débitos)
- Expõe um endpoint REST para criação de transações.
- Publica eventos de transações na fila do Redis para processamento assíncrono.

### 2. Serviço de Consolidação

- Escuta eventos de transações em uma fila do Redis.
- Processa e salva o saldo consolidado no SQL Server e no Redis.
- Otimiza as consultas retornando dados do Redis primeiro.
- Caso o saldo não seja encontrado no Redis, busca no SQL Server, salva no Redis e retorna o resultado.

## Desenho da Solução

```plaintext
+--------------------+        +-----------------------+        +------------------------+
| Serviço de Usuário |        | Serviço de Lançamentos|        | Serviço de Consolidação|
|   (Autenticação)   | -----> |  (Criação de Trans.)  | -----> | (Fila de Processamento)|
+--------------------+        +-----------------------+        +------------------------+
                                                          |
                                                          v
                                                +----------------------+
                                                | Banco de Dados (SQL) |
                                                +----------------------+
                                                          |
                                                          v
                                                +----------------------+
                                                |        Redis         |
                                                | (Cache Consolidado)  |
                                                +----------------------+
                                                          |
                                                          v
                                                +-----------------------+
                                                | Interface REST (API)  |
                                                +-----------------------+
```

## Endpoints

### Usuários

- `POST /api/v1/users` - Registra um novo usuário
- `POST /api/v1/users/login` - Realiza login e retorna um token JWT

### Transações

- `POST /api/v1/transactions` - Cria uma nova transação

### Consolidação

- `GET /api/v1/consolidations?date=2025-02-06` - Retorna o saldo consolidado do usuário
  - Primeiro consulta no Redis, se não encontrar, busca no SQL Server e salva no Redis para otimizar acessos futuros.

## Execução Local

### Requisitos

- Docker

### Comandos para executar em modo produção

- `cd src`
- `docker-compose up -d`
- Documentação Swagger: [http://localhost:5050/swagger/index.html](http://localhost:5050/swagger/index.html)
- Status da infraestrutura (SQL Server e Redis): [http://localhost:5050/health-details](http://localhost:5050/health-details)

### Comandos para executar em modo debug (apenas Redis e SQL Server)

- `cd src`
- `docker-compose -f docker-compose.debug.yml up -d`
- Execute o projeto no Visual Studio ou no VS Code.

### Configuração do Banco de Dados

Em ambos os cenários de execução (produção e debug), é necessário rodar o script SQL que cria a base de dados antes de iniciar a aplicação.
