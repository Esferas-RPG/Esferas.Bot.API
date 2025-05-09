# Esferas.Bot.API

Esferas.Bot.API é uma API REST desenvolvida com ASP.NET Core para dar suporte ao servidor de roleplay *Esferas D\&D 2024*. Ela fornece endpoints seguros e organizados para gestão de personagens, eventos, fichas e outras funcionalidades integradas com o bot do Discord.

## 🚀 Funcionalidades

* Gerenciamento de personagens e fichas
* Controle de eventos RP
* Autenticação de usuários
* Integração direta com o bot Esferas-Bot
* Documentação da API via Swagger (OpenAPI)

## 📆 Instalação

### Requisitos

* [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* Docker (opcional, para execução em container)
* Banco de dados PostgreSQL

### Passos para execução local

```bash
git clone https://github.com/Esferas-RPG/Esferas.Bot.API.git
cd Esferas.Bot.API
dotnet restore
dotnet build
dotnet run
```

A aplicação estará acessível em: `https://localhost:5001`

## 🚧 Uso com Docker

1. Configure um arquivo `.env` com as variáveis necessárias (conexão com banco, chaves etc.)
2. Execute:

```bash
docker-compose up -d
```

3. Para parar:

```bash
docker-compose down
```

## 🔍 Endpoints principais

* `GET /api/personagens` - Lista personagens
* `POST /api/personagens` - Cria personagem
* `GET /api/eventos` - Lista eventos ativos
* `POST /api/eventos` - Cria novo evento
* `POST /api/autenticacao/login` - Login de usuário

Acesse a documentação interativa em `/swagger` quando o projeto estiver rodando.

## 📄 Licença

Este projeto está licenciado sob a MIT License. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

> "Em Esferas, o mundo muda com cada escolha — até as que você faz aqui, no código."
