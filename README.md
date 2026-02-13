# 🧪 Teste Técnico - Dev Fullstack (.NET/C#) - Festpay

## 🎯 Objetivo

Construir e manter uma api em .NET 9 utilizando o padrão CQRS afim de manter um sistema de contas e transações da Festpay. Utilizando dos métodos já existentes, construa a entidade de Transações e o seu respectivo CRUD.
A entidade deve herdar a entidade base e possuir os seguintes dados:

- **Conta de destino**
- **Conta de origem**
- **Valor**
- **Cancelada**

Deverá ser desenvolvido métodos para:

- **Buscar todas as transações**
- **Buscar uma transação pelo Id**
- **Inserir uma transação**
- **Cancelar uma transação**

---

**ATENÇÃO** - Não se esqueça de desenvolver os testes de domínio e testes de aplicação.

---

## 🧱 Critérios de Avaliação

- Separação das regras de domínio e regras de aplicação
- Estrutura e funcionalidade do código existente e do código redigido
- Uso correto da arquitetura definida no projeto
- Princípios SOLID
- Tratamento de exceções
- Código limpo e organizado

---

## 📤 Entrega

- Criar um fork do projeto e submetê-lo com as implementações
- Atualizar o README com:
  - Tecnologias utilizadas
  - Instruções para rodar o projeto
- As instruções para envio do projeto deverão seguir as orientações enviadas pelo recrutador.

---

## 🛠️ Tecnologias Utilizadas

A API foi construída com as seguintes tecnologias:
| Categoria           | Tecnologias / Versões                                      |
| ------------------- | ---------------------------------------------------------- |
| Plataforma          | **.NET 9**                                                 |
| API                 | **ASP.NET Core 9.0.4**                                     |
| Persistência        | **Entity Framework Core 9.0.4**                            |
| Banco de Dados      | **SQLite**                                                 |
| Documentação de API | **Swagger / OpenAPI** (via `Microsoft.AspNetCore.OpenApi`) |
| Testes              | **xUnit / Moq**                                            |

---

## ▶ Executando localmente
1 Clone o repositório:

`git clone https://github.com/degar405/festpay-onboarding-api.git`

`cd festpay-onboarding-api`


2 Restaure dependências:

`dotnet restore`


3 Inicie a API:

`dotnet run --project Festpay.Onboarding.Api`


4 Acesse o Swagger:

https://localhost:7266/swagger/index.html

### Criando Migrations

`dotnet ef migrations add NewMigration -p Festpay.Onboarding.Infra`

As migrations são aplicadas automaticamente ao executar a aplicação localmente.
