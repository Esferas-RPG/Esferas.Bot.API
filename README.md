# Esferas.Bot.API

**Esferas.Bot.API** é a API backend que alimenta o bot do Discord [Esferas-Bot](https://github.com/Esferas-RPG/Esferas-Bot). Desenvolvida com .NET 8, ela é responsável por armazenar, processar e expor os dados do universo de roleplay *Esferas D\&D 2024*, integrando-se com diversas funcionalidades de gerenciamento e automação.

## 🚀 Tecnologias Utilizadas

* ASP.NET Core 8
* Google Sheets API

## 📁 Estrutura

* `Controllers/`: Endpoints REST
* `Services/`: Lógicas de negócio
* `Data/`: Contexto e migrations do EF Core
* `Models/`: Entidades e DTOs
* `Configurations/`: Setup de dependências, políticas CORS, JWT etc.

## 🛠️ Requisitos

* .NET 8 SDK
* Conta de serviço com acesso à Google Sheets API

## ⚙️ Instalação e Execução

1. Clone o projeto:

```bash
git clone https://github.com/Esferas-RPG/Esferas.Bot.API.git
cd Esferas.Bot.API
```

2. Adicione a credencial da Google API (Service Account) [Criar credenciais](https://developers-google-com.translate.goog/workspace/guides/create-credentials?_x_tr_sl=en&_x_tr_tl=pt&_x_tr_hl=pt&_x_tr_pto=tc)
   
Crie um arquivo `credentials.json` (baixe do Google Cloud Console) e coloque na raiz do projeto (ou caminho configurado no seu serviço).

Exemplo:

```json
{
  "type": "service_account",
  "project_id": "...",
  ...
}
```

3. Crie um arquivo `.env` com as seguintes variáveis de ambiente:

```env
CLIENT_CREDENTIONS_JSON_PATH=<CAMINHO_PARA_O_ARQUIVO_DE_CREDENCIAL>
SHEET_TEMPLATE_ID=<TEMPLATE_DE_FICHA>
FOLDER_ID=<PASTA_DE_PERSONAGENS>
APP_SCRIPT_ID_ADD_COMMENTS=<ID_PARA_COMENTARIOS>
PLAYER_DATA_BASE_ID=<ID_DADOS_DAS_FICHAS_SALVAS>
ROOT_FOLDER_ID=<PASTA_RAIZ_DO_PROJETO>
```

Essas variáveis são essenciais para o funcionamento da integração com a Google API e devem ser mantidas em segurança.

4. Execute o projeto:

```bash
dotnet run
```

## 🐳 Docker

Para rodar via Docker:

```bash
docker build -t esferas-api .
docker run -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Development esferas-api
```

**Lembre-se:** mapeie o volume com o `credentials.json` e passe o `.env` como variável de ambiente

## 🧪 Testes

Utilize os endpoints com ferramentas como Postman ou Insomnia.

## 📄 Licença

MIT © 2024 Esferas RPG
